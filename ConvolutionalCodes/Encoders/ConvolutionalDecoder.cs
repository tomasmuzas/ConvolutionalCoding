using System;
using System.Collections.Generic;
using System.Linq;
using ConvolutionalCodes.Entities;

namespace ConvolutionalCodes.Encoders
{
    public class ConvolutionalDecoder : IDecoder
    {
        private Bit MajorityDecisionElement(IEnumerable<Bit> bits)
        {
            return bits.Count(b => b == new Bit(1)) > (bits.Count() / 2) ? new Bit(1) : new Bit(0);
        }

        private IEnumerable<IRegister> _registers { get; set; }

        public ConvolutionalDecoder()
        {
            _registers = new List<IRegister> { new ShiftingRegister(6), new ShiftingRegister(6) };
        }

        public IBitStream Decode(IBitStream encodedStream)
        {
            var decodedBits = new List<Bit>();

            var upperRegister = _registers.ElementAt(0);
            var lowerRegister = _registers.ElementAt(1);

            var position = 0;

            while (position < encodedStream.Length)
            {
                var encodedBits = encodedStream.ReadBits(2);

                var firstBit = encodedBits.ElementAt(0);
                var secondBit = encodedBits.ElementAt(1);

                var firstRegisterBits = upperRegister.GetBits();
                var xoredBit = XORBitsWithIndices(firstRegisterBits, new int[] { 2, 5, 6 });

                var fistPartOfDecodedBit = upperRegister.Shift(firstBit);

                var inputForSecondRegister = firstBit ^ secondBit ^ xoredBit;

                var secondRegisterBits = lowerRegister.GetBits();

                var secondPartOfDecodedBit = MajorityDecisionElement(new Bit[] {
                    inputForSecondRegister,
                    secondRegisterBits.ElementAt(0),
                    secondRegisterBits.ElementAt(3),
                    lowerRegister.Shift(inputForSecondRegister)
                });

                var decodedBit = fistPartOfDecodedBit ^ secondPartOfDecodedBit;
                decodedBits.Add(decodedBit);

                position+=2;
            }

            return new BitStream(decodedBits.Skip(6));
        }

        public Bit XORBitsWithIndices(IEnumerable<Bit> bits, IEnumerable<int> coeficients)
        {
            Bit generatedBit = new Bit(0);

            foreach (var coeficient in coeficients)
            {
                generatedBit = generatedBit ^ bits.ElementAt(coeficient - 1);
            }

            return generatedBit;
        }
    }
}
