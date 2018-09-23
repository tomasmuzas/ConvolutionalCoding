using System.Collections.Generic;
using System.Linq;
using ConvolutionalCodes.Entities;
using ConvolutionalCodes.Extensions;

namespace ConvolutionalCodes.Encoders
{
    public class ConvolutionalEncoder : IEncoder
    {
        private IRegister _register { get; set; }
       

        public ConvolutionalEncoder()
        {
            _register = new ShiftingRegister(6);
        }

        public IBitStream Encode(IBitStream stream)
        {
            var encodedStream = new List<Bit>();
            var bitsToEncode = stream
                .ReadAllBits()
                .Prepend(new [] {
                    new Bit(0),
                    new Bit(0),
                    new Bit(0),
                    new Bit(0),
                    new Bit(0),
                    new Bit(0)
                })
                .ToArray();
            int position = 0;
            var bitsCount = bitsToEncode.Length;

            while (position < bitsCount)
            {
                var inputBit = GetBitAt(bitsToEncode, position + 6);
                var secondBit = inputBit
                    ^ GetBitAt(bitsToEncode, position + 4) // reversed 2nd bit (6 - 2)
                    ^ GetBitAt(bitsToEncode, position + 1) // reversed 5th bit (6 - 5)
                    ^ GetBitAt(bitsToEncode, position); // reversed 6th bit (6 - 6)
                encodedStream.Add(inputBit);
                encodedStream.Add(secondBit);
                position++;
            }
            return new BitStream(encodedStream);
        }

        private Bit GetBitAt(Bit[] bits, int position)
        {
            if (position < bits.Length)
            {
                return bits[position];
            }
            return new Bit(0);
        }
    }
}
