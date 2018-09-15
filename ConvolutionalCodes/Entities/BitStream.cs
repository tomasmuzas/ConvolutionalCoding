using System;
using System.Collections.Generic;

namespace ConvolutionalCodes.Entities
{
    public class BitStream : IBitStream
    {
        private List<Bit> _data { get; set; }

        private int _position { get; set; }

        public int Length => _data.Count;

        public BitStream()
        {
            _data = new List<Bit>();
            _position = 0;
        }

        public BitStream(IEnumerable<Bit> bits)
        {
            _data = new List<Bit>();
            foreach (var bit in bits)
            {
                _data.Add(bit);
            }
            _position = _data.Count - 1;
        }

        public BitStream(IEnumerable<byte> bytes)
        {
            _data = new List<Bit>();
            foreach(var b in bytes)
            {
                for (int i = 0; i <= 7; i++)
                {
                    var bit = new Bit(b >> (7 - i));
                    _data.Add(bit); 
                }
            }
            _position = _data.Count - 1;
        }

        public BitStream(BitStream stream)
        {
            _data = new List<Bit>();
            
            foreach (var bit in stream.ReadAllBits())
            {
                _data.Add(bit);
            }

            _position = _data.Count - 1;
        }

        /// <summary>
        /// Read specified number of bits
        /// </summary>
        /// <param name="length">Number of bits to read</param>
        /// <returns><see cref="IEnumerable{Bit}"/> of specified length</returns>
        public IEnumerable<Bit> ReadBits(int length)
        {
            List<Bit> bits = new List<Bit>(length);
            for (int i = 0; i < length ; i++)
            {
                bits.Add(_data[_position--]);
            }
            return bits;
        }

        public IEnumerable<Bit> ReadAllBits()
        {
            foreach (var bit in _data)
            {
                yield return bit;
            }
        }

        public byte[] ToByteArray()
        {
            var bytes = new List<byte>();
            int bitCount = 0;
            byte currentByte = 0;

            foreach (var bit in this.ReadAllBits())
            {
                var positionalBit = (int)bit << (7 - bitCount);
                currentByte = (byte)(currentByte  | positionalBit);

                if (++bitCount == 8)
                {
                    bytes.Add(currentByte);
                    currentByte = 0;
                    bitCount = 0;
                }
            }

            return bytes.ToArray();
        }
    }
}
