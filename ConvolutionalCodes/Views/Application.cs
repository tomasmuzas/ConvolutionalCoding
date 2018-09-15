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
            
            IConverter<string> converter = new TextConverter(Encoding.UTF8);

            IBitStream initialBits = converter.ToBitStream("abcXYZ");
            var initialText = converter.FromBitStream(initialBits);

            ICommunicationChannel channel = new NoisyChannel(errorChance: 0.1);

            IBitStream bitsAfterTransmission = channel.Transmit(initialBits);
            var textAfterTransmission = converter.FromBitStream(bitsAfterTransmission);

            Console.WriteLine(initialText);
            Console.WriteLine(textAfterTransmission);
        }
    }
}
