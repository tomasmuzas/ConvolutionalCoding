using System.Collections.Generic;

namespace ConvolutionalCodes.Controllers
{
    public class TransmissionResult
    {
        public int Errors { get; set; }
    }

    public class StringResultWithErrorPositions : StringResult
    {
        public List<int> ErrorPositions;
    }

    public class StringResult : TransmissionResult
    {
        public string Result;
    }

    public class ByteArrayResult : TransmissionResult
    {
        public byte[] Result;
    }
}
