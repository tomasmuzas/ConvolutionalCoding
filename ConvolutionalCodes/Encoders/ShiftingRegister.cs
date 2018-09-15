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
            throw new System.NotImplementedException();
        }
    }
}
