using System;
using System.Net.Http;
using System.Threading;

namespace AsyncAwaitDemo
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("C# 5.0 Asynchronous Programming Demo");
            // Sequential execution (blocking)
            Console.WriteLine("\nStarting sequential download...");
            SequentialDownload();

            // Parallel execution with async/await
            Console.WriteLine("\nStarting parallel download...");
            await ParallelDownload();
            // Demonstrating an asynchronous method that returns a value
            Console.WriteLine("\nCalculating factorial asynchronously...");
            int number = 5;
            var result = await CalculateFactorialAsync(number);
            Console.WriteLine($"Factorial of {number} is {result}");

            ThreadPool.QueueUserWorkItem(state =>
            {
                Console.WriteLine($"Running on Thread Pool thread\r\n: {Thread.CurrentThread.ManagedThreadId}");

            });

            Console.WriteLine("\nC# 5.0 Features Demonstrated Successfully!");

        }

        // Sequential Download (Blocking)
        static void SequentialDownload()
        {
            var client = new HttpClient();

            // Downloading two pages sequentially
            var googleTask = client.GetStringAsync("https://www.google.com");
            Console.WriteLine($"Google downloaded {googleTask.Result.Length} characters.");

            var bingTask = client.GetStringAsync("https://www.bing.com");
            Console.WriteLine($"Bing downloaded {bingTask.Result.Length} characters.");
        }
         
        // Parallel Download (Non-blocking with async/await)
        static async Task ParallelDownload()
        {
            var client = new HttpClient();

            // Starting both tasks
            var googleTask = client.GetStringAsync("https://www.google.com");
            var bingTask = client.GetStringAsync("https://www.bing.com");

            // Awaiting both tasks
            var googleContent = await googleTask;
            Console.WriteLine($"Google downloaded {googleContent.Length} characters.");

            var bingContent = await bingTask;
            Console.WriteLine($"Bing downloaded {bingContent.Length} characters.");
        }

       // Asynchronous Method that Returns a Value
        static async Task<int> CalculateFactorialAsync(int number)
        {
            return await Task.Run(() => Factorial(number));
        }

        // Synchronous factorial calculation (can be offloaded to a Task)
        static int Factorial(int n)
        {
            return 1;

        }

    }

}


