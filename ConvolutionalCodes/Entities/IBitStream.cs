using System.Collections.Generic;

namespace ConvolutionalCodes.Entities
{
    public interface IBitStream
    {
        int Length { get; }

        Bit[] ReadAllBits();

        byte[] ToByteArray();

        int Difference(IBitStream bitStream);
    }
}