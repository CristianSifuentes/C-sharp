using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CSharp8FeaturesDemo
{
    // Interface with a Default Interface Method
    public interface ILogger
    {
        // Default Method
        void Log(string message)
        {
            Console.WriteLine($"[INFO]: {message}");
        }

        // Abstract Method (must be implemented)
        void LogError(string error);
    }

    // Class implementing ILogger
    public class ConsoleLogger : ILogger
    {
        public void LogError(string error)
        {
            Console.WriteLine($"[ERROR]: {error}");
        }
    }

    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("C# 8.0 Features Demo");

            // 1. Nullable Reference Types
            Console.WriteLine("\nUsing Nullable Reference Types:");
            string? nullableString = null; // Nullable reference type
            string nonNullableString = "Hello, C# 8.0!";
            
            // Safe usage with nullable types
            Console.WriteLine(nullableString?.ToUpper() ?? "String is null"); // Null-safe access
            Console.WriteLine(nonNullableString.ToUpper());

            // 2. Async Streams
            Console.WriteLine("\nUsing Async Streams:");
            await foreach (var number in GetNumbersAsync())
            {
                Console.WriteLine($"Number: {number}");
            }

            // 3. Default Interface Methods
            Console.WriteLine("\nUsing Default Interface Methods:");
            ILogger logger = new ConsoleLogger();
            logger.Log("This is a default method from the interface.");
            logger.LogError("This is an error logged from the implementation.");

            Console.WriteLine("\nC# 8.0 Features Demonstrated Successfully!");
        }

        // Async Stream Method
        static async IAsyncEnumerable<int> GetNumbersAsync()
        {
            for (int i = 1; i <= 5; i++)
            {
                await Task.Delay(500); // Simulate asynchronous work
                yield return i;
            }
        }
    }
}
