using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ConvolutionalCodes.Entities
{
    public class BitStream : IBitStream
    {
        private IEnumerable<Bit> _data { get; set; }

        private int _position { get; set; }

        public int Length => _data.Count();

        public BitStream()
        {
            _data = new List<Bit>();
            _position = 0;
        }

        public BitStream(IEnumerable<Bit> bits)
        {
            _data = bits;
            _position = 0;
        }

        public BitStream(IEnumerable<byte> bytes)
        {
            var list = new List<Bit>();
            foreach(var b in bytes)
            {
                for (int i = 0; i <= 7; i++)
                {
                    var bit = new Bit(b >> (7 - i));
                    list.Add(bit); 
                }
            }

            _data = list;
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

        public IEnumerable<Bit> ReadAllBits()
        {
            return _data;
        }

        public byte[] ToByteArray()
        {
//            var bytes = new List<byte>();
//            int bitCount = 0;
//            byte currentByte = 0;
//
//            foreach (var bit in _data)
//            {
//                // Shift 1 or 0 representation of a Bit to the right position
//                var positionalBit = (int)bit << (7 - bitCount);
//                // Set the right position of a byte
//                currentByte = (byte)(currentByte  | positionalBit);
//
//                if (++bitCount == 8)
//                {
//                    bytes.Add(currentByte);
//                    currentByte = 0;
//                    bitCount = 0;
//                }
//            }
//
//            return bytes.ToArray();
            var bitArray = new BitArray(_data.Select(b => (bool)b).ToArray());
            byte[] bytes = new byte[bitArray.Length / 8];
            bitArray.CopyTo(bytes, 0);
            return bytes;
        }
    }
}
