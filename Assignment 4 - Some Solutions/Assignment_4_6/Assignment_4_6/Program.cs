using System.Collections.Concurrent;

namespace Assignment_4_6
{
    internal class Program
    {
        const int N = 100_000_000;
        static Random randomSeed = new Random();

        static void Main(string[] args)
        {
            double[] numbers = GetRandomArray(N, 1, 100);
            double time = TimeMeasurer.TimeMeasurer.MeasureTimeInSeconds(() => GetTopN(10, numbers));
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

        private static void GetTopN(int results, double[] numbers)
        {
            List<KeyValuePair<double, int>> highestFrequence =
                numbers.AsParallel()
                .Select(x => new KeyValuePair<double, int>(x, 1))

                .GroupBy(kv => kv.Key)
                .Select(g => new KeyValuePair<double, int>(g.Key, g.Count()))
                .AsSequential()
                .OrderByDescending(kv => kv.Value)
                .Take(results)
                .ToList();

            foreach (KeyValuePair<double, int> kvp in highestFrequence)
            {
                Console.WriteLine($"{kvp.Key,10}: {kvp.Value,10:N0}");
            }
        }
    }
}