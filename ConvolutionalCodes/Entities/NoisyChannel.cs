﻿using System;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

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

        /// <summary>
        /// Transmits <see cref="Bit"/>s through the noisy channel and distorts it with a given error chance
        /// </summary>
        /// <param name="stream"><see cref="IBitStream"/> to distort</param>
        /// <returns>Distorted <see cref="IBitStream"/></returns>
        public IBitStream Transmit(IBitStream stream)
        {
            var randomNumberGenerator = new Random();

            var newValues = new Bit[stream.Length];
            var oldValues = stream.ReadAllBits();

            for(int counter = 0; counter < stream.Length; counter ++)
            {
                var bit = oldValues[counter];
                var randomDouble = randomNumberGenerator.NextDouble();

                var newBit = randomDouble < _errorChance ? !bit : bit;

                newValues[counter] = newBit;
            }

            return new BitStream(newValues);
        }
    }
}
