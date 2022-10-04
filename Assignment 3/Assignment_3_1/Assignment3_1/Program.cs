using System.Diagnostics;

namespace Assignment3_1
{
    internal class Program
    {
        const int TASK_1_TIME = 5;
        const int TASK_2_TIME = 10;

        static void Main(string[] args)
        {
            MeasureTime(() => RunSequential());
            MeasureTime(() => RunParallel_1());
            MeasureTime(() => RunParallel_2());
        }


        static void MeasureTime(Action ac)
        {
            Stopwatch sw = Stopwatch.StartNew();
            ac.Invoke();
            sw.Stop();
            Console.WriteLine($"Time = {sw.Elapsed.TotalSeconds} seconds");
            Console.WriteLine();
        }
        static void RunSequential()
        {
            Console.WriteLine("Sequential approach");
            RunTime(1, TASK_1_TIME);
            RunTime(2, TASK_2_TIME);
        }

        static void RunParallel_1()
        {
            Console.WriteLine("Parallel approach using explicit tasks");
            Task t1 = Task.Factory.StartNew(() => RunTime(1, TASK_1_TIME));
            Task t2 = Task.Factory.StartNew(() => RunTime(2, TASK_2_TIME));
            Task.WaitAll(t1, t2);
        }
        static void RunParallel_2()
        {
            Console.WriteLine("Parallel approach using implicit tasks");
            Parallel.Invoke
            (
                () => RunTime(1, TASK_1_TIME),
                () => RunTime(2, TASK_2_TIME)
            );
        }

        static void RunTime(int taskID, int seconds)
        {
            Console.WriteLine($"Task {taskID} runs for {seconds} seconds");
            Task.Delay(seconds * 1000).Wait();
            //Thread.Sleep(seconds * 1000);
        }
    }
}