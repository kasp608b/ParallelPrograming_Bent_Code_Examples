using System.Diagnostics;

namespace Assignment_3_4
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Task t1 = Task.Run(() => Method5());
            Task t2 = Task.Run(() => Method10());
            try
            {
                Task.WaitAll(t1, t2);
            }
            catch (AggregateException ex)
            {
                ex.Flatten().Handle(e =>
                {
                    if (e is DivideByZeroException)
                    {
                        Console.WriteLine(e.Message);
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                });
            }

            Console.WriteLine("Task 1 (method5) status = " + t1.Status);
            Console.WriteLine("Task 2 (method10) status = " + t2.Status);
        }


        private static void Method5()
        {
            Console.WriteLine("Method5 runs for 5 seconds");
            Stopwatch sw = Stopwatch.StartNew();
            while (sw.ElapsedMilliseconds < 5000)
            {
                // do nothing
            }
            Console.WriteLine("Method5 DONE");
        }

        private static void Method10()  // has DivideByZeroException after 3 seconds
        {
            Console.WriteLine("Method10 runs for 10 seconds");
            Stopwatch sw = Stopwatch.StartNew();
            Thread.Sleep(50); // to bypass the first 50 ms
            while (sw.ElapsedMilliseconds < 10000)
            {
                if (sw.ElapsedMilliseconds % 3000 < 5) // happens after arround 3 seconds
                {
                    int x = 10;
                    int y = 0;
                    x = x / y;
                }
            }
            Console.WriteLine("Method10 DONE");
        }
    }
}