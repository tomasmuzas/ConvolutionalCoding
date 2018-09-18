using ConvolutionalCodes.Entities;
using ConvolutionalCodes.Encoders;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using ConvolutionalCodes.Tests.UnitTests.TestHelpers;

namespace ConvolutionalCodes.Tests.UnitTests
{
    public class ParityBitGeneratorsTests : CollectionTest
    {
        [Theory]
        [InlineData(T)]
        [InlineData(F)]
        public void ReturnFirstBit_Success(bool inputBitValue)
        {
            var expectedBit = new Bit(inputBitValue);
            
            var resultBit = ParityBitGenerators.ReturnFirstBit(new Bit(inputBitValue), new Bit[] { });

            Assert.True(expectedBit == resultBit);
        }
        
        [Theory]
        // 1 ^ 1 = 0, input bit used
        [InlineData(
            true, // inputBitValue
            true, // shouldUseInputBit
            new bool[] { T }, // registerBitValues
            new int[] { 1 }, // coeficients
            F // expectedValue
            )]
        // 1 ^ 0 = 1, input bit used
        [InlineData(
            true, // inputBitValue
            true, // shouldUseInputBit
            new bool[] { F }, // registerBitValues
            new int[] { 1 }, // coeficients
            T // expectedValue
            )]
        // 0 ^ 0 = 0, input bit used
        [InlineData(
            false, // inputBitValue
            true, // shouldUseInputBit
            new bool[] { F }, // registerBitValues
            new int[] { 1 }, // coeficients
            F // expectedValue
            )]
        // 0 ^ 1 = 1, input bit used
        [InlineData(
            false, // inputBitValue
            true, // shouldUseInputBit
            new bool[] { T }, // registerBitValues
            new int[] { 1 }, // coeficients
            T // expectedValue
            )]
        // 1 ^ 1 ^ 1 = 1, input bit used
        [InlineData(
            true, // inputBitValue
            true, // shouldUseInputBit
            new bool[] { T, T }, // registerBitValues
            new int[] { 1, 1 }, // coeficients
            T // expectedValue
            )]
        // 1 ^ 1 = 0, input bit NOT used
        [InlineData(
            true, // inputBitValue
            false, // shouldUseInputBit
            new bool[] { T, T }, // registerBitValues
            new int[] { 1, 1 }, // coeficients
            F // expectedValue
            )]
        // 1 ^ 1 ^ 0 ^ 1 ^ 1 = 0, input bit used
        [InlineData(
            true, // inputBitValue
            true, // shouldUseInputBit
            new bool[] { T, F, T, T }, // registerBitValues
            new int[] { 1, 1, 1, 1 }, // coeficients
            F // expectedValue
            )]
        // 1 ^ 0 ^ 1 ^ 1 = 1, input bit used
        [InlineData(
            true, // inputBitValue
            true, // shouldUseInputBit
            new bool[] { T, F, T, T }, // registerBitValues
            new int[] { 0, 1, 1, 1 }, // coeficients
            T // expectedValue
            )]
        // 0 ^ 1 ^ 1 = 0, input bit used
        [InlineData(
            false, // inputBitValue
            true, // shouldUseInputBit
            new bool[] { T, F, T, T }, // registerBitValues
            new int[] { 0, 1, 1, 1 }, // coeficients
            F // expectedValue
            )]
        // 1 ^ 0 ^ 1 ^ 0 = 0, input bit used
        [InlineData(
            true, // inputBitValue
            true, // shouldUseInputBit
            new bool[] { T, F, T, F, T, F }, // registerBitValues
            new int[] { 0, 1, 0, 0, 1, 1 }, // coeficients
            F // expectedValue
            )]
        public void PolynomialParityBitGenerator_Success(
            bool inputBitValue, 
            bool shouldUseInputBit,
            IEnumerable<bool> registerBitValues,
            IEnumerable<int> coeficients,
            bool expectedValue)
        {
            var inputBit = new Bit(inputBitValue);

            var registerBits = registerBitValues.Select(v => new Bit(v));

            var generator = ParityBitGenerators
                .PolynomialParityBitGenerator(coeficients, shouldUseInputBit);

            var resultBit = generator(inputBit, registerBits);

            Assert.True(resultBit == new Bit(expectedValue));
        }
    }
}
