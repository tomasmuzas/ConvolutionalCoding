using System.Collections.Generic;
using ConvolutionalCodes.Entities;
using System.Linq;

namespace ConvolutionalCodes.Encoders
{
    public class ShiftingRegister : IRegister
    {
        private Queue<Bit> _bits { get; set; }

        private IGeneratingPolynomial _polynomial{ get; set; }

        public ShiftingRegister(IGeneratingPolynomial polynomial)
        {
            int slotCount = polynomial.Coeficients.Count();
            _bits = new Queue<Bit>(slotCount);
            for (int i = 0; i < slotCount; i++)
            {
                _bits.Enqueue(new Bit(0));
            }
            _polynomial = polynomial;
        }

        public IEnumerable<Bit> Shift(Bit nextBit)
        {
            _bits.Dequeue();
            _bits.Enqueue(nextBit);
            int position = 0;

            var resultBits = new List<Bit>();
            resultBits.Add(nextBit);

            Bit secondBit = new Bit(0);

            foreach (var coeficient in _polynomial.Coeficients)
            {
                if (coeficient == 1)
                {
                    secondBit = secondBit ^ _bits.ElementAt(position); 
                }
                position++;
            }

            resultBits.Add(secondBit);

            return resultBits;
        }
    }
}
