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
        [InlineData(T, new bool[]{T, T, F, F, F, T, F, F, F, F, F, T, F, T})]
        [InlineData(F, new bool[] { F, F, F, F, F, F, F, F, F, F, F, F, F, F })]
        public void ConvolutionalEncoder_OneBit_Success(bool bitValue, IEnumerable<bool> expectedValues)
        {
            var bitsToEncode = new List<Bit> { new Bit(bitValue) };

            AssertBits(bitsToEncode, expectedValues.Select(v => new Bit(v)));
        }

        [Theory]
        [InlineData(new bool[] { T, T}, new bool[] { T, T, T, T, F, T, F, T, F, F, F, T, F, F, F, T })]
        [InlineData(new bool[] { T, F }, new bool[] { T, T, F, F, F, T, F, F, F, F, F, T, F, T, F, F })]
        [InlineData(new bool[] { F, T }, new bool[] { F, F, T, T, F, F, F, T, F, F, F, F, F, T, F, T })]
        [InlineData(new bool[] { F, F }, new bool[] { F, F, F, F, F, F, F, F, F, F, F, F, F, F, F, F })]
        public void ConvolutionalEncoder_MoreBits_Success(IEnumerable<bool> bitValues, IEnumerable<bool> expectedValues)
        {
            var bitsToEncode = new List<Bit>();

            bitsToEncode.AddRange(bitValues.Select(v => new Bit(v)));

            AssertBits(bitsToEncode,expectedValues.Select(v => new Bit(v)));
        }

        private void AssertBits(IEnumerable<Bit> bitsToEncode, IEnumerable<Bit> expectedValues)
        {
            var bitStream = new BitStream(bitsToEncode);

            var register = new ShiftingRegister(slotCount: 6);
            var encoder = new ConvolutionalEncoder(register);

            encoder.AddParityBitGenerator(ParityBitGenerators.ReturnFirstBit);
            encoder.AddParityBitGenerator(ParityBitGenerators.PolynomialParityBitGenerator(
                new int[] { 0, 1, 0, 0, 1, 1 },
                useInputBit: true));

            var result = encoder.Encode(bitStream);

            var expectedStream = new BitStream(expectedValues);

            // Resulting stream should always be 2 * n + 12 length
            Assert.True(((BitStream)result).Length == 2 * bitsToEncode.Count() + 12);
            Assert.True(expectedStream == (BitStream)result);
        }
    }
}
