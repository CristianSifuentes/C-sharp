using System;
using System.Collections.Generic;

namespace CSharp12FeaturesDemo;

// Primary Constructor in Non-Record Class
public class Employee(string name, int id)
{
    public string Name { get; } = name;
    public int Id { get; } = id;

    public override string ToString() => $"Employee: {Name}, ID: {Id}";
}

// Primary Constructor in Structure
public readonly struct Point(int x, int y)
{
    public int X { get; } = x;
    public int Y { get; } = y;

    public override string ToString() => $"Point({X}, {Y})";
}

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("C# 12.0 Features Demo");

        // 1. Primary Constructors in Non-Record Classes and Structures
        Console.WriteLine("\nPrimary Constructors:");
        var employee = new Employee("Alice", 101);
        Console.WriteLine(employee);

        var point = new Point(10, 20);
        Console.WriteLine(point);

        // 2. Collection Expressions
        Console.WriteLine("\nCollection Expressions:");
        var numbers = [1, 2, 3, 4, 5];
        var names = ["Alice", "Bob", "Charlie"];
        Console.WriteLine($"Numbers: {string.Join(", ", numbers)}");
        Console.WriteLine($"Names: {string.Join(", ", names)}");

        // 3. Inline Arrays
        Console.WriteLine("\nInline Arrays:");
        InlineArrayExample();

        // 4. Using Aliases for Any Type
        Console.WriteLine("\nUsing Aliases for Any Type:");
        var aliasInstance = new PointAlias(5, 10);
        Console.WriteLine($"PointAlias: {aliasInstance}");

        // 5. Interceptors (Experimental)
        Console.WriteLine("\nInterceptors (Experimental):");
        var mathService = new MathService();
        Console.WriteLine($"Addition: {mathService.Add(5, 10)}");

        Console.WriteLine("\nC# 12.0 Features Demonstrated Successfully!");
    }

    // 3. Inline Arrays
    static void InlineArrayExample()
    {
        var inlineArray = new MyInlineArray();
        inlineArray[0] = 42;
        Console.WriteLine($"Inline array element: {inlineArray[0]}");
    }
}

// 3. Inline Arrays Example
[System.Runtime.CompilerServices.InlineArray(10)]
public struct MyInlineArray
{
    private int _element;
}

// 4. Using Aliases for Any Type
using PointAlias = CSharp12FeaturesDemo.Point;

// 5. Interceptors (Experimental)
public interface IMathService
{
    int Add(int a, int b);
}

// Interceptor Attribute
public class InterceptorAttribute : Attribute
{
    public static IMathService CreateInterceptor() => new InterceptorImplementation();
}

// Interceptor Implementation
public class InterceptorImplementation : IMathService
{
    public int Add(int a, int b)
    {
        Console.WriteLine("Intercepting Add method...");
        return a + b;
    }
}

// MathService that uses Interceptors
[Interceptor]
public class MathService : IMathService
{
    public int Add(int a, int b) => a + b;
}
