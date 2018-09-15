using System.Text;
using ConvolutionalCodes.Entities;

namespace ConvolutionalCodes.Utilities
{
    public class TextConverter : IConverter<string>
    {
        private Encoding _encoding { get; set; }

        public TextConverter(Encoding encoding)
        {
            _encoding = encoding;
        }

        public string FromBitStream(IBitStream stream)
        {
            var bytes = stream.ToByteArray();
            return _encoding.GetString(bytes);
        }

        public IBitStream ToBitStream(string value)
        {
            var bytes = _encoding.GetBytes(value);
            return new BitStream(bytes);
        }
    }
}
