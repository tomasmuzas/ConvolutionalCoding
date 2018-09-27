using ConvolutionalCodes.Entities;

namespace ConvolutionalCodes.Encoders
{
    public class ShiftingRegister : IRegister
    {
        private readonly Bit[] _bits;

        public ShiftingRegister(int slotCount)
        {
            _bits = new Bit[slotCount];
            for (int i = 0; i < slotCount; i++)
            {
                _bits[i] = new Bit(0);
            }
        }

        public Bit Shift(Bit nextBit)
        {
            var returnBit = _bits[_bits.Length - 1];

            for (int i = _bits.Length - 1; i > 0; i--)
            {
                _bits[i] = _bits[i - 1];
            }

            _bits[0] = nextBit;
            return returnBit;
        }

        public Bit[] GetBits()
        {
            return _bits;
        }
    }
}
