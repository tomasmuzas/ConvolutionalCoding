using System.Collections.Generic;

namespace ConvolutionalCodes.Entities
{
    public interface IBitStream
    {
        int Length { get; }

        IEnumerable<Bit> ReadBits(int length);

        Bit[] ReadAllBits();

        byte[] ToByteArray();

        int Difference(IBitStream bitStream);
    }
}