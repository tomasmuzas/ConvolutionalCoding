using System.Collections.Generic;
using ConvolutionalCodes.Entities;

namespace ConvolutionalCodes.Encoders
{
    public class ConvolutionalEncoder : IEncoder
    {
        private IRegister _register { get; set; }

        public ConvolutionalEncoder(IRegister register)
        {
            _register = register;
        }

        public IBitStream Encode(IBitStream stream)
        {
            var encodedStream = new List<Bit>();
            foreach (var bit in stream.ReadAllBits())
            {
                IEnumerable<Bit> result = _register.Shift(bit);
                encodedStream.AddRange(result);
            }

            return new BitStream(encodedStream);
        }
    }
}
