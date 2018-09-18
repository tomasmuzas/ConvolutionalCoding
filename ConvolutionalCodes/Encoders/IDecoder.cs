using ConvolutionalCodes.Entities;

namespace ConvolutionalCodes.Encoders
{
    public interface IDecoder
    {
        IBitStream Decode(IBitStream stream);

        void AddParityBitResolver(ParityBitGenerator generator);
    }
}
