using ConvolutionalCodes.Controllers;
using ConvolutionalCodes.Encoders;
using ConvolutionalCodes.Entities;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace ConvolutionalCodes
{
    public partial class Application : Form
    {
        public Application()
        {
            InitializeComponent();
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
            encodingResultPanel.Controls.Add(CreateLabelWithText("Initial Text: " + initialText));
            encodingResultPanel.Controls.Add(CreateLabelWithText("Unencoded Text: " + unencodedText));
            encodingResultPanel.Controls.Add(CreateLabelWithText("Encoded Text: " + encodedText));
        }

        private Label CreateLabelWithText(string text)
        {
            var label = new Label
            {
                AutoSize = true,
                Text = text,
                MaximumSize = new Size(200, 0)
            };
            return label;
        }

        private FlowLayoutPanel CreateImagePanel(Bitmap bmp)
        {
            var layout = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.TopDown,
                AutoSize = true,
                MaximumSize = new Size(200, 0),
                WrapContents = true
            };
            var pictureBox = new PictureBox
            {
                Image = bmp,
                Width = bmp.Width,
                Height = bmp.Height
            };

            layout.Controls.Add(pictureBox);

            return layout;
        }

        public byte[] GetColorBytes(Bitmap bmp)
        {
            // Lock the bitmap's bits.  
            Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            BitmapData bmpData =
                bmp.LockBits(rect, ImageLockMode.ReadWrite,
                bmp.PixelFormat);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare an array to hold the bytes of the bitmap.
            int bytes = Math.Abs(bmpData.Stride) * bmp.Height;
            byte[] rgbValues = new byte[bytes];

            // Copy the RGB values into the array.
            System.Runtime.InteropServices.Marshal.Copy(ptr, rgbValues, 0, bytes);

            bmp.UnlockBits(bmpData);
            return rgbValues;
        }

        public Bitmap SetImageBytes(Bitmap original, byte[] imageBytes)
        {
            // Create bitmap.
            Bitmap bmp = (Bitmap)original.Clone();

            // Lock the bitmap's bits.  
            Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            BitmapData bmpData =
                bmp.LockBits(rect, ImageLockMode.ReadWrite,
                bmp.PixelFormat);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Copy the RGB values back to the bitmap
            System.Runtime.InteropServices.Marshal.Copy(imageBytes, 0, ptr, imageBytes.Length);

            // Unlock the bits.
            bmp.UnlockBits(bmpData);

            return bmp;
        }

        [STAThread]
        private async void uploadImageButton_Click(object sender, EventArgs e)
        {
            encodingResultPanel.Controls.Clear();
            encodingResultPanel.Controls.Add(CreateLabelWithText("Loading images... Please wait."));
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

            var originalImage = new Bitmap(dialog.FileName);
            var scaledHeight = originalImage.Height / ((double)originalImage.Width / 200);

            var scaledImage = new Bitmap(originalImage, new Size(200, (int)scaledHeight));
            originalImage.Dispose();

            var imageBytes = GetColorBytes(scaledImage);

            string channelNoiseText = ChannelNoiseInput.Text;
            if (!double.TryParse(channelNoiseText, out var noise) || noise < 0 || noise > 1)
            {
                MessageBox.Show("Noise must be a number between 0 and 1.");
                return;
            }

            var channel = new NoisyChannel(noise);

            var unencodedResult = await MessageController.SendBytes(imageBytes, channel);
            var unencodedImageBytes = unencodedResult.Result;
            var unencodedErrors = unencodedResult.Errors;
            var unencodedImage = SetImageBytes(scaledImage, unencodedImageBytes);


            var encodedResult = await MessageController.SendBytes(
                imageBytes,
                channel,
                new ConvolutionalEncoder(),
                new ConvolutionalDecoder());
            var encodedImageBytes = encodedResult.Result;
            var encodedErrors = encodedResult.Errors;
            var encodedImage = SetImageBytes(scaledImage, encodedImageBytes);

            encodingResultPanel.Controls.Clear();
            encodingResultPanel.Controls.Add(CreateImagePanel(scaledImage));
            encodingResultPanel.Controls.Add(CreateImagePanel(unencodedImage));
            encodingResultPanel.Controls.Add(CreateImagePanel(encodedImage));
                
            EncodedErrorsText.Text = encodedErrors.ToString();
            EncodedErrorsText.ForeColor = encodedErrors > 0? Color.Crimson : Color.ForestGreen;
            UnencodedErrorsText.Text = unencodedErrors.ToString();
            UnencodedErrorsText.ForeColor = unencodedErrors > 0 ? Color.Crimson : Color.ForestGreen;
        }
    }
}
