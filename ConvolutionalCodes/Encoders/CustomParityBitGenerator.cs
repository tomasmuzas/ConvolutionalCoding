using System;
using System.Collections.Generic;
using ConvolutionalCodes.Entities;

namespace ConvolutionalCodes.Encoders
{
    public class CustomParityBitGenerator : ParityBitGenerator
    {
        public override Func<IEnumerable<Bit>, Bit> GeneratorFunction { get; }

        public CustomParityBitGenerator(Func<IEnumerable<Bit>, Bit> customFunction)
        {
            GeneratorFunction = customFunction;
        }
    }
}
