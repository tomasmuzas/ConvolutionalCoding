using ConvolutionalCodes.Controllers;
using ConvolutionalCodes.Encoders;
using ConvolutionalCodes.Entities;
using ConvolutionalCodes.Utilities;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace ConvolutionalCodes
{
    public partial class Application : Form
    {
        public Application()
        {
            InitializeComponent();
        }

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

        private void DisplayTextResult(string original, StringResult unencoded, StringResult encoded)
        {
            encodingResultPanel.Controls.Clear();
            
            encodingResultPanel.Controls.Add(UIHelper.CreateLabelWithText("Initial Text: " + original));
            encodingResultPanel.Controls.Add(UIHelper.CreateLabelWithText("Unencoded Text: " + unencoded.Result));
            encodingResultPanel.Controls.Add(UIHelper.CreateLabelWithText("Encoded Text: " + encoded.Result));

            DisplayErrors(unencoded.Errors, encoded.Errors);
        }


        private void DisplayImageResult(Bitmap original, ByteArrayResult unencoded, ByteArrayResult encoded)
        {
            encodingResultPanel.Controls.Clear();

            var unencodedImage = BitmapHelper.SetImageBytes(original, unencoded.Result);
            var encodedImage = BitmapHelper.SetImageBytes(original, encoded.Result);

            encodingResultPanel.Controls.Add(UIHelper.CreateImagePanel(original));
            encodingResultPanel.Controls.Add(UIHelper.CreateImagePanel(unencodedImage));
            encodingResultPanel.Controls.Add(UIHelper.CreateImagePanel(encodedImage));

            DisplayErrors(unencoded.Errors, encoded.Errors);
        }

        private async void TextSubmit_Click(object sender, EventArgs e)
        {
            var initialText = TextInput.Text;

            string channelNoiseText = ChannelNoiseInput.Text;

            if (!double.TryParse(channelNoiseText, out var noise) || noise < 0 || noise > 1 )
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
            if (!double.TryParse(channelNoiseText, out var noise) || noise < 0 || noise > 1)
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
