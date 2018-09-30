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

        /// <summary>
        /// Shift register by inserting a new value
        /// </summary>
        /// <param name="nextBit">A <see cref="Bit"/> to insert into the register</param>
        /// <returns><see cref="Bit"/> that inserted in the register first</returns>
        public Bit Shift(Bit nextBit)
        {
            // Get the last bit
            var returnBit = _bits[_bits.Length - 1];

            // Shift bits by taking previous one from left to right
            for (int i = _bits.Length - 1; i > 0; i--)
            {
                _bits[i] = _bits[i - 1];
            }

            // Insert the new value
            _bits[0] = nextBit;
            return returnBit;
        }

        public Bit[] GetBits()
        {
            return _bits;
        }
    }
}
