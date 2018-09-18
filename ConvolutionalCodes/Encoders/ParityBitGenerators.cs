using ConvolutionalCodes.Entities;
using System.Collections.Generic;
using System.Linq;

namespace ConvolutionalCodes.Encoders
{
    public delegate Bit ParityBitGenerator(Bit inputBit, IEnumerable<Bit> registerBits);

    public class ParityBitGenerators
    {
        /// <summary>
        /// Returns input bit without performing any operation on it.
        /// </summary>
        public static ParityBitGenerator ReturnFirstBit =
            (inputBit, registerBits) => inputBit;

        /// <summary>
        ///     <para>Performs XOR operation on bits based on coeficients</para> 
        ///     <para>Bit is used in XOR operation only if it's coeficient is 1</para>
        /// </summary>
        /// <param name="coeficients">
        ///     <para><see cref="IEnumerable{int}"/> of integers representing coeficients.</para>
        ///     <para>Allowed values are only 1 and 0.</para>
        ///     <para>1 - bit is used in XOR operation</para>
        ///     <para>0 - bit is ignored</para>
        /// </param>
        /// <param name="useInputBit">Indicates whehter the Input Bit be used in XOR operation</param>
        /// <returns></returns>
        public static ParityBitGenerator PolynomialParityBitGenerator(IEnumerable<int> coeficients, bool useInputBit)
        {
            return (inputBit, bits) =>
            {
                Bit generatedBit = useInputBit? inputBit : new Bit(0);
                int position = 0;

                foreach (var coeficient in coeficients)
                {
                    if (coeficient == 1)
                    {
                        generatedBit = generatedBit ^ bits.ElementAt(position);
                    }
                    position++;
                }

                return generatedBit;
            };
        }
    }

}
