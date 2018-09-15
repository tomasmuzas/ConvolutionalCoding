using ConvolutionalCodes.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ConvolutionalCodes.Encoders
{
    public class PolynomialParityBitGenerator : ParityBitGenerator
    {
        private IEnumerable<int> _coeficients { get; set; }

        public override Func<IEnumerable<Bit>, Bit> GeneratorFunction => 
            bits => ApplyCoeficients(bits);
        

        public PolynomialParityBitGenerator(IEnumerable<int> coeficients)
        {
            _coeficients = coeficients;
        }

        private Bit ApplyCoeficients(IEnumerable<Bit> bits)
        {
            Bit generatedBit = new Bit(0);
            int position = 0;

            foreach (var coeficient in _coeficients)
            {
                if (coeficient == 1)
                {
                    generatedBit = generatedBit ^ bits.ElementAt(position);
                }
                position++;
            }

            return generatedBit;
        }
    }
}
