using ConvolutionalCodes.Encoders;
using ConvolutionalCodes.Entities;
using ConvolutionalCodes.Utilities;
using System.Text;
using System.Threading.Tasks;

namespace ConvolutionalCodes.Controllers
{
    public class MessageController
    {
        private static readonly IConverter<string> textConverter = new TextConverter(Encoding.UTF8);
        private static readonly IConverter<string> vectorConverter = new VectorConverter();

        /// <summary>
        /// Encodes a given vector
        /// </summary>
        /// <param name="vector">A vector to encode</param>
        /// <param name="encoder">Encoder to encode vector with</param>
        /// <returns>Resulting text</returns>
        public static async Task<StringResultWithErrorPositions> EncodeAndSendVector(string vector, IEncoder encoder, ICommunicationChannel channel)
        {
            var bits = vectorConverter.ToBitStream(vector);

            var encodedBits = await Task.FromResult(encoder.Encode(bits));

            var bitsAfterTransmission = await Task.FromResult(channel.Transmit(encodedBits));

            var errors = encodedBits.DifferenceWithPositions(bitsAfterTransmission);

            var encodedVector = vectorConverter.FromBitStream(encodedBits);

            return new StringResultWithErrorPositions
            {
                ErrorPositions = errors,
                Errors = errors.Count,
                Result = encodedVector
            };
        }

        /// <summary>
        /// Decodes a given encoded vector
        /// </summary>
        /// <param name="encodedVector">Encoded vector to decode</param>
        /// <param name="decoder">Decoder to decode a vector</param>
        /// <returns>A <see cref="StringResult"/> that contains a decoded vector as Result property</returns>
        public static async Task<StringResult> DecodeVector(string encodedVector, ConvolutionalDecoder decoder)
        {
            var bitStream = vectorConverter.ToBitStream(encodedVector);
            
            var result = await Task.FromResult(decoder.Decode(bitStream));

            var decodedVector = vectorConverter.FromBitStream(result);
            
            return new StringResult
            {
                Errors = 0,
                Result = decodedVector
            };
        }


        /// <summary>
        /// Sends text through noisy channel
        /// </summary>
        /// <param name="text">A text to send</param>
        /// <param name="channel">Noisy channel</param>
        /// <returns>Resulting text together with error information</returns>
        public static async Task<StringResult> SendText(string text, ICommunicationChannel channel)
        {
            var bits = await Task.FromResult(textConverter.ToBitStream(text));
            var bitsAfterTransmission = await Task.FromResult(channel.Transmit(bits));
            var result = await Task.FromResult(textConverter.FromBitStream(bitsAfterTransmission));

            
            return new StringResult
            {
                Errors = bits.Difference(bitsAfterTransmission),
                Result = result
            };
        }

        /// <summary>
        /// Send text with encoding through noisy channel
        /// </summary>
        /// <param name="text">Text to send</param>
        /// <param name="channel">Noisy channel</param>
        /// <param name="encoder">Encoder to encode bits</param>
        /// <param name="decoder">Corresponding decoder to decode bits</param>
        /// <returns>Resulting text after decoding together with error information</returns>
        public static async Task<StringResult> SendText(string text, ICommunicationChannel channel, IEncoder encoder, IDecoder decoder)
        {
            var bits = textConverter.ToBitStream(text);
            var encodedBits = await Task.FromResult(encoder.Encode(bits));
            var bitsAfterTransmission = await Task.FromResult(channel.Transmit(encodedBits));
            var decodedBits = await Task.FromResult(decoder.Decode(bitsAfterTransmission));
            var result = await Task.FromResult(textConverter.FromBitStream(decodedBits));

            return new StringResult
            {
                Errors = bits.Difference(decodedBits),
                Result = result
            };
        }


        /// <summary>
        /// Send bytes through noisy channel
        /// </summary>
        /// <param name="bytes">Text to send</param>
        /// <param name="channel">Noisy channel</param>
        /// <returns>Resulting bytes together with error information</returns>
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

        /// <summary>
        /// Send bytes with encoding through noisy channel
        /// </summary>
        /// <param name="bytes">Bytes to send</param>
        /// <param name="channel">Noisy channel</param>
        /// <param name="encoder">Encoder to encode bits</param>
        /// <param name="decoder">Corresponding decoder to decode bits</param>
        /// <returns>Resulting bytes after decoding together with error information</returns>
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
