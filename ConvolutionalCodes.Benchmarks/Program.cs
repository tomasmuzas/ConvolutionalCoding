using System;
using System.Threading.Tasks;

namespace ConvolutionalCodes.Benchmarks
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            RunTimeBenchmark.RunBenchmark(100, 1000).Wait();
            Console.WriteLine("Done.");
            Console.ReadKey();
        }
    }
}
