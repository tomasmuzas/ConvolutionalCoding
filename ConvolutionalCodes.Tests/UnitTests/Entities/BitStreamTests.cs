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

        [Fact]
        public void BitStream_Copies_IEnumerable_Of_Bits_Correctly()
        {
            var testData = new List<Bit>
            {
                new Bit(1),
                new Bit(0),
                new Bit(0),
                new Bit(1),
                new Bit(1),
                new Bit(0)
            };

            IBitStream bitStream = new BitStream(testData);

            Assert.True(CollectionsAreEqual(testData, bitStream.ReadAllBits()));
        }
    }
}
