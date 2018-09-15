using System.Collections;
using System.Collections.Generic;

namespace ConvolutionalCodes.Entities
{
    public class BitStream : IBitStream
    {
        private BitArray _data { get; set; }

        private int _position { get; set; }

        public BitStream(byte[] bytes)
        {
            _data = new BitArray(bytes);
            _position = _data.Count - 1;
        }

        public IEnumerable<Bit> ReadBits(int length)
        {
            List<Bit> bits = new List<Bit>(length);
            for (int i = 0; i < length ; i++)
            {
                bits.Add(_data.Get(_position--));
            }
            return bits;
        }

        public IEnumerable<Bit> ReadAllBits()
        {
            // Step through each byte
            for (int i = 0; i < _data.Count; i += 8)
            {
                // Reverse bits
                for (short j = 7; j >= 0; j--)
                {
                    yield return _data.Get(i + j);
                }
            }
        }
    }
}
