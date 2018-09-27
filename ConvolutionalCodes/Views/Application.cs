using ConvolutionalCodes.Controllers;
using ConvolutionalCodes.Encoders;
using ConvolutionalCodes.Entities;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
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
                Text = text
            };
            return label;
        }

        private FlowLayoutPanel CreatePanelWithLabel(Bitmap bmp, string text)
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

            //var label = CreateLabelWithText(text);
            //layout.Controls.Add(label);
            layout.Controls.Add(pictureBox);

            return layout;
        }

        public byte[] GetImageBytes(Bitmap bmp)
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

            // Declare an array to hold the bytes of the bitmap.
            int bytes = Math.Abs(bmpData.Stride) * bmp.Height;

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

            DialogResult Action = dialog.ShowDialog();
            if (Action == DialogResult.OK)
            {
                
                Bitmap originalImage = new Bitmap(dialog.FileName);
                var scaledHeight = originalImage.Height / ((double)originalImage.Width / 200);

                Bitmap scaledImage = new Bitmap(originalImage, new Size(200, (int)scaledHeight));
                originalImage.Dispose();

                var imageBytes = GetImageBytes(scaledImage);

                string channelNoiseText = ChannelNoiseInput.Text;
                if (!double.TryParse(channelNoiseText, out var noise) || noise < 0 || noise > 1)
                {
                    MessageBox.Show("Noise must be a number between 0 and 1.");
                    return;
                }

                var channel = new NoisyChannel(noise);

                var unencodedImageBytes = await MessageController.SendBytes(imageBytes, channel);
                var unencodedImage = SetImageBytes(scaledImage, unencodedImageBytes);



                var encodedImageBytes = await MessageController.SendBytes(
                    imageBytes,
                    channel,
                    new ConvolutionalEncoder(),
                    new ConvolutionalDecoder());
                var encodedImage = SetImageBytes(scaledImage, encodedImageBytes);

                encodingResultPanel.Controls.Clear();
                encodingResultPanel.Controls.Add(CreatePanelWithLabel(scaledImage, "Original Image:"));
                encodingResultPanel.Controls.Add(CreatePanelWithLabel(unencodedImage, "Unencoded Transmission:"));
                encodingResultPanel.Controls.Add(CreatePanelWithLabel(encodedImage, "Encoded Transmission:"));
            }
        }
    }
}
