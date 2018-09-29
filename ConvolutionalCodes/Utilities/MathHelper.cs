using System;

namespace ConvolutionalCodes.Utilities
{
    public static class MathHelper
    {
        public static double CalculatePercentageWithPrecision(int min, int max, int precision)
        {
            if (max == 0)
            {
                return 0;
            }

            var percentage = 1 - (double)min / max;
            return Math.Round(percentage * 100, precision);
        }
    }
}
