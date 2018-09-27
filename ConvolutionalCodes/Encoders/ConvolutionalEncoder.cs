using System;
using ConvolutionalCodes.Entities;
using ConvolutionalCodes.Extensions;

namespace ConvolutionalCodes.Encoders
{
    public class ConvolutionalEncoder : IEncoder
    { 
        public IBitStream Encode(IBitStream stream)
        {
            var encodedStream = new Bit[2 * stream.Length + 12];

            var bitsToEncode = new Bit[stream.Length + 6];
            for (int i = 0; i < 6; i++)
            {
                bitsToEncode[i] = new Bit(false);
            }

            var bits = stream.ReadAllBits();
            Array.Copy(bits, 0, bitsToEncode, 6, bits.Length);

            var position = 0;
            var streamPosition = 0;
            var bitsCount = bitsToEncode.Length;

            while (position < bitsCount)
            {
                var inputBit = bitsToEncode.GetBitOrDefault(position + 6);
                var secondBit = inputBit
                    ^ bitsToEncode.GetBitOrDefault(position + 4) // reversed 2nd bit (6 - 2)
                    ^ bitsToEncode.GetBitOrDefault(position + 1) // reversed 5th bit (6 - 5)
                    ^ bitsToEncode.GetBitOrDefault(position); // reversed 6th bit (6 - 6)
                encodedStream[streamPosition++] = inputBit;
                encodedStream[streamPosition++] = secondBit;
                position++;
            }
            return new BitStream(encodedStream);
        }
    }
}
