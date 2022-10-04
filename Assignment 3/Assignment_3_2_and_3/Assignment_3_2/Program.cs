using System.Diagnostics;

namespace Assignment_3_2
{
    internal class Program
    {
        const int TASK_1_TIME = 5;
        const int TASK_2_TIME = 10;

        static void Main(string[] args)
        {
            CancellationTokenSource cts = new CancellationTokenSource();
            CancellationToken token = cts.Token;

            Task.Factory.StartNew(() => CancelationTask(cts));

            Task t1 = Task.Factory.StartNew(() => RunTime_2(1, TASK_1_TIME, token), token);
            Task t2 = Task.Factory.StartNew(() => RunTime_2(2, TASK_2_TIME, token), token);

            try
            {
                Task.WaitAll(t1, t2);
            }
            catch (AggregateException ae)
            {
                ae.Handle(e =>
                {
                    if (e is OperationCanceledException)
                    {
                        Console.WriteLine("Exception message: " + e.Message);
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                });
            }
            Console.WriteLine("Task 1 status = " + t1.Status);
            Console.WriteLine("Task 2 status = " + t2.Status);
        }

        private static void CancelationTask(CancellationTokenSource cts)
        {
            Console.WriteLine("Press any key to cancel...");
            Console.ReadKey();
            Console.WriteLine("Canceling all tasks...");
            cts.Cancel();
        }

        static void RunTime_1(int taskID, int seconds, CancellationToken token)
        {
            Console.WriteLine($"Task {taskID} runs for {seconds} seconds");
            Stopwatch sw = Stopwatch.StartNew();
            while (sw.Elapsed.TotalSeconds <= seconds)
            {
                if (sw.ElapsedMilliseconds % 1000 < 20)
                {
                    if (token.IsCancellationRequested)
                    {
                        Console.WriteLine($"Task {taskID} was canceled");
                        break;
                    }
                }
            }
            Console.WriteLine($"Task {taskID} DONE");
        }
        static void RunTime_2(int taskID, int seconds, CancellationToken token)
        {
            Console.WriteLine($"Task {taskID} runs for {seconds} seconds");
            Stopwatch sw = Stopwatch.StartNew();
            while (sw.Elapsed.TotalSeconds <= seconds)
            {
                if (sw.ElapsedMilliseconds % 1000 < 20)
                {
                    if (token.IsCancellationRequested)
                    {
                        Console.WriteLine($"Task {taskID} was canceled");

                        //throw new OperationCanceledException("Hey dude, we are canceling the meeting");
                        token.ThrowIfCancellationRequested();
                    }
                }
            }
            Console.WriteLine($"Task {taskID} DONE");
        }
    }
}
