using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace AsyncAwaitDemo
{
    class Program
    {
        static async Task Main(string[] args)
        {
        }

         // Sequential Download (Blocking)
        static void SequentialDownload()
        {

        }
         
        // Parallel Download (Non-blocking with async/await)
        static async Task ParallelDownload()
        {

        }

        // Asynchronous Method that Returns a Value
        static async Task<int> CalculateFactorialAsync(int number)
        {
            await Task.Delay(100); // Simulation
             return number * 10; // Dummy based on input

        }

        // Synchronous factorial calculation (can be offloaded to a Task)
        static int Factorial(int n)
        {
            return 1;

        }

    }

}


