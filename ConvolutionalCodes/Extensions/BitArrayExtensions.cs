using ConvolutionalCodes.Entities;

namespace ConvolutionalCodes.Extensions
{
    public static class BitArrayExtensions
    {
        /// <summary>
        /// Safely get a Bit of BitArray
        /// </summary>
        /// <param name="bits"></param>
        /// <param name="position"></param>
        /// <returns><see cref="Bit"/> at a given position or 0 <see cref="Bit"/> if the position is invalid</returns>
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
