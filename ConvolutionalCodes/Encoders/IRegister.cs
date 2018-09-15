using ConvolutionalCodes.Entities;
using System.Collections.Generic;

namespace ConvolutionalCodes.Encoders
{
    public interface IRegister
    {
        IEnumerable<Bit> Shift(Bit nextBit);
    }
}