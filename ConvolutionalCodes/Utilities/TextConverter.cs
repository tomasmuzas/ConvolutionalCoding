using System.Text;
using System.Text.RegularExpressions;
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
            // Replace non printable characters
            return Regex.Replace(_encoding.GetString(bytes), @"\p{C}+", "?"); 
        }

        public IBitStream ToBitStream(string value)
        {
            var bytes = _encoding.GetBytes(value);
            return new BitStream(bytes);
        }
    }
}
