using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace ConvolutionalCodes.Utilities
{
    public static class BitmapHelper
    {
        public static byte[] GetColorBytes(Bitmap bmp)
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

        public static Bitmap SetImageBytes(Bitmap original, byte[] imageBytes)
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

        public static Bitmap ScaleBitmap(Bitmap originalImage, int desiredWith)
        {
            var scaledHeight = originalImage.Height / ((double)originalImage.Width / desiredWith);

            var scaledImage = new Bitmap(originalImage, new Size(desiredWith, (int)scaledHeight));
            originalImage.Dispose();

            return scaledImage;
        }
    }
}
