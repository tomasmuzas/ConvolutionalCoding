using System;
using System.Drawing;
using System.Threading.Tasks;
using ConvolutionalCodes.Controllers;
using ConvolutionalCodes.Encoders;
using ConvolutionalCodes.Entities;
using ConvolutionalCodes.Utilities;

namespace ConvolutionalCodes.Benchmarks
{
    public class ErrorCorrectionBenchmark
    {
        public static async Task RunBenchmark(double errorDelta, double expectedErrorCorrectionRate)
        {
            var bmp = new Bitmap(100, 100);
            var bytes = BitmapHelper.GetColorBytes(bmp);

            double averageErrorCorrectionPercentage = 100;
            double currentErrorChance = 0;
            var encoder = new ConvolutionalEncoder();
            var decoder = new ConvolutionalDecoder();
            do
            {
                var noisyChannel = new NoisyChannel(currentErrorChance);

                double sum = 0;
                for (int i = 0; i < 10; i++)
                {
                    var resultWithoutEncoding = await MessageController.SendBytes(
                        bytes, noisyChannel
                    );


                    var resultWithEncoding = await MessageController.SendBytes(
                        bytes,
                        new NoisyChannel(currentErrorChance),
                        encoder,
                        decoder);

                    var Percentage =
                        MathHelper.CalculatePercentageWithPrecision(
                            resultWithEncoding.Errors,
                            resultWithoutEncoding.Errors,
                            2);

                    sum += Percentage;
                }

                averageErrorCorrectionPercentage = sum / 10;

                Console.WriteLine($"{currentErrorChance},{averageErrorCorrectionPercentage:F2}");

                currentErrorChance += errorDelta;

            } while(averageErrorCorrectionPercentage >= expectedErrorCorrectionRate);
        }
    }
}
