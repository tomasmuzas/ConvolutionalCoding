using System;
using System.Text;
using ConvolutionalCodes.Entities;

namespace ConvolutionalCodes.Utilities
{
    public class VectorConverter : IConverter<string>
    {
        public IBitStream ToBitStream(string value)
        {
            var bits = new Bit[value.Length];
            var position = 0;

            foreach (var c in value)
            {
                switch (c)
                {
                    case '1':
                        bits[position++] = new Bit(1);
                        break;
                    case '0':
                        bits[position++] = new Bit(0);
                        break;
                    default:
                        throw new ArgumentException($"Invalid character '{c}' ar position: " + (position + 1));
                }
            }

            return new BitStream(bits);
        }

        public string FromBitStream(IBitStream stream)
        {
            var stringBuilder = new StringBuilder();

            foreach (var bit in stream.ReadAllBits())
            {
                if (bit == true)
                {
                    stringBuilder.Append('1');
                }
                else
                {
                    stringBuilder.Append(0);
                }
            }

            return stringBuilder.ToString();
        }
    }
}
