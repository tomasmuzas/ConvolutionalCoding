using ConvolutionalCodes.Entities;

namespace ConvolutionalCodes.Utilities
{
    public interface IConverter<T>
    {
        IBitStream ToBitStream(T value);

        T FromBitStream(IBitStream stream);
    }
}
