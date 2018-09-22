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
            
            IConverter<string> converter = new TextConverter(Encoding.UTF8);

            var initialText = "Labas, mano vardas Tomas";
            var initialBits = converter.ToBitStream(initialText);

            var channel = new NoisyChannel(errorChance: 0.2);

            // Text without encoding
            var bitsAfterTransmission = channel.Transmit(initialBits);
            var unencodedTextAfterTransmission = converter.FromBitStream(bitsAfterTransmission);


            // Text with encoding
            var encoder = new ConvolutionalEncoder();
            var encodedText = encoder.Encode(initialBits);

            var transmittedEncodedText = channel.Transmit(encodedText);

            var decoder = new ConvolutionalDecoder();
            var decodedBitsAfterTransmission = decoder.Decode(transmittedEncodedText);
            var encodedTextAfterTransmission = converter.FromBitStream(decodedBitsAfterTransmission);

            Console.WriteLine("Initial text:\t\t\t\t\t\t" + initialText);
            Console.WriteLine("Unencoded text after transmission:\t\t\t" + unencodedTextAfterTransmission);
            Console.WriteLine("Encoded text after transmission:\t\t\t" + encodedTextAfterTransmission);

        }
    }
}
