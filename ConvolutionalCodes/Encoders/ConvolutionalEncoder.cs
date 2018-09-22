using System.Collections.Generic;
using System.Linq;
using ConvolutionalCodes.Entities;

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
            foreach (var bit in stream.ReadAllBits())
            {
                // First step - add input bit directly to the encoded message
                encodedStream.Add(bit);

                var bits = _register.GetBits();

                var xoredBit = XORBitsWithIndices(bits, new int[] { 2, 5, 6});
                xoredBit = xoredBit ^ bit;

                encodedStream.Add(xoredBit);

                // Add new bit to the register only after parity bits are generated
                _register.Shift(bit);
            }

            // Encode additional 6 bits to reset register state
            for (int i = 0; i < 6; i++)
            {
                var inputBit = new Bit(0);
                // First step - add input bit directly to the encoded message
                encodedStream.Add(inputBit);

                var bits = _register.GetBits();

                var xoredBit = XORBitsWithIndices(bits, new int[] { 2, 5, 6 });
                xoredBit = xoredBit ^ inputBit;

                encodedStream.Add(xoredBit);

                // Add new bit to the register only after parity bits are generated
                _register.Shift(inputBit);
            }

            return new BitStream(encodedStream);
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
