using ConvolutionalCodes.Entities;

namespace ConvolutionalCodes.Extensions
{
    public static class BitArrayExtensions
    {
        public static Bit GetBitOrDefault(this Bit[] bits, int position)
        {
            if (position < bits.Length)
            {
                return bits[position];
            }
            return new Bit(false);
        }
    }
}
