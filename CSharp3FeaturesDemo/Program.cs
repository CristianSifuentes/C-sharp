using System;
using System.Collections.Generic;
using System.Linq;

// Namespace encapsulating the application
namespace CSharp3FeaturesDemo
{
    // Class with automatic properties
    public class Product
    {
        // Automatic Properties
        public string Name { get; set; }
        public decimal Price { get; set; }

        public override string ToString()
        {
            return $"Product: {Name}, Price: {Price:C}";
        }
    }

     // Static class containing extension methods
    public static class ProductExtensions
    {

    }

    class Program
    {
        static void Main(string[] args)
        {

        }

    }

}