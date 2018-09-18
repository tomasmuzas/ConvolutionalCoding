using Xunit;
using System.Collections.Generic;
using ConvolutionalCodes.Entities;
using System.Linq;
using ConvolutionalCodes.Tests.UnitTests.TestHelpers;

namespace ConvolutionalCodes.Tests.UnitTests
{
    public class BitStreamTests : CollectionTest
    { 

        [Theory]
        [InlineData(new bool[] { T, F, F, T, T, F})]
        [InlineData(new bool[] { T, F, F, T, T, F, T, T})]
        [InlineData(new bool[] { T, F, F, T, T, F, T, T, T, F, F, T, T, F, T, T})]
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
            new bool[] { F, T, T, T, F, T, F, T})]
        [InlineData(
            new byte[] { 0b11110101, 0b00101011 },
            new bool[] { T, T, T, T, F, T, F, T, F, F, T, F, T, F, T, T })]
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
        
        [Theory]
        // Amount of bits is divisible by 8
        [InlineData(
    new byte[] { 0b11110101, 0b00101011 },
    new bool[] { T, T, T, T, F, T, F, T, F, F, T, F, T, F, T, T })]
        // Amount of bits is not divisible by 8
        [InlineData(
    new byte[] { 0b11110101, 0b00101010 },
    new bool[] { T, T, T, T, F, T, F, T, F, F, T, F, T, F, T })]
        // No bits
        [InlineData(
    new byte[] { },
    new bool[] { })]
        public void BitStream_ToByteArray_Success(
    IEnumerable<byte> bytes,
    IEnumerable<bool> bools)
        {
            var testData = bools.Select(b => new Bit(b));

            IBitStream bitStream = new BitStream(bytes);

            var result = bitStream.ToByteArray();

            Assert.True(CollectionsAreEqual(bytes, result));
        }
    }
}
