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

        public Bit Shift(Bit nextBit)
        {
            var lastBit = _bits.Dequeue();
            _bits.Enqueue(nextBit);
            return lastBit;
        }

        public IEnumerable<Bit> GetBits()
        {
            return _bits.AsEnumerable().Reverse();
        }
    }
}
