using System.Collections.Generic;

namespace ConvolutionalCodes.Entities
{
    public interface IBitStream
    {
        IEnumerable<Bit> ReadBits(int length);

        IEnumerable<Bit> ReadAllBits();
    }
}