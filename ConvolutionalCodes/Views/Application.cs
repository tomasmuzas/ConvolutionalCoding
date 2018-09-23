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
            var label = new Label();
            label.AutoSize = true;
            label.MaximumSize = new System.Drawing.Size(200, 0);
            label.Text = text;
            label.Margin = new Padding(0, 0, 0, 20);
            return label;
        }

        private PictureBox CreatePictureBox(Bitmap bmp)
        {
            var pictureBox = new PictureBox();
            var newWidth = 200;
            var newHeight = bmp.Height / ((double)bmp.Width / 200);
            pictureBox.Image = new Bitmap(bmp, new Size(newWidth, (int)newHeight));

            bmp.Dispose();
            pictureBox.Width = newWidth;
            pictureBox.Height = (int)newHeight;
            return pictureBox;
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
            var dialog = new OpenFileDialog();
            dialog.ShowHelp = true;
            dialog.Filter = "Image Files(*.jpeg;*.bmp;*.png;*.jpg)|*.jpeg;*.bmp;*.png;*.jpg";
            dialog.InitialDirectory = @"c:\test";
            dialog.RestoreDirectory = false;
            DialogResult Action = dialog.ShowDialog();
            if (Action == DialogResult.OK)
            {
                Bitmap originalImage = new Bitmap(dialog.FileName);
                var imageBytes = GetImageBytes(originalImage);

                string channelNoiseText = ChannelNoiseInput.Text;
                if (!double.TryParse(channelNoiseText, out var noise) || noise < 0 || noise > 1)
                {
                    MessageBox.Show("Noise must be a number between 0 and 1.");
                    return;
                }

                var channel = new NoisyChannel(errorChance: noise);

                var unencodedImageBytes = await MessageController.SendBytes(imageBytes, channel);
                var unencodedImage = SetImageBytes(originalImage, unencodedImageBytes);



//                var encodedImageBytes = await MessageController.SendBytes(
//                    imageBytes,
//                    channel,
//                    new ConvolutionalEncoder(),
//                    new ConvolutionalDecoder());
//                var encodedImage = SetImageBytes(originalImage, encodedImageBytes);


                encodingResultPanel.Controls.Add(CreateLabelWithText("Orginal Image:"));
                encodingResultPanel.Controls.Add(CreatePictureBox(originalImage));
                encodingResultPanel.Controls.Add(CreateLabelWithText("Unencoded Image:"));
                encodingResultPanel.Controls.Add(CreatePictureBox(unencodedImage));
//                encodingResultPanel.Controls.Add(CreateLabelWithText("Encoded Image:"));
//                encodingResultPanel.Controls.Add(CreatePictureBox(encodedImage));
            }
        }
    }
}
