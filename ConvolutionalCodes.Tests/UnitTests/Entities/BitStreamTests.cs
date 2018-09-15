using Xunit;
using System.Collections.Generic;
using ConvolutionalCodes.Entities;
using System.Linq;
using System;

namespace ConvolutionalCodes.UnitTests
{
    public class BitStreamTests
    {
        private bool CollectionsAreEqual<T>(IEnumerable<T> collection1, IEnumerable<T> collection2)
        {
            return collection1
                .SequenceEqual(collection2);
        }

        [Theory]
        [InlineData(new bool[] { true, false, false, true, true, false })]
        [InlineData(new bool[] { true, false, false, true, true, false, true, true })]
        [InlineData(new bool[] { true, false, false, true, true, false, true, true, true, false, false, true, true, false, true, true })]
        [InlineData(new bool[] { })]
        public void BitStream_Copies_IEnumerable_Of_Bits_Correctly(IEnumerable<bool> bits)
        {
            var testData = bits
                .Select(b => new Bit(b));

            IBitStream bitStream = new BitStream(testData);

            var result = bitStream.ReadAllBits();

            Assert.True(CollectionsAreEqual(testData, result));
        }

        [Theory]
        [InlineData(
            new byte[] { 0b01110101 },
            new bool[] { false, true, true, true, false, true, false, true})]
        [InlineData(
            new byte[] { 0b11110101, 0b00101011 },
            new bool[] { true, true, true, true, false, true, false, true, false, false, true, false, true, false, true, true })]
        [InlineData(
            new byte[] { },
            new bool[] { })]
        public void BitStream_Copies_IEnumerable_Of_Bytes_Correctly(
            IEnumerable<byte> bytes,
            IEnumerable<bool> bools)
        {
            var testData = bools.Select(b => new Bit(b));

            IBitStream bitStream = new BitStream(bytes);

            var result = bitStream.ReadAllBits();

            Assert.True(CollectionsAreEqual(testData, result));
        }
    }
}
