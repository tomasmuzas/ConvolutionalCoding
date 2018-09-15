using Xunit;
using System.Collections.Generic;
using ConvolutionalCodes.Entities;
using System.Linq;

namespace ConvolutionalCodes.UnitTests
{
    public class BitStreamTests
    {
        private bool CollectionsAreEqual<T>(IEnumerable<T> collection1, IEnumerable<T> collection2)
        {
            return collection1
                .AsQueryable()
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
                .AsQueryable()
                .Select(b => new Bit(b));

            IBitStream bitStream = new BitStream(testData);

            Assert.True(CollectionsAreEqual(testData, bitStream.ReadAllBits()));
        }
    }
}
