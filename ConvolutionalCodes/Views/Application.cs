using ConvolutionalCodes.Controllers;
using ConvolutionalCodes.Encoders;
using ConvolutionalCodes.Entities;
using ConvolutionalCodes.Utilities;
using System;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConvolutionalCodes
{
    public partial class Application : Form
    {
        public Application()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Displays error information
        /// <para>- errors made by sending unencrypted message through noisy channel compared to original channel</para>
        /// <para>- errors made by sending encrypted message through noisy channel compared to original channel</para>
        /// <para>- percentage of corrected errors using encoding</para>
        /// </summary>
        /// <param name="unencodedErrors">Number of errors in bits compared to original message </param>
        /// <param name="encodedErrors">Number of errors in bits after decoding encoded messsage compared to original message</param>
        private void DisplayErrors(int unencodedErrors, int encodedErrors)
        {
            EncodedErrorsText.Text = encodedErrors.ToString();
            EncodedErrorsText.ForeColor = encodedErrors > 0 ? Color.Crimson : Color.ForestGreen;

            UnencodedErrorsText.Text = unencodedErrors.ToString();
            UnencodedErrorsText.ForeColor = unencodedErrors > 0 ? Color.Crimson : Color.ForestGreen;

            var percentage = MathHelper.CalculatePercentageWithPrecision(encodedErrors, unencodedErrors, 2);
            ErrorsFixedText.Text = percentage.ToString() + '%';
            ErrorsFixedText.ForeColor = percentage <= 50 ? Color.Crimson : Color.ForestGreen;
        }

        /// <summary>
        /// Displays encoded vector, which can be edited
        /// </summary>
        /// <param name="vectorInfo">Encoded vector</param>
        private void DisplayEncodedVector(StringResultWithErrorPositions vectorInfo)
        {
            encodingResultPanel.Controls.Clear();

            var layout = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.TopDown,
                AutoSize = true,
                MaximumSize = new Size(200, 0),
                WrapContents = true
            };

            layout.Controls.Add(UIHelper.CreateLabelWithText("Encoded Vector:"));

            var textBox = UIHelper.CreateTextBox(vectorInfo.Result);
            layout.Controls.Add(textBox);
            var button = UIHelper.CreateButton("Decode", async () =>
            {
                await DecodeAndDisplayVector(textBox.Text);
            });
            layout.Controls.Add(button);
            layout.Controls.Add(UIHelper.CreateLabelWithText($"Errors: {vectorInfo.Errors}, Positions: {string.Join(",", vectorInfo.ErrorPositions)}"));
            encodingResultPanel.Controls.Add(layout);
        }

        /// <summary>
        /// Displays a vector
        /// </summary>
        /// <param name="vector">Vector to display</param>
        private void DisplayDecodedVector(string vector)
        {
            encodingResultPanel.Controls.Add(UIHelper.CreateLabelWithText("Decoded Vector: " + vector));
        }

        /// <summary>
        /// Decode given vector
        /// </summary>
        /// <param name="encodedVectorText"></param>
        private async Task DecodeAndDisplayVector(string encodedVectorText)
        {
            try
            {
                var decodedResult = await MessageController.DecodeVector(encodedVectorText, new ConvolutionalDecoder());
                DisplayDecodedVector(decodedResult.Result);
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        /// <summary>
        /// Displays text messages: original, unencoded and withEncoding
        /// </summary>
        /// <param name="original">Original message</param>
        /// <param name="resultWithoutEncoding">Text result when no encoding was used together with error information</param>
        /// <param name="resultWithEncoding">Text result when encoding was used together with error information</param>
        private void DisplayTextResult(string original, StringResult resultWithoutEncoding, StringResult resultWithEncoding)
        {
            encodingResultPanel.Controls.Clear();
            
            encodingResultPanel.Controls.Add(UIHelper.CreateLabelWithText("Initial Text: " + original));
            encodingResultPanel.Controls.Add(UIHelper.CreateLabelWithText("Unencoded Text: " + resultWithoutEncoding.Result));
            encodingResultPanel.Controls.Add(UIHelper.CreateLabelWithText("Encoded Text: " + resultWithEncoding.Result));

            DisplayErrors(resultWithoutEncoding.Errors, resultWithEncoding.Errors);
        }

        /// <summary>
        /// Displays image messages: original, unencoded and withEncoding
        /// </summary>
        /// <param name="original">Original image</param>
        /// <param name="resultWithoutEncoding">Image bytes when no encoding was used together with error information</param>
        /// <param name="resultWithEncoding">Image bytes when ecoding was used together with error information</param>
        private void DisplayImageResult(Bitmap original, ByteArrayResult resultWithoutEncoding, ByteArrayResult resultWithEncoding)
        {
            encodingResultPanel.Controls.Clear();

            var imageWithoutEncoding = BitmapHelper.SetImageBytes(original, resultWithoutEncoding.Result);
            var imageWithEncoding = BitmapHelper.SetImageBytes(original, resultWithEncoding.Result);

            encodingResultPanel.Controls.Add(UIHelper.CreateImagePanel(original));
            encodingResultPanel.Controls.Add(UIHelper.CreateImagePanel(imageWithoutEncoding));
            encodingResultPanel.Controls.Add(UIHelper.CreateImagePanel(imageWithEncoding));

            DisplayErrors(resultWithoutEncoding.Errors, resultWithEncoding.Errors);
        }

        private async void VectorSubmit_Click(object sender, EventArgs e)
        {
            var vectorText = VectorInput.Text;

            string channelNoiseText = ChannelNoiseInput.Text;

            if (!double.TryParse(channelNoiseText.Replace(',', '.'), out var noise) || noise < 0 || noise > 1)
            {
                MessageBox.Show("Noise must be a number between 0 and 1.");
                return;
            }

            var channel = new NoisyChannel(errorChance: noise);

            var encodedResult = await MessageController.EncodeAndSendVector(vectorText, new ConvolutionalEncoder(), channel);

            DisplayEncodedVector(encodedResult);
        }

        private async void TextSubmit_Click(object sender, EventArgs e)
        {
            var initialText = TextInput.Text;

            string channelNoiseText = ChannelNoiseInput.Text;

            if (!double.TryParse(channelNoiseText.Replace(',', '.'), out var noise) || noise < 0 || noise > 1 )
            {
                MessageBox.Show("Noise must be a number between 0 and 1.");
                return;
            }

            var channel = new NoisyChannel(errorChance: noise);

            var unencodedResult = await MessageController.SendText(initialText, channel);

            var encodedResult = await MessageController.SendText(
                initialText,
                channel,
                new ConvolutionalEncoder(),
                new ConvolutionalDecoder());

            DisplayTextResult(initialText, unencodedResult, encodedResult);
        }

        [STAThread]
        private async void uploadImageButton_Click(object sender, EventArgs e)
        {
            encodingResultPanel.Controls.Clear();
            encodingResultPanel.Controls.Add(UIHelper.CreateLabelWithText("Loading images... Please wait."));
            var dialog = new OpenFileDialog
            {
                ShowHelp = true,
                Filter = "Image Files(*.jpeg;*.bmp;*.png;*.jpg)|*.jpeg;*.bmp;*.png;*.jpg"
            };

            var Response = dialog.ShowDialog();
            if (Response != DialogResult.OK)
            {
                return;
            }

            var channelNoiseText = ChannelNoiseInput.Text;
            if (!double.TryParse(channelNoiseText.Replace(',', '.'), out var noise) || noise < 0 || noise > 1)
            {
                MessageBox.Show("Noise must be a number between 0 and 1.");
                return;
            }

            var channel = new NoisyChannel(noise);

            var originalImage = new Bitmap(dialog.FileName);
            var scaledImage = BitmapHelper.ScaleBitmap(originalImage, desiredWidth: 200);

            var imageBytes = BitmapHelper.GetColorBytes(scaledImage);

            

            var unencodedResult = await MessageController.SendBytes(imageBytes, channel);

            var encodedResult = await MessageController.SendBytes(
                imageBytes,
                channel,
                new ConvolutionalEncoder(),
                new ConvolutionalDecoder());

            DisplayImageResult(scaledImage, unencodedResult, encodedResult);
        }
    }
}
