using ConvolutionalCodes.Encoders;
using ConvolutionalCodes.Entities;
using ConvolutionalCodes.Utilities;
using System;
using System.Text;
using System.Windows.Forms;
using System.Linq;

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

            IRegister register = new ShiftingRegister(slotCount: 6);
            IEncoder encoder = new ConvolutionalEncoder(register);

            // TODO: need to figure out what to do with input bit!!!
            //encoder.AddParityBitGenerator(new CustomParityBitGenerator(bits => bits.First()));
            //encoder.AddParityBitGenerator(new PolynomialParityBitGenerator(new int[] { 0, 1, 0, 0, 1, 1}));

            Console.WriteLine(initialText);
            Console.WriteLine(textAfterTransmission);
        }
    }
}
