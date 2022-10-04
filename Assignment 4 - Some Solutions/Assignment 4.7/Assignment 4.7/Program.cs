using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Exercise_4_7
{
    class Program
    {
        static void Main(string[] args)
        {
            sequential();
            parallel_F1asTask();
            parallel_F2_F3asTask_WrongPlace();
            parallel_F32asTask_CorrectPlace();
            parallel_F1_F32asTasks();
        }

        static void sequential()
        {
            Console.WriteLine("Sequential F1->F3(F2)->F4");
            var sw = Stopwatch.StartNew();
            int a = 22;
            
            int b = F1(a);
            int cd = F3(F2(a));
            int e = F4(b, cd);
            
            sw.Stop();
            Console.WriteLine("e = {0}. Extepted = 505", e);
            Console.WriteLine("Time = {0:f3} sec.", sw.ElapsedMilliseconds / 1000d);
            Console.WriteLine();
        }

        static void parallel_F1asTask()
        {
            Console.WriteLine("Parallel (F1 + F3(F2))->F4");
            var sw = Stopwatch.StartNew();
            int a = 22;
            
            Task<int> t1 = Task.Factory.StartNew(() => F1(a));
            int cd = F3(F2(a));
            int e = F4(t1.Result, cd);
            
            sw.Stop();
            Console.WriteLine("e = {0}. Extepted = 505", e);
            Console.WriteLine("Time = {0:f3} sec.", sw.ElapsedMilliseconds / 1000d);
            Console.WriteLine();
        }

        static void parallel_F2_F3asTask_WrongPlace()
        {
            Console.WriteLine("Parallel: F1->F3(F2) as task->F4");
            var sw = Stopwatch.StartNew();
            int a = 22;

            int b = F1(a);
            Task<int> t2 = Task.Factory.StartNew(() => F3(F2(a)));
            int e = F4(b, t2.Result);
            
            sw.Stop();
            Console.WriteLine("e = {0}. Extepted = 505", e);
            Console.WriteLine("Time = {0:f3} sec.", sw.ElapsedMilliseconds / 1000d);
            Console.WriteLine();
        }

        static void parallel_F32asTask_CorrectPlace()
        {
            Console.WriteLine("Parallel: (F3(F2) + F1)->F4");
            var sw = Stopwatch.StartNew();
            int a = 22;

            Task<int> t2 = Task.Factory.StartNew(() => F3(F2(a)));
            int b = F1(a);
            int e = F4(b, t2.Result);
            
            sw.Stop();
            Console.WriteLine("Time = {0:f3} sec.", sw.ElapsedMilliseconds / 1000d);
            Console.WriteLine("e = {0}", e);
            Console.WriteLine();
        }

        static void parallel_F1_F32asTasks()
        {
            Console.WriteLine("Parallel: ((F1 as Task) + (F3(F2) as Task))->F4");
            var sw = Stopwatch.StartNew();
            int a = 22;

            Task<int> t1 = Task.Factory.StartNew(() => F1(a));
            Task<int> t2 = Task.Factory.StartNew(() => F3(F2(a)));
            int e = F4(t1.Result, t2.Result);
            
            sw.Stop();
            Console.WriteLine("Time = {0:f3} sec.", sw.ElapsedMilliseconds / 1000d);
            Console.WriteLine("e = {0}", e);
            Console.WriteLine();
        }

        static int F1(int value)
        {
            var sw = Stopwatch.StartNew();
            while (sw.ElapsedMilliseconds < 2000);            
            return value * value;
        }

        
        static int F2(int value)
        {
            var sw = Stopwatch.StartNew();
            while (sw.ElapsedMilliseconds < 2000);
            return value - 2;
        }

        
        static int F3(int value)
        {
            var sw = Stopwatch.StartNew();
            while (sw.ElapsedMilliseconds < 2000);
            return value + 1;
        }

        
        static int F4(int value1, int value2)
        {
            var sw = Stopwatch.StartNew();
            while (sw.ElapsedMilliseconds < 1000);
            return value1 + value2;
        }

    }
}
