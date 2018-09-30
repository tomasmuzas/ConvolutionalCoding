using System.Linq;

namespace ConvolutionalCodes.Entities
{
    public class BitStream : IBitStream
    {
        private Bit[] _data { get; }

        public int Length => _data.Length;

        public BitStream(Bit[] bits)
        {
            _data = bits;
        }

        public BitStream(byte[] bytes)
        {
            _data = new Bit[bytes.Length * 8];
            int position = 0;

            foreach (var b in bytes)
            { 
                for (int i = 0; i <= 7; i++)
                {
                    var bit = new Bit(b >> (7 - i));
                    _data[position++] = bit; 
                }
            }
        }

        public BitStream(IBitStream stream)
        {
            _data = stream.ReadAllBits();
        }

        public static bool operator ==(BitStream stream1, BitStream stream2)
        {
            return stream1._data.SequenceEqual(stream2._data);
        }

        /// <summary>
        /// Counts different bits between two streams
        /// </summary>
        /// <param name="bitStream"><see cref="BitStream"/> to compare to</param>
        /// <returns>Number of different bits</returns>
        public int Difference(IBitStream bitStream)
        {
            var errorCount = 0;
            var bitsToCompare = bitStream.ReadAllBits();
            for (int i = 0; i < _data.Length; i++)
            {
                if (_data[i] != bitsToCompare[i])
                {
                    errorCount++;
                }
            }

            return errorCount;
        }

        public static bool operator !=(BitStream stream1, BitStream stream2)
        {
            return !(stream1 == stream2);
        }

        public Bit[] ReadAllBits()
        {
            return _data;
        }

        /// <summary>
        /// Converts <see cref="BitStream"/> to byte array
        /// </summary>
        /// <returns>Bits converted to bytes</returns>
        public byte[] ToByteArray()
        {
            var bytes = new byte[_data.Length / 8];

            int byteCount = 0;

            for (int i = 0; i < _data.Length; i += 8)
            {
                byte currentByte = 0;
                // Set every bit of a byte starting from left to right
                // j denotes a bit position in a byte where 7 - rightmost bit, 0 - leftmost bit
                for (int j = 0; j <= 7; j++)
                {
                    var bit = _data[i + j];

                    // Shift bit by j positions creating only one bit set in j-th position
                    var positionalBit = (int)bit << (7 - j);

                    // Set bit to a certain position in a byte
                    currentByte = (byte)(currentByte | positionalBit);
                }

                bytes[byteCount++] = currentByte;
            }

            return bytes;
        }
    }
}
