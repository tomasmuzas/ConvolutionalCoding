using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ConvolutionalCodes.Entities
{
    public class BitStream : IBitStream
    {
        private Bit[] _data { get; set; }

        private int _position { get; set; }

        public int Length => _data.Length;

        public BitStream(Bit[] bits)
        {
            _data = bits;
            _position = 0;
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
            _position = 0;
        }

        public BitStream(IBitStream stream)
        {
            _data = stream.ReadAllBits();
            _position = 0;
        }

        public static bool operator ==(BitStream stream1, BitStream stream2)
        {
            return stream1._data.SequenceEqual(stream2._data);
        }

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

        /// <summary>
        /// Read specified number of bits
        /// </summary>
        /// <param name="length">Number of bits to read</param>
        /// <returns><see cref="IEnumerable{Bit}"/> of specified length</returns>
        public IEnumerable<Bit> ReadBits(int length)
        {
            var returnData = _data.Skip(_position).Take(length);
            _position += length;
            return returnData;
        }

        public Bit[] ReadAllBits()
        {
            return _data;
        }

        public byte[] ToByteArray()
        {
            var bytes = new byte[_data.Length / 8];

            int byteCount = 0;

            for (int i = 0; i < _data.Length; i += 8)
            {
                byte currentByte = 0;
                for (int j = 0; j <= 7; j++)
                {
                    var bit = _data[i + j];

                    var positionalBit = (int)bit << (7 - j);
                    currentByte = (byte)(currentByte | positionalBit);
                }

                bytes[byteCount++] = currentByte;
            }

            return bytes;
        }
    }
}
