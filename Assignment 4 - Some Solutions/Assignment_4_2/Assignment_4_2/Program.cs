using System.Collections.Concurrent;

namespace Assignment_4_2
{
    internal class Program
    {
        const int N = 100_000_000;
        static Random randomSeed = new Random(1234);

        static void Main(string[] args)
        {
            double[] numbers = GetRandomArray(N, 1, 100);
            double time = TimeMeasurer.TimeMeasurer.MeasureTimeInSeconds(() => CalculateSum2(numbers));
            Console.WriteLine($"Time = {time} seconds");
        }

        private static double[] GetRandomArray(int size, int minValue, int maxValue)
        {
            double[] b = new double[size];
            Parallel.ForEach(
                Partitioner.Create(0, size),
                new ParallelOptions(),
                () => { return new Random(GetRandomSeed()); },
                (range, loopState, rnd) =>
                {
                    for (int i = range.Item1; i < range.Item2; i++)
                        b[i] = rnd.Next(minValue, maxValue + 1);
                    return rnd;
                },
                _ => { }
            );
            return b;
        }

        private static int GetRandomSeed()
        {
            lock (randomSeed)
            {
                return randomSeed.Next();
            }
        }

        private static void CalculateSum2(double[] numbers)
        {
            double sum = 0;
            object sumLock = new object();

            Parallel.For(0, numbers.Length, i =>
            {
                lock (sumLock)
                {
                    sum += numbers[i];
                }
            });
            Console.WriteLine($"Sum = {sum:N0}");
        }


    }
}