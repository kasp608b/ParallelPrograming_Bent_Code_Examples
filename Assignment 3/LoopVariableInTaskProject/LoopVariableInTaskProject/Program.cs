namespace LoopVariableInTaskProject
{
    internal class Program
    {
        static void Main(string[] args)
        {
            for (int i = 1; i <= 10; i++)
            {
                int tmp = i;
                Task.Factory.StartNew(() => Go(tmp));
            }
            Console.ReadLine();
        }

        static void Go(int i)
        {
            Console.WriteLine("Hello from task " + i);
        }
    }
}