using ConvolutionalCodes.Controllers;
using ConvolutionalCodes.Encoders;
using ConvolutionalCodes.Entities;
using System;
using System.Windows.Forms;

namespace ConvolutionalCodes
{
    public partial class Application : Form
    {
        public Application()
        {
            InitializeComponent();
        }

        private void TextSubmit_Click(object sender, EventArgs e)
        {
            var initialText = TextInput.Text;

            string channelNoiseText = ChannelNoiseInput.Text;

            if (!double.TryParse(channelNoiseText, out var noise) || noise < 0 || noise > 1 )
            {
                MessageBox.Show("Noise must be a number between 0 and 1.");
                return;
            }

            var channel = new NoisyChannel(errorChance: noise);

            var unencodedText = MessageController.SendText(initialText, channel);

            var encodedText = MessageController.SendText(
                initialText,
                channel,
                new ConvolutionalEncoder(),
                new ConvolutionalDecoder());

            flowLayoutPanel1.Controls.Clear();
            flowLayoutPanel1.Controls.Add(CreateLabelWithText("Initial Text: " + initialText));
            flowLayoutPanel1.Controls.Add(CreateLabelWithText("Unencoded Text: " + unencodedText));
            flowLayoutPanel1.Controls.Add(CreateLabelWithText("Encoded Text: " + encodedText));
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
    }
}
