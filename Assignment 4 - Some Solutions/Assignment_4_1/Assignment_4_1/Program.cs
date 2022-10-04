using System.Collections.Concurrent;

namespace Assignment_4_1
{
    internal class Program
    {
        const int N = 100000000;
        static Random randomSeed = new Random(1234);

        static void Main(string[] args)
        {
            Console.WriteLine($"Creating random array of size: {N:N0}\n");
            double[] numbers = GetRandomArray(N, 1, 100);

            Console.WriteLine("Assignment 4.1: Sum by Sequential adding");
            double time = TimeMeasurer.TimeMeasurer.MeasureTimeInSeconds(() => CalculateSum1(numbers));
            Console.WriteLine($"Time = {time} seconds\n");

            Console.WriteLine("Assignment 4.2: Sum by Parallel.For loop");
            time = TimeMeasurer.TimeMeasurer.MeasureTimeInSeconds(() => CalculateSum2(numbers));
            Console.WriteLine($"Time = {time} seconds\n");

            Console.WriteLine("Assignment 4.3: Sum by Parallel.Foreach with Partitioner and Task-Local localSum");
            time = TimeMeasurer.TimeMeasurer.MeasureTimeInSeconds(() => CalculateSum3(numbers));
            Console.WriteLine($"Time = {time} seconds\n");

            Console.WriteLine("Assignment 4.4: Sum by Parallel.Foreach with Partitioner");
            time = TimeMeasurer.TimeMeasurer.MeasureTimeInSeconds(() => CalculateSum4(numbers));
            Console.WriteLine($"Time = {time} seconds\n");

            Console.WriteLine("Assignment 4.5: Sum by using PLINQ");
            time = TimeMeasurer.TimeMeasurer.MeasureTimeInSeconds(() => CalculateSum5(numbers));
            Console.WriteLine($"Time = {time} seconds\n");
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
                        b[i] = rnd.Next(minValue, maxValue+1);
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

        private static void CalculateSum1(double[] numbers)
        {
            double sum = 0;
            for (int i = 0; i < numbers.Length; i++)
            {
                sum += numbers[i];
            }
            Console.WriteLine($"Sum = {sum:N0}");
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

        private static void CalculateSum3(double[] numbers)
        {
            double sum = 0;
            object sumLock = new object();

            Parallel.ForEach(
                Partitioner.Create(0, numbers.Length),
                new ParallelOptions(),
                () => { return 0d; },
                (range, loopState, localSum) =>
                {
                    for (int i = range.Item1; i < range.Item2; i++)
                        localSum += numbers[i];
                    return localSum;
                },
                localSum =>
                {
                    lock (sumLock)
                    {
                        sum += localSum;
                    }
                });
            Console.WriteLine($"Sum = {sum:N0}");
        }

        private static void CalculateSum4(double[] numbers)
        {
            double sum = 0d;
            object sumLock = new object();

            Parallel.ForEach(
                Partitioner.Create(0, numbers.Length),
                (range) =>
                {
                    double localSum = 0d;
                    for (int i = range.Item1; i < range.Item2; i++)
                    {
                        localSum += numbers[i];
                    }
                    lock (sumLock)
                    {
                        sum += localSum;
                    }
                });
            Console.WriteLine($"Sum = {sum:N0}");
        }

        private static void CalculateSum5(double[] numbers)
        {
            double sum = numbers.AsParallel().Sum();
            Console.WriteLine($"Sum = {sum:N0}");
        }


    }
}