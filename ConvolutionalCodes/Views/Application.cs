using ConvolutionalCodes.Controllers;
using ConvolutionalCodes.Encoders;
using ConvolutionalCodes.Entities;
using ConvolutionalCodes.Utilities;
using System;
using System.Text;
using System.Windows.Forms;

namespace ConvolutionalCodes
{
    public partial class Application : Form
    {
        public Application()
        {
            InitializeComponent();

            var initialText = "Labas, mano vardas Tomas";

            var channel = new NoisyChannel(errorChance: 0.2);

            var unencodedText = MessageController.SendText(initialText, channel);

            var encodedText = MessageController.SendText(
                initialText,
                channel,
                new ConvolutionalEncoder(),
                new ConvolutionalDecoder());

            Console.WriteLine("Initial text:\t\t\t\t\t\t" + initialText);
            Console.WriteLine("Unencoded text after transmission:\t\t\t" + unencodedText);
            Console.WriteLine("Encoded text after transmission:\t\t\t" + encodedText);

        }
    }
}
