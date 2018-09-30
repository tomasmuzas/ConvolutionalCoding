using System.Linq;
using ConvolutionalCodes.Entities;

namespace ConvolutionalCodes.Encoders
{
    public class ConvolutionalDecoder : IDecoder
    {
        /// <summary>
        /// Selects the bit based on majority
        /// </summary>
        /// <param name="bits">Bits to check</param>
        /// <returns>1 if there were more 1's than 0's, 0 otherwise</returns>
        private Bit MajorityDecisionElement(Bit[] bits)
        {
            var trueCount = 0;
            for (var i = 0; i < bits.Length; i++)
            {
                if (bits[i] == true) trueCount++;
            }

            return trueCount > bits.Length / 2 ? new Bit(true) : new Bit(false);
        }

        /// <summary>
        /// Decodes an encoded <see cref="IBitStream"/> by using predefined convolutional decoder.
        /// </summary>
        /// <param name="encodedStream"><see cref="IBitStream"/> with encoded bits</param>
        /// <returns><see cref="IBitStream"/> of decoded bits</returns>
        public IBitStream Decode(IBitStream encodedStream)
        {
            // Bit ratio is 1:2, thus decoding will result in 2 times less bits
            var decodedBits = new Bit[encodedStream.Length / 2 ];
            var encodedBits = encodedStream.ReadAllBits();
            
            var streamPosition = 0;
            var decodedStreamPosition = 0;

            var upperRegister = new ShiftingRegister(6);
            var lowerRegister = new ShiftingRegister(6);

            while (streamPosition < encodedStream.Length)
            {
                var firstBit = encodedBits[streamPosition++];
                var secondBit = encodedBits[streamPosition++];

                // Get all bits from upper register BEFORE shifting it
                var firstRegisterBits = upperRegister.GetBits();
                var xoredBit = 
                    firstRegisterBits[1] 
                    ^ firstRegisterBits[4] 
                    ^ firstRegisterBits[5]; 

                // Shift the register and get first bit that entered the register
                var fistPartOfDecodedBit = upperRegister.Shift(firstBit);

                var inputForSecondRegister = firstBit ^ secondBit ^ xoredBit;

                // Get all bits from lower register BEFORE shifting it
                var secondRegisterBits = lowerRegister.GetBits();

                var secondPartOfDecodedBit = MajorityDecisionElement(new [] {
                    inputForSecondRegister,
                    secondRegisterBits[0],
                    secondRegisterBits[3],
                    // Shift second register and get first bit that entered the register
                    lowerRegister.Shift(inputForSecondRegister)
                });

                var decodedBit = fistPartOfDecodedBit ^ secondPartOfDecodedBit;
                decodedBits[decodedStreamPosition++] = decodedBit;
            }

            // Skip 6 first bits since they are just initial state information
            return new BitStream(decodedBits.Skip(6).ToArray());
        }
    }
}
