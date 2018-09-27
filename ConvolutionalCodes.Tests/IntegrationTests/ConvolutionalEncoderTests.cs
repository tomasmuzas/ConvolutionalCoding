using ConvolutionalCodes.Encoders;
using ConvolutionalCodes.Entities;
using ConvolutionalCodes.Tests.UnitTests.TestHelpers;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace ConvolutionalCodes.Tests.IntegrationTests
{
    public class ConvolutionalEncoderTests : CollectionTest
    {
        [Theory]
        [InlineData(T, new []{T, T, F, F, F, T, F, F, F, F, F, T, F, T})]
        [InlineData(F, new [] { F, F, F, F, F, F, F, F, F, F, F, F, F, F })]
        public void ConvolutionalEncoder_OneBit_Success(bool bitValue, IEnumerable<bool> expectedValues)
        {
            var bitsToEncode = new List<Bit> { new Bit(bitValue) };

            AssertBits(bitsToEncode, expectedValues.Select(v => new Bit(v)));
        }

        [Theory]
        [InlineData(new [] { T, T}, new [] { T, T, T, T, F, T, F, T, F, F, F, T, F, F, F, T })]
        [InlineData(new [] { T, F }, new [] { T, T, F, F, F, T, F, F, F, F, F, T, F, T, F, F })]
        [InlineData(new [] { F, T }, new [] { F, F, T, T, F, F, F, T, F, F, F, F, F, T, F, T })]
        [InlineData(new [] { F, F }, new [] { F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F })]
        public void ConvolutionalEncoder_MoreBits_Success(IEnumerable<bool> bitValues, IEnumerable<bool> expectedValues)
        {
            var bitsToEncode = new List<Bit>();

            bitsToEncode.AddRange(bitValues.Select(v => new Bit(v)));

            AssertBits(bitsToEncode,expectedValues.Select(v => new Bit(v)));
        }

        private void AssertBits(IEnumerable<Bit> bitsToEncode, IEnumerable<Bit> expectedValues)
        {
            var bitsArray = bitsToEncode.ToArray();
            var bitStream = new BitStream(bitsArray);
            var encoder = new ConvolutionalEncoder();

            var result = encoder.Encode(bitStream);

            var expectedStream = new BitStream(expectedValues.ToArray());

            // Resulting stream should always be 2 * n + 12 length
            Assert.True(((BitStream)result).Length == 2 * bitsArray.Length + 12);
            Assert.True(expectedStream == (BitStream)result);
        }
    }
}
