using System;
using System.Collections.Generic;

namespace CSharp4FeaturesDemo
{
    // Example class to demonstrate dynamic binding
    public class DynamicExample
    {
        public void SayHello(string name)
        {
            Console.WriteLine($"Hello, {name}!");
        }
    }

    // Interface to demonstrate covariance
    public interface IAnimal<out T>
    {
        T GetAnimal();
    }

    // Covariant implementation
    public class Dog : IAnimal<Dog>
    {
        public Dog GetAnimal() => new Dog();

        public override string ToString() => "I am a Dog.";
    }

    public class Cat : IAnimal<Cat>
    {
        public Cat GetAnimal() => new Cat();

        public override string ToString() => "I am a Cat.";
    }

    // Main Program
    class Program
    {
        // Method to demonstrate named and optional parameters
        public static void DisplayInfo(string message, string prefix = "Info", int repeat = 1)
        {
            for (int i = 0; i < repeat; i++)
            {
                Console.WriteLine($"[{prefix}] {message}");
            }
        }

        // Method demonstrating covariance
        public static void PrintAnimal(IAnimal<object> animal)
        {
            Console.WriteLine($"Animal: {animal.GetAnimal()}");
        }

        static void Main(string[] args)
        {
            // Dynamic binding
            Console.WriteLine("Dynamic Binding:");
            //dynamic is a keyword in C# that allows for dynamic typing
            // and late binding. It is used to bypass compile-time type checking.
            // The dynamic type can hold any type of object, and the actual type is resolved at runtime.
            //Represents an object whose operations will be resolved at runtime.
            dynamic dynamicObject = new DynamicExample();
            dynamicObject.SayHello("Alice"); // Runtime binding
            Console.WriteLine();

            // Named and Optional Parameters
            Console.WriteLine("Named and Optional Parameters:");
            DisplayInfo("This is a standard message."); // Using defaults
            DisplayInfo("This is a warning message.", prefix: "Warning"); // Named parameter
            DisplayInfo("Repeating message.", repeat: 3); // Mixing defaults and named parameters
            Console.WriteLine();

            // Covariance
            Console.WriteLine("Covariance:");
            IAnimal<object> dog = new Dog(); // Covariance allows this assignment
            IAnimal<object> cat = new Cat(); // Covariance allows this assignment
            PrintAnimal(dog);
            PrintAnimal(cat);
            Console.WriteLine();

            object exp = "Hello, World!";
            string str = exp as string; // Using 'as' operator for safe casting

            object anotherObject = str;




            Console.WriteLine("C# 4.0 Features Demonstrated Successfully!");
        }
    }
}
