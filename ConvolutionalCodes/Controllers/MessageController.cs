using ConvolutionalCodes.Encoders;
using ConvolutionalCodes.Entities;
using ConvolutionalCodes.Utilities;
using System.Text;
using System.Threading.Tasks;

namespace ConvolutionalCodes.Controllers
{
    public class MessageController
    {
        private static readonly IConverter<string> converter = new TextConverter(Encoding.UTF8);

        public static async Task<StringResult> SendText(string text, ICommunicationChannel channel)
        {
            var bits = await Task.FromResult(converter.ToBitStream(text));
            var bitsAfterTransmission = await Task.FromResult(channel.Transmit(bits));
            var result = await Task.FromResult(converter.FromBitStream(bitsAfterTransmission));

            
            return new StringResult
            {
                Errors = bits.Difference(bitsAfterTransmission),
                Result = result
            };
        }

        public static async Task<StringResult> SendText(string text, ICommunicationChannel channel, IEncoder encoder, IDecoder decoder)
        {
            var bits = converter.ToBitStream(text);
            var encodedBits = await Task.FromResult(encoder.Encode(bits));
            var bitsAfterTransmission = await Task.FromResult(channel.Transmit(encodedBits));
            var decodedBits = await Task.FromResult(decoder.Decode(bitsAfterTransmission));
            var result = await Task.FromResult(converter.FromBitStream(decodedBits));
            return new StringResult
            {
                Errors = bits.Difference(bitsAfterTransmission),
                Result = result
            };
        }

        public static async Task<ByteArrayResult> SendBytes(byte[] bytes, ICommunicationChannel channel)
        {
            var bits = new BitStream(bytes);
            var bitsAfterTransmission = await Task.FromResult(channel.Transmit(bits));
            var result = await Task.FromResult(bitsAfterTransmission.ToByteArray());
            return new ByteArrayResult
            {
                Errors = bits.Difference(bitsAfterTransmission),
                Result = result
            };
        }

        public static async Task<ByteArrayResult> SendBytes(byte[] bytes, ICommunicationChannel channel, IEncoder encoder, IDecoder decoder)
        {
            var bits = new BitStream(bytes);
            var encodedBits = await Task.FromResult(encoder.Encode(bits));
            var bitsAfterTransmission = await Task.FromResult(channel.Transmit(encodedBits));
            var decodedBits = await Task.FromResult(decoder.Decode(bitsAfterTransmission));
            var result = await Task.FromResult(decodedBits.ToByteArray());
            return new ByteArrayResult
            {
                Errors = bits.Difference(decodedBits),
                Result = result
            };
        }


    }
}
