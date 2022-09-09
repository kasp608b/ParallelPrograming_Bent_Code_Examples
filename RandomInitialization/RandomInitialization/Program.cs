using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace RandomInitialization
{
    class Program
    {
        static int seed = 1234;
        static object seedLock = new object();
        static void Main(string[] args)
        {
            const int N = 500_000_000;
            Stopwatch sw = new Stopwatch();

            sw.Restart();
            int[] numbers = InitializeArraySequential(N);
            sw.Stop();
            Console.WriteLine("InitializeSequential - Time = {0:f4}", sw.ElapsedMilliseconds / 1000d);

            numbers = null;
            System.GC.Collect();

            sw.Restart();
            numbers = InitializeArrayParallel(N);
            sw.Stop();
            Console.WriteLine("InitializeParallel 1 - Time = {0:f4}", sw.ElapsedMilliseconds / 1000d);
            
            sw.Restart();
            numbers = InitializeArrayParallel2(N);
            sw.Stop();
            Console.WriteLine("InitializeParallel 2 - Time = {0:f4}", sw.ElapsedMilliseconds / 1000d);




        }

        private static int[] InitializeArraySequential(int n)
        {
            int[] numbers = new int[n];
            Random rnd = new Random();
            for (int i = 0; i < n; i++)
                numbers[i] = rnd.Next(n / 2);
            return numbers;
        }

        private static int[] InitializeArrayParallel(int n)
        {
            int[] numbers = new int[n];
            Parallel.ForEach(
                Partitioner.Create(0, n),
                () => { return new Random(RandomSeed()); },
                (range, loopState, rnd) =>
                {
                    for (int i = range.Item1; i < range.Item2; i++)
                    {
                        numbers[i] = rnd.Next(n / 2);
                    }
                    return rnd;
                },
                (_) => { });

            return numbers;
        }
        private static int[] InitializeArrayParallel2(int n)
        {
            int[] numbers = new int[n];
            Parallel.ForEach(
                Partitioner.Create(0, n),
                (range) =>
                {
                    Random rnd = new Random(RandomSeed());
                    for (int i = range.Item1; i < range.Item2; i++)
                    {
                        numbers[i] = rnd.Next(n / 2);
                    }
                });

            return numbers;
        }

        static int RandomSeed()
        {
            lock(seedLock)
            {
                return seed++;
            }
        }
    }
}
