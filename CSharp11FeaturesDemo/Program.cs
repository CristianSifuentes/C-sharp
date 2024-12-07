using System;
using System.Collections.Generic;

namespace CSharp11FeaturesDemo;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("C# 11.0 Features Demo");

        // 1. Raw String Literals
        Console.WriteLine("\nUsing Raw String Literals:");
        var multiLineText = """
            This is a raw string literal.
            It preserves whitespace
            and supports multi-line text.
            """;
        Console.WriteLine(multiLineText);

        var jsonTemplate = """
            {
                "name": "Alice",
                "age": 30,
                "languages": ["C#", "Python", "JavaScript"]
            }
            """;
        Console.WriteLine("\nJSON Template:");
        Console.WriteLine(jsonTemplate);

        // 2. List Patterns
        Console.WriteLine("\nUsing List Patterns:");
        int[] numbers = { 1, 2, 3, 4, 5 };
        Console.WriteLine(numbers is [1, 2, .., 5] ? "List matches the pattern!" : "No match found.");

        // Matching specific patterns
        switch (numbers)
        {
            case [1, _, _, _, 5]:
                Console.WriteLine("The list starts with 1 and ends with 5.");
                break;
            default:
                Console.WriteLine("No specific match for the list.");
                break;
        }

        // 3. Generic Attributes
        Console.WriteLine("\nUsing Generic Attributes:");
        var processor = new DataProcessor();
        processor.Process();

        // 4. Improvements to Spans
        Console.WriteLine("\nUsing Improvements to Spans:");
        ReadOnlySpan<char> span = "Hello, C# 11!".AsSpan();
        Console.WriteLine($"First word in span: {span.Slice(0, 5).ToString()}");
        Console.WriteLine($"Last word in span: {span.Slice(7, 6).ToString()}");

        Console.WriteLine("\nC# 11.0 Features Demonstrated Successfully!");
    }
}

// 3. Generic Attributes
[AttributeUsage(AttributeTargets.Class)]
public class DataAttribute<T> : Attribute
{
    public string Name { get; }
    public DataAttribute(string name)
    {
        Name = name;
    }
}

// Using the generic attribute
[Data<int>("Example Data Processor")]
public class DataProcessor
{
    public void Process()
    {
        Console.WriteLine("Processing data using a generic attribute...");
    }
}
