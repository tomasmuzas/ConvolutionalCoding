using System;
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
                foreach (var generator in parityBitGenerators)
                {
                    var parityBit = generator.GeneratorFunction.Invoke(newBits);
                    encodedStream.Add(parityBit);
                }
            }

            return new BitStream(encodedStream);
        }
    }
}
