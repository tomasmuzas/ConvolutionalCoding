﻿using ConvolutionalCodes.Entities;

namespace ConvolutionalCodes.Encoders
{
    public interface IEncoder
    {
        IBitStream Encode(IBitStream stream);
    }
}
