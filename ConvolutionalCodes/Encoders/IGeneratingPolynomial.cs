using System.Collections.Generic;

namespace ConvolutionalCodes.Encoders
{
    public interface IGeneratingPolynomial
    {
        IEnumerable<int> Coeficients { get; set; }
    }
}