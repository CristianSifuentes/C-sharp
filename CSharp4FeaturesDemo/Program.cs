using System;
using System.Collections.Generic;

namespace CSharp4FeaturesDemo
{

// Example class to demonstrate dynamic binding
    public class DynamicExample
    {

    }

    // Interface to demonstrate covariance
    public interface IAnimal<out T>
    {

    }

    // Covariant implementation
    public class Dog : IAnimal<Dog>
    {

    }

    public class Cat : IAnimal<Cat>
    {

    }

    // Main Program
    class Program
    {
        // Method to demonstrate named and optional parameters
        public static void DisplayInfo(string message, string prefix = "Info", int repeat = 1)
        {

        }

        // Method demonstrating covariance
        public static void PrintAnimal(IAnimal<object> animal)
        {

        }

         static void Main(string[] args)
        {
            Console.WriteLine("Hi!");

        }

    }
}