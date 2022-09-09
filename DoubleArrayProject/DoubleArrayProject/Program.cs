using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading.Tasks;

namespace DoubleArrayProject
{
    class Program
    {
        const int n = 1_000_000_000;

        static void Main(string[] args)
        {
            double[] numbers = new double[n];

            // initializing the array
            for (int i = 0; i < n; i++)
                numbers[i] = i;

            Stopwatch sw = new Stopwatch();

            // Doubling the numbers of the array sequentially

            sw.Restart();
            for (int i = 0; i < n; i++)
                numbers[i] *= 2;
            sw.Stop();
            Console.WriteLine("Sequential: {0:f4} sec.", sw.ElapsedMilliseconds / 1000d);

            // Doubling the numbers of the array using Parallel.For

            sw.Restart();
            Parallel.For(0, n, i =>
            {
                numbers[i] *= 2;
            });
            sw.Stop();
            Console.WriteLine("Parallel For: {0:f4} sec.", sw.ElapsedMilliseconds / 1000d);

            // Doubling the numbers of the array using Parallel.ForEach + Partitioner

            sw.Restart();
            Parallel.ForEach(Partitioner.Create(0, n), range =>
            {
                for (int i = range.Item1; i < range.Item2; i++)
                    numbers[i] *= 2;
            });
            sw.Stop();
            Console.WriteLine("Parallel ForEach + Partitioner: {0:f4} sec.", sw.ElapsedMilliseconds / 1000d);
        }
    }
}
