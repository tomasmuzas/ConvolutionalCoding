using ConvolutionalCodes.Encoders;
using ConvolutionalCodes.Entities;
using ConvolutionalCodes.Utilities;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ConvolutionalCodes.Controllers
{
    public class MessageController
    {
        private static IConverter<string> converter = new TextConverter(Encoding.UTF8);

        public static async Task<string> SendText(string text, ICommunicationChannel channel)
        {
            var bits = await Task.FromResult(converter.ToBitStream(text));
            var bitsAfterTransmission = await Task.FromResult(channel.Transmit(bits));
            return await Task.FromResult(converter.FromBitStream(bitsAfterTransmission));
        }

        public static async Task<string> SendText(string text, ICommunicationChannel channel, IEncoder encoder, IDecoder decoder)
        {
            var bits = converter.ToBitStream(text);
            var encodedBits = await Task.FromResult(encoder.Encode(bits));
            var bitsAfterTransmission = await Task.FromResult(channel.Transmit(encodedBits));
            var decodedBits = await Task.FromResult(decoder.Decode(bitsAfterTransmission));
            return await Task.FromResult(converter.FromBitStream(decodedBits));
        }

        public static async Task<byte[]> SendBytes(byte[] bytes, ICommunicationChannel channel)
        {
            var bits = new BitStream(bytes);
            var bitsAfterTransmission = await Task.FromResult(channel.Transmit(bits));
            return await Task.FromResult(bitsAfterTransmission.ToByteArray());
        }

        public static async Task<byte[]> SendBytes(byte[] bytes, ICommunicationChannel channel, IEncoder encoder, IDecoder decoder)
        {
            var bits = new BitStream(bytes);
            var encodedBits = await Task.FromResult(encoder.Encode(bits));
            var bitsAfterTransmission = await Task.FromResult(channel.Transmit(encodedBits));
            var decodedBits = await Task.FromResult(decoder.Decode(bitsAfterTransmission));
            return await Task.FromResult(decodedBits.ToByteArray());
        }


    }
}
