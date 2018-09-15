using System;
using ConvolutionalCodes.Entities;

namespace ConvolutionalCodes.Encoders
{
    public class ConvolutionalEncoder : IEncoder
    {
        private IRegister _register { get; set; }

        public ConvolutionalEncoder(IGeneratingPolynomial generatingPolynomial)
        {
            _register = parityBitCalculator;
        }

        public IBitStream Encode(IBitStream stream)
        {
            throw new NotImplementedException();
        }
    }
}
