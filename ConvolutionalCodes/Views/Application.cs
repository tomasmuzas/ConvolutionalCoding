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

            var initialTextLabel = new Label();
            initialTextLabel.AutoSize = true;
            initialTextLabel.Text = "Initial Text: " + initialText;
            flowLayoutPanel1.Controls.Add(initialTextLabel);

            var unencodedTextLabel = new Label();
            unencodedTextLabel.AutoSize = true;
            unencodedTextLabel.Text = "Unencoded Text: " + unencodedText;
            flowLayoutPanel1.Controls.Add(unencodedTextLabel);

            var encodedTextLabel = new Label();
            encodedTextLabel.AutoSize = true;
            encodedTextLabel.Text = "Encoded Text: " + encodedText;
            flowLayoutPanel1.Controls.Add(encodedTextLabel);
        }
    }
}
