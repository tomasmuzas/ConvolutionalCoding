using System.Collections.Generic;
using ConvolutionalCodes.Entities;

namespace ConvolutionalCodes.Encoders
{
    public class ConvolutionalEncoder : IEncoder
    {
        private IRegister _register { get; set; }

        private List<ParityBitGenerator> parityBitGenerators { get; set; }

        public ConvolutionalEncoder(IRegister register)
        {
            _register = register;
            parityBitGenerators = new List<ParityBitGenerator>();
        }

        public void AddParityBitGenerator(ParityBitGenerator generator)
        {
            parityBitGenerators.Add(generator);
        }

        public IBitStream Encode(IBitStream stream)
        {
            var encodedStream = new List<Bit>();
            foreach (var bit in stream.ReadAllBits())
            {
                IEnumerable<Bit> newBits = _register.Shift(bit);
                var parityBits = GenerateParityBits(newBits);
                encodedStream.AddRange(parityBits);
            }

            // Encode additional 6 bits to reset register state
            for (int i = 0; i < 6; i++)
            {
                IEnumerable<Bit> newBits = _register.Shift(new Bit(0));
                var parityBits = GenerateParityBits(newBits);
                encodedStream.AddRange(parityBits);
            }

            return new BitStream(encodedStream);
        }

        private IEnumerable<Bit> GenerateParityBits(IEnumerable<Bit> bits)
        {
            var parityBits = new List<Bit>();
            foreach (var generator in parityBitGenerators)
            {
                var nextParityBit = generator.GeneratorFunction.Invoke(bits);
                parityBits.Add(nextParityBit);
            }

            return parityBits;
        }
    }
}
