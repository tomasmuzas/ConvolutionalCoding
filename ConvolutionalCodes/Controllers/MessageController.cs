using ConvolutionalCodes.Encoders;
using ConvolutionalCodes.Entities;
using ConvolutionalCodes.Utilities;
using System.Text;

namespace ConvolutionalCodes.Controllers
{
    public class MessageController
    {
        private static IConverter<string> converter = new TextConverter(Encoding.UTF8);

        public static string SendText(string text, ICommunicationChannel channel)
        {
            var bits = converter.ToBitStream(text);
            var bitsAfterTransmission = channel.Transmit(bits);
            return converter.FromBitStream(bitsAfterTransmission);
        }

        public static string SendText(string text, ICommunicationChannel channel, IEncoder encoder, IDecoder decoder)
        {
            var bits = converter.ToBitStream(text);
            var encodedBits = encoder.Encode(bits);
            var bitsAfterTransmission = channel.Transmit(encodedBits);
            var decodedBits = decoder.Decode(bitsAfterTransmission);
            return converter.FromBitStream(decodedBits);
        }


    }
}
