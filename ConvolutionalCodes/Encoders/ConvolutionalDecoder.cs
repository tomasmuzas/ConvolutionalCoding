using System.Collections.Generic;
using System.Linq;
using ConvolutionalCodes.Entities;

namespace ConvolutionalCodes.Encoders
{
    public class ConvolutionalDecoder : IDecoder
    {
        private Bit MajorityDecisionElement(Bit[] bits)
        {
            var trueCount = 0;
            for (var i = 0; i < bits.Length; i++)
            {
                if (bits[i] == true) trueCount++;
            }

            return trueCount > bits.Length / 2 ? new Bit(true) : new Bit(false);
        }

        public IBitStream Decode(IBitStream encodedStream)
        {
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

                var firstRegisterBits = upperRegister.GetBits();
                var xoredBit = 
                    firstRegisterBits[1] 
                    ^ firstRegisterBits[4] 
                    ^ firstRegisterBits[5];

                var fistPartOfDecodedBit = upperRegister.Shift(firstBit);

                var inputForSecondRegister = firstBit ^ secondBit ^ xoredBit;

                var secondRegisterBits = lowerRegister.GetBits();

                var secondPartOfDecodedBit = MajorityDecisionElement(new [] {
                    inputForSecondRegister,
                    secondRegisterBits[0],
                    secondRegisterBits[3],
                    lowerRegister.Shift(inputForSecondRegister)
                });

                var decodedBit = fistPartOfDecodedBit ^ secondPartOfDecodedBit;
                decodedBits[decodedStreamPosition++] = decodedBit;
            }

            return new BitStream(decodedBits.Skip(6).ToArray());
        }
    }
}
