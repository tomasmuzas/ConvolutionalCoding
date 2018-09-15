using System.Collections.Generic;
using ConvolutionalCodes.Entities;
using System.Linq;

namespace ConvolutionalCodes.Encoders
{
    public class ShiftingRegister : IRegister
    {
        private Queue<Bit> _bits { get; set; }

        public ShiftingRegister(int slotCount)
        {
            _bits = new Queue<Bit>(slotCount);
            for (int i = 0; i < slotCount; i++)
            {
                _bits.Enqueue(new Bit(0));
            }
        }

        public IEnumerable<Bit> Shift(Bit nextBit)
        {
            _bits.Dequeue();
            _bits.Enqueue(nextBit);
            return _bits.AsEnumerable();
        }
    }
}
