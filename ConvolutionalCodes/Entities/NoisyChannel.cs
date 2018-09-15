using System;
using System.Collections.Generic;

namespace ConvolutionalCodes.Entities
{
    public class NoisyChannel : ICommunicationChannel
    {
        private double _errorChance { get; set; }

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
            var newValues = new List<Bit>();

            var randomNumberGenerator = new Random();

            foreach (var bit in stream.ReadAllBits())
            {
                bool errorHappened = randomNumberGenerator.NextDouble() < _errorChance;
                newValues.Add(errorHappened ? !bit : bit);
            }

            return new BitStream(newValues);
        }
    }
}
