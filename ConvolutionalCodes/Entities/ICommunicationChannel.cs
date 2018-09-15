namespace ConvolutionalCodes.Entities
{
    public interface ICommunicationChannel
    {
        IBitStream Transmit(IBitStream bits);
    }
}
