using ConvolutionalCodes.Entities;
using System.Collections.Generic;

namespace ConvolutionalCodes.Encoders
{
    public interface IRegister
    {
        Bit Shift(Bit nextBit);

        IEnumerable<Bit> GetBits();
    }
}