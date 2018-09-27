using ConvolutionalCodes.Encoders;
using ConvolutionalCodes.Entities;
using ConvolutionalCodes.Tests.UnitTests.TestHelpers;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace ConvolutionalCodes.Tests.IntegrationTests
{
    public class ConvolutionalDecoderTests : CollectionTest
    {
        [Theory]
        [InlineData(new bool[] { T, T, F, F, F, T, F, F, F, F, F, T, F, T }, new bool[] { T })]
        [InlineData(new bool[] { F, F, F, F, F, F, F, F, F, F, F, F, F, F }, new bool[] { F })]
        public void ConvolutionalDecoder_OneBit_Success(IEnumerable<bool> bitValues, IEnumerable<bool> expectedBitValues)
        {
            var bitsToDecode = bitValues.Select(v => new Bit(v));
            var expectedBits = expectedBitValues.Select(v => new Bit(v));

            AssertBits(bitsToDecode.ToArray(), expectedBits.ToArray());
        }

        [Theory]
        [InlineData(new bool[] { T, T, T, T, F, T, F, T, F, F, F, T, F, F, F, T }, new bool[] { T, T })]
        [InlineData(new bool[] { T, T, F, F, F, T, F, F, F, F, F, T, F, T, F, F }, new bool[] { T, F })]
        [InlineData(new bool[] { F, F, T, T, F, F, F, T, F, F, F, F, F, T, F, T }, new bool[] { F, T })]
        [InlineData(new bool[] { F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F }, new bool[] { F, F })]
        public void ConvolutionalEncoder_MoreBits_Success(IEnumerable<bool> bitValues, IEnumerable<bool> expectedValues)
        {
            var bitsToEncode = new List<Bit>();

            bitsToEncode.AddRange(bitValues.Select(v => new Bit(v)));

            AssertBits(bitsToEncode.ToArray(), expectedValues.Select(v => new Bit(v)).ToArray());
        }

        private void AssertBits(Bit[] bitsToDecode, Bit[] expectedValues)
        {
            var bitStream = new BitStream(bitsToDecode);

            var decoder = new ConvolutionalDecoder();

            var result = decoder.Decode(bitStream);

            var expectedStream = new BitStream(expectedValues);

            Assert.True((BitStream)result == expectedStream);
        }
    }
}
