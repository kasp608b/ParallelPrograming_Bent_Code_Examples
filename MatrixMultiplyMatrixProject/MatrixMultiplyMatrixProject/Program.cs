using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace MatrixMultiplicationProject
{
    class Program
    {
        private const int N = 1000;
        static void Main()
        {
            Console.WriteLine("Initializing matrices A and B...");
            double[,] A = InitializeMatrix(N, N, 'A');
            double[,] B = InitializeMatrix(N, N, 'B');

            Console.WriteLine("Calculating C = A * B...");
            double[,] C;

            C = NaiveMatrixMultiply(A, B);
            
            C = CacheFriendlyMatrixMultiply(A, B);
            
            C = ParallelNaiveMatrixMultiply(A, B);
            
            C = ParallelCacheFriendlyMatrixMultiply(A, B);
            
            //ChechMatrix(C);
            Console.ReadKey();
        }

        private static void ChechMatrix(double[,] C)
        {
            Console.WriteLine("TL = " + C[0, 0] + ", expected = " + N);
            Console.WriteLine("TR = " + C[0, N - 1] + ", expected = " + N * N);
            Console.WriteLine("BL = " + C[N - 1, 0] + ", expected = " + N * N);
            Console.WriteLine("BR = " + C[N - 1, N - 1] + ", expected = " + N * N * N);
        }

        static double[,] InitializeMatrix(int rows, int cols, char matrixType)
        {
            double[,] tmp = new double[rows, cols];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                    tmp[i, j] = (matrixType == 'A' ? i + 1 : j + 1);
            }
            return tmp;
        }

        static double[,] NaiveMatrixMultiply(double[,] A, double[,] B)
        {
            Console.Write($"{"Naive Matrix Multiply...", -50}");
            Stopwatch sw = Stopwatch.StartNew();

            int rows = A.GetLength(0);
            int cols = B.GetLength(1);
            int intermediate = A.GetLength(1);

            double[,] C = new double[rows, cols];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    C[i, j] = 0;
                    for (int k = 0; k < intermediate; k++)
                    {
                        C[i, j] += A[i, k] * B[k, j];
                    }
                }
            }
            sw.Stop();
            long time = sw.ElapsedMilliseconds;

            Console.WriteLine("Done! Time = {0:f3} sec.", time / 1000d);
            Console.WriteLine();
            return C;
        }

        static double[,] CacheFriendlyMatrixMultiply(double[,] A, double[,] B)
        {
            Console.Write($"{"Cache Friendly Matrix Multiply...", -50}");
            Stopwatch sw = Stopwatch.StartNew();

            int rows = A.GetLength(0);
            int cols = B.GetLength(1);
            int intermediate = A.GetLength(1);

            double[,] C = new double[rows, cols];

            // TO DO
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    C[i, j] = 0;
                }
            }

            for (int i = 0; i < rows; i++)
            {
                for (int k = 0; k < intermediate; k++)
                {
                    for (int j = 0; j < cols; j++)
                    {
                        C[i, j] += A[i, k] * B[k, j];
                    }
                }
            }
            sw.Stop();
            long time = sw.ElapsedMilliseconds;

            Console.WriteLine("Done! Time = {0:f3} sec.", time / 1000d);
            Console.WriteLine();
            return C;
        }

        static double[,] ParallelCacheFriendlyMatrixMultiply(double[,] A, double[,] B)
        {
            Console.Write($"{"Cache Friendly Parallel Matrix Multiply...", -50}");
            Stopwatch sw = Stopwatch.StartNew();

            int rows = A.GetLength(0);
            int cols = B.GetLength(1);
            int intermediate = A.GetLength(1);

            double[,] C = new double[rows, cols];

            // TO DO
            Parallel.For(0, rows, (i) =>
             {
                 for (int j = 0; j < cols; j++)
                 {
                     C[i, j] = 0;
                 }
             });

            Parallel.For(0, rows, (i) =>
             {
                 for (int k = 0; k < intermediate; k++)
                 {
                     for (int j = 0; j < cols; j++)
                     {
                         C[i, j] += A[i, k] * B[k, j];
                     }
                 }
             });

            sw.Stop();
            long time = sw.ElapsedMilliseconds;

            Console.WriteLine("Done! Time = {0:f3} sec.", time / 1000d);
            Console.WriteLine();
            return C;
        }

        static double[,] ParallelNaiveMatrixMultiply(double[,] A, double[,] B)
        {
            Console.Write($"{"Parallel Naive Matrix Multiply...", -50}");
            Stopwatch sw = Stopwatch.StartNew();

            int rows = A.GetLength(0);
            int cols = B.GetLength(1);
            int intermediate = A.GetLength(1);

            double[,] C = new double[rows, cols];

            Parallel.For(0, rows, (i) =>
              {
                  for (int j = 0; j < cols; j++)
                  {
                      C[i, j] = 0;
                      for (int k = 0; k < intermediate; k++)
                      {
                          C[i, j] += A[i, k] * B[k, j];
                      }
                  }
              });

            sw.Stop();
            long time = sw.ElapsedMilliseconds;

            Console.WriteLine("Done! Time = {0:f3} sec.", time / 1000d);
            Console.WriteLine();
            return C;
        }
    }
}
