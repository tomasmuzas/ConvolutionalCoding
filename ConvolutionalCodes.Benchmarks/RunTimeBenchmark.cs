using System;
using System.Drawing;
using System.Threading.Tasks;
using ConvolutionalCodes.Controllers;
using ConvolutionalCodes.Encoders;
using ConvolutionalCodes.Entities;
using ConvolutionalCodes.Utilities;

namespace ConvolutionalCodes.Benchmarks
{
    public class RunTimeBenchmark
    {
        public static async Task RunBenchmark(int deltaN, int max)
        {
            int currentSize = deltaN;

            var encoder = new ConvolutionalEncoder();
            var decoder = new ConvolutionalDecoder();
            var noisyChannel = new NoisyChannel(0.01);

            while (currentSize <= max)
            {
                var bmp = new Bitmap(currentSize, currentSize);
                var bytes = BitmapHelper.GetColorBytes(bmp);

                var watch = System.Diagnostics.Stopwatch.StartNew();
                var resultWithEncoding = await MessageController.SendBytes(
                    bytes,
                    noisyChannel,
                    encoder,
                    decoder);

                watch.Stop();

                Console.WriteLine($"{currentSize},{watch.Elapsed}");

                currentSize += deltaN;

            }
        }
    }
}
