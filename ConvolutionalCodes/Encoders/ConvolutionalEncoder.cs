using System;
using ConvolutionalCodes.Entities;
using ConvolutionalCodes.Extensions;

namespace ConvolutionalCodes.Encoders
{
    public class ConvolutionalEncoder : IEncoder
    { 
        /// <summary>
        /// Encode bits by using predefined convolutional encoding
        /// </summary>
        /// <param name="stream"><see cref="IBitStream"/> to encode</param>
        /// <returns>Encoded <see cref="IBitStream"/></returns>
        public IBitStream Encode(IBitStream stream)
        {
            // Since bit ratio is 1:2, there will be 2 times more encoded bits
            // To save register state, we will have to encode 6 more 0-bits, resulting in 12 additional bits            
            var encodedStream = new Bit[2 * stream.Length + 12];

            // We need to encode 6 more bits, because first 6 bits in register are 0s
            var bitsToEncode = new Bit[stream.Length + 6];
            for (int i = 0; i < 6; i++)
            {
                bitsToEncode[i] = new Bit(false);
            }

            // Copy 6 0-bits to the beginning of the array of bits to encode
            var bits = stream.ReadAllBits();
            Array.Copy(bits, 0, bitsToEncode, 6, bits.Length);

            var position = 0;
            var streamPosition = 0;
            var bitsCount = bitsToEncode.Length;

            // This piece of code does not use registers, thus we assume that we are going
            // through imaginary register just by acccessing 6 bits in a row,
            // and those bits are in reverse order
            while (position < bitsCount)
            {
                // 7th bit is the 'input' bit, 6 bits before it - imaginary register 
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
