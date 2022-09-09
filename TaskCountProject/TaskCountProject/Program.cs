using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace TaskCountProject
{
    class Program
    {
        static object countLock = new object();
        static int count = 0;

        static void Main(string[] args)
        {
            ConcurrentDictionary<int, int> ids = new();

            Console.Write("size of array: ");
            int n = 0;
            while (!int.TryParse(Console.ReadLine(), out n))
            {
                Console.WriteLine("value is not a number");
                Console.Write("size of array: ");
            }

            //Parallel.ForEach(Enumerable.Range(0, n), i =>
            Parallel.ForEach(Partitioner.Create(0, n), range =>
            {
                Interlocked.Increment(ref count);
                ids.AddOrUpdate(Task.CurrentId.Value, 1, (id, value) => value += 1);

                //Console.WriteLine("Id: {0}, value = {1}", Task.CurrentId.Value, i);
               // Console.WriteLine("Id: {0}, [{1} - {2}]", Task.CurrentId.Value, range.Item1, range.Item2);
            });

            Console.WriteLine("\nNumber of Tasks created = " + count);
            Console.WriteLine("\nNumber of kernel threads used = " + ids.Distinct().Count());
            foreach (KeyValuePair<int, int> kv in ids)
                Console.WriteLine("Id: {0}, Times used: {1}", kv.Key, kv.Value);
        }
    }
}
