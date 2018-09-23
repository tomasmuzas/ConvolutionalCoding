using System;
using System.Collections.Generic;
using System.Linq;

namespace ConvolutionalCodes.Entities
{
    public class NoisyChannel : ICommunicationChannel
    {
        private readonly double _errorChance;

        /// <summary>
        /// Creates a communication channel with a given error rate
        /// </summary>
        /// <param name="errorChance">Error rate for each individual bit</param>
        public NoisyChannel(double errorChance)
        {
            _errorChance = errorChance;
        }

        public IBitStream Transmit(IBitStream stream)
        {
            var randomNumberGenerator = new Random(1892365);

            var newValues = stream
                .ReadAllBits()
                .Select(bit =>
                    (randomNumberGenerator.Next(10000) / 10000.0f)  < _errorChance ? !bit : bit);

            return new BitStream(newValues);
        }
    }
}
