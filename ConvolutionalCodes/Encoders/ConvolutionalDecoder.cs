using System.Collections.Generic;
using System.Linq;
using ConvolutionalCodes.Entities;

namespace ConvolutionalCodes.Encoders
{
    public class ConvolutionalDecoder : IDecoder
    {
        private Bit MajorityDecisionElement(IEnumerable<Bit> bits)
        {
            return bits.Count(b => b == new Bit(1)) > (bits.Count() / 2) ? new Bit(1) : new Bit(0);
        }

        private IEnumerable<IRegister> _registers { get; set; }

        private List<ParityBitGenerator> parityBitResolvers { get; set; }

        public ConvolutionalDecoder(IEnumerable<IRegister> registers)
        {
            _registers = registers;

            parityBitResolvers = new List<ParityBitGenerator>();
        }

        public void AddParityBitResolver(ParityBitGenerator resolver)
        {
            parityBitResolvers.Add(resolver);
        }

        public IBitStream Decode(IBitStream encodedStream)
        {
            var decodedStream = new List<Bit>();

            var bitRatio = 2;

            // Go through all of the stream bits except last 12, which are just unnecessary info
            for (int i = 0; i < encodedStream.Length - 12; i+= bitRatio)
            {
                var encodedBits = encodedStream.ReadBits(bitRatio);

                int bitIndex = 0;

                var decodedBit = new Bit(0);

                foreach (var bit in encodedBits)
                {
                    var currentRegister = _registers.ElementAt(bitIndex);
                    var registerBits = currentRegister.GetBits();
                    decodedBit = decodedBit ^ RecoverPartOfBit();

                    currentRegister.Shift(bit);

                    bitIndex++;
                }

                decodedStream.Add(decodedBit);
            }

            return new BitStream(decodedStream);
        }

        private Bit RecoverPartOfBit()
        {
            return new Bit(0);
        }
    }
}
