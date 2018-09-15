using ConvolutionalCodes.Entities;
using System;
using System.Collections.Generic;

namespace ConvolutionalCodes.Encoders
{
    public abstract class ParityBitGenerator
    {
        public abstract Func<IEnumerable<Bit>, Bit> GeneratorFunction { get; }

    }
}