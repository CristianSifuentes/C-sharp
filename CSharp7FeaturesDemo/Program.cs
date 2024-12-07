using System;

namespace CSharp7FeaturesDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("C# 7.0 - 7.3 Features Demo");

            // 1. Tuples
            Console.WriteLine("\nUsing Tuples:");
            var (name, age) = GetPerson();
            Console.WriteLine($"Name: {name}, Age: {age}");

            // 2. Pattern Matching
            Console.WriteLine("\nUsing Pattern Matching:");
            DescribeShape(new Circle(5));
            DescribeShape(new Rectangle(10, 20));
            DescribeShape(null);

            // 3. Local Functions
            Console.WriteLine("\nUsing Local Functions:");
            int Factorial(int n)
            {
                if (n == 0) return 1;
                return n * Factorial(n - 1);
            }
            Console.WriteLine($"Factorial of 5: {Factorial(5)}");

            // 4. Ref Returns
            Console.WriteLine("\nUsing Ref Returns:");
            int[] numbers = { 1, 2, 3, 4, 5 };
            ref int refNumber = ref FindRef(numbers, 3);
            Console.WriteLine($"Original value: {refNumber}");
            refNumber = 99; // Modifies the original array
            Console.WriteLine($"Modified array: {string.Join(", ", numbers)}");

            // 5. In Parameters
            Console.WriteLine("\nUsing In Parameters:");
            Console.WriteLine($"Sum (in parameter): {Sum(in numbers[0], in numbers[1])}");

            Console.WriteLine("\nC# 7.0 - 7.3 Features Demonstrated Successfully!");
        }

        // 1. Tuples: Method returning a tuple
        static (string Name, int Age) GetPerson()
        {
            return ("Alice", 30);
        }

        // 2. Pattern Matching: Using is and switch patterns
        static void DescribeShape(object shape)
        {
            switch (shape)
            {
                case Circle c:
                    Console.WriteLine($"Circle with radius {c.Radius}");
                    break;
                case Rectangle r when r.Length == r.Width:
                    Console.WriteLine($"Square with side {r.Length}");
                    break;
                case Rectangle r:
                    Console.WriteLine($"Rectangle with length {r.Length} and width {r.Width}");
                    break;
                case null:
                    Console.WriteLine("No shape provided.");
                    break;
                default:
                    Console.WriteLine("Unknown shape.");
                    break;
            }
        }

        // Shape classes for pattern matching
        public class Circle
        {
            public int Radius { get; }
            public Circle(int radius) => Radius = radius;
        }

        public class Rectangle
        {
            public int Length { get; }
            public int Width { get; }
            public Rectangle(int length, int width) => (Length, Width) = (length, width);
        }

        // 4. Ref Returns: Returning a reference to an array element
        static ref int FindRef(int[] array, int value)
        {
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] == value)
                    return ref array[i]; // Return by reference
            }
            throw new ArgumentException("Value not found in array");
        }

        // 5. In Parameters: Passing arguments by reference without modification
        static int Sum(in int a, in int b)
        {
            return a + b; // Cannot modify 'a' or 'b' inside this method
        }
    }
}
