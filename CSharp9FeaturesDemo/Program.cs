using System;

// C# 9.0 Features Demo

// 1. Record Type
var person = new Person("Alice", 30);
Console.WriteLine($"Record: {person}");

// Using with-expression to create a copy with modifications
var updatedPerson = person with { Age = 31 };
Console.WriteLine($"Updated Record: {updatedPerson}");

// 2. Init-Only Properties
var car = new Car { Make = "Toyota", Model = "Camry", Year = 2022 };
// car.Year = 2023; // This line would throw a compile-time error due to init-only properties
Console.WriteLine($"Car: {car.Make} {car.Model} ({car.Year})");

// 3. Improved Pattern Matching
object shape = new Rectangle(10, 20);
DescribeShape(shape);

// Additional pattern matching with positional records
shape = new Circle(5);
DescribeShape(shape);

// End of Program
Console.WriteLine("\nC# 9.0 Features Demonstrated Successfully!");

// Record Type
public record Person(string Name, int Age);

// Improved Pattern Matching: Positional Record Example
public record Circle(int Radius);
public record Rectangle(int Length, int Width);

// Pattern Matching Function
void DescribeShape(object shape)
{
    Console.WriteLine("\nPattern Matching:");
    switch (shape)
    {
        case Circle(int radius):
            Console.WriteLine($"Circle with radius {radius}");
            break;
        case Rectangle(int length, int width) when length == width:
            Console.WriteLine($"Square with side {length}");
            break;
        case Rectangle(int length, int width):
            Console.WriteLine($"Rectangle with length {length} and width {width}");
            break;
        default:
            Console.WriteLine("Unknown shape.");
            break;
    }
}

// Init-Only Properties Example
public class Car
{
    public string Make { get; init; }
    public string Model { get; init; }
    public int Year { get; init; }
}
