using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// C# 10.0 - File-scoped namespaces
namespace AdvancedCSharpDemo;

// Main Program
class Program
{
    // C# 9.0 - Record types
    public record Person(string Name, int Age);

    // C# 12.0 - Primary constructors for non-records
    public class Student(string name, int age)
    {
        public string Name => name;
        public int Age => age;
    }

    // C# 1.0 - Classes and methods
    public class LegacyFeature
    {
        public string Greet(string name) => $"Hello, {name}!";
    }

    // C# 2.0 - Generics
    public static T GetFirst<T>(List<T> items)
    {
        return items[0];
    }

    // C# 3.0 - LINQ and Lambda Expressions
    public static IEnumerable<int> FilterEvenNumbers(IEnumerable<int> numbers)
    {
        return numbers.Where(x => x % 2 == 0);
    }

    // C# 4.0 - Optional Parameters
    public static void PrintMessage(string message, string prefix = "Info")
    {
        Console.WriteLine($"[{prefix}] {message}");
    }

    // C# 5.0 - Async and Await
    public static async Task<int> GetSumAsync(int a, int b)
    {
        await Task.Delay(1000); // Simulate async operation
        return a + b;
    }

    // C# 6.0 - Expression-bodied members and string interpolation
    public static string WelcomeMessage => $"Welcome to the Advanced C# Demo!";

    // C# 7.0 - Tuples
    public static (string Name, int Age) GetPersonDetails()
    {
        return ("John Doe", 30);
    }

    // C# 7.3 - In parameters
    public static int Increment(in int value)
    {
        return value + 1; // Note: in-parameter can't be modified directly
    }

    // C# 8.0 - Async Streams and Switch Expressions
    public static async IAsyncEnumerable<int> GetNumbersAsync()
    {
        for (int i = 1; i <= 5; i++)
        {
            await Task.Delay(500); // Simulate async operation
            yield return i;
        }
    }

    // C# 9.0 - Init-only Properties
    public class Config
    {
        public string ApplicationName { get; init; }
    }

    // C# 10.0 - Global using statements (see namespace declaration)
    public static List<int> Numbers = new() { 1, 2, 3, 4, 5 };

    // C# 11.0 - Raw String Literals
    public static string GetRawString() => """
        This is a raw string literal.
        Multi-line strings are easier to read now!
        """;

    // C# 12.0 - Collection Expressions
    public static int[] GetNumbers() => [1, 2, 3, 4, 5];

    static async Task Main()
    {
        // C# 1.0 - Basic usage of classes and methods
        var legacy = new LegacyFeature();
        Console.WriteLine(legacy.Greet("Alice"));

        // C# 2.0 - Using generics
        var numbers = new List<int> { 1, 2, 3, 4 };
        Console.WriteLine($"First number: {GetFirst(numbers)}");

        // C# 3.0 - LINQ and Lambda expressions
        var evenNumbers = FilterEvenNumbers(numbers);
        Console.WriteLine($"Even numbers: {string.Join(", ", evenNumbers)}");

        // C# 4.0 - Optional parameters
        PrintMessage("This is an important message!", "Alert");
        PrintMessage("This is a regular message.");

        // C# 5.0 - Async and Await
        var sum = await GetSumAsync(3, 5);
        Console.WriteLine($"Sum: {sum}");

        // C# 6.0 - Expression-bodied members
        Console.WriteLine(WelcomeMessage);

        // C# 7.0 - Tuples
        var person = GetPersonDetails();
        Console.WriteLine($"Name: {person.Name}, Age: {person.Age}");

        // C# 7.3 - In parameters
        const int value = 10;
        Console.WriteLine($"Incremented value: {Increment(value)}");

        // C# 8.0 - Async Streams
        await foreach (var number in GetNumbersAsync())
        {
            Console.WriteLine($"Async number: {number}");
        }

        // C# 9.0 - Init-only properties
        var config = new Config { ApplicationName = "AdvancedCSharpDemo" };
        Console.WriteLine($"Application Name: {config.ApplicationName}");

        // C# 10.0 - Global usings and collection initializers
        Console.WriteLine($"Numbers: {string.Join(", ", Numbers)}");

        // C# 11.0 - Raw string literals
        Console.WriteLine(GetRawString());

        // C# 12.0 - Collection expressions
        Console.WriteLine($"Numbers from collection expression: {string.Join(", ", GetNumbers())}");

        // C# 12.0 - Primary constructors
        var student = new Student("Bob", 21);
        Console.WriteLine($"Student: {student.Name}, Age: {student.Age}");
    }
}
