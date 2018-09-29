using System.Drawing;
using System.Windows.Forms;

namespace ConvolutionalCodes.Utilities
{
    public static class UIHelper
    {
        public static Label CreateLabelWithText(string text)
        {
            var label = new Label
            {
                AutoSize = true,
                Text = text,
                MaximumSize = new Size(200, 0)
            };
            return label;
        }

        public static FlowLayoutPanel CreateImagePanel(Bitmap bmp)
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
    }
}
