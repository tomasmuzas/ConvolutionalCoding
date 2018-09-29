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

            var unencodedText = await MessageController.SendText(initialText, channel);

            var encodedText = await MessageController.SendText(
                initialText,
                channel,
                new ConvolutionalEncoder(),
                new ConvolutionalDecoder());

            encodingResultPanel.Controls.Clear();
            encodingResultPanel.Controls.Add(UIHelper.CreateLabelWithText("Initial Text: " + initialText));
            encodingResultPanel.Controls.Add(UIHelper.CreateLabelWithText("Unencoded Text: " + unencodedText));
            encodingResultPanel.Controls.Add(UIHelper.CreateLabelWithText("Encoded Text: " + encodedText));
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

            var originalImage = new Bitmap(dialog.FileName);
            var scaledImage = BitmapHelper.ScaleBitmap(originalImage, desiredWidth: 200);

            var imageBytes = BitmapHelper.GetColorBytes(scaledImage);

            var channel = new NoisyChannel(noise);

            var unencodedResult = await MessageController.SendBytes(imageBytes, channel);
            var unencodedImageBytes = unencodedResult.Result;
            var unencodedErrors = unencodedResult.Errors;
            var unencodedImage = BitmapHelper.SetImageBytes(scaledImage, unencodedImageBytes);


            var encodedResult = await MessageController.SendBytes(
                imageBytes,
                channel,
                new ConvolutionalEncoder(),
                new ConvolutionalDecoder());
            var encodedImageBytes = encodedResult.Result;
            var encodedErrors = encodedResult.Errors;
            var encodedImage = BitmapHelper.SetImageBytes(scaledImage, encodedImageBytes);

            encodingResultPanel.Controls.Clear();
            encodingResultPanel.Controls.Add(UIHelper.CreateImagePanel(scaledImage));
            encodingResultPanel.Controls.Add(UIHelper.CreateImagePanel(unencodedImage));
            encodingResultPanel.Controls.Add(UIHelper.CreateImagePanel(encodedImage));
                
            DisplayErrors(unencodedErrors, encodedErrors);
        }
    }
}
