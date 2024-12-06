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
        // Extension Method for Product
        public static bool IsExpensive(this Product product, decimal threshold)
        {
            return product.Price > threshold;
        }

        // Extension Method for IEnumerable<Product> using LINQ
        public static IEnumerable<Product> FilterByPrice(this IEnumerable<Product> products, decimal minPrice)
        {
            return products.Where(p => p.Price >= minPrice);
        }
    }

    // Main Program demonstrating C# 3.0 features
    class Program
    {
        static void Main(string[] args)
        {
            // Creating a list of products (Automatic Properties)
            var products = new List<Product>
            {
                new Product { Name = "Laptop", Price = 1500.00m },
                new Product { Name = "Mouse", Price = 25.00m },
                new Product { Name = "Keyboard", Price = 75.00m },
                new Product { Name = "Monitor", Price = 300.00m },
                new Product { Name = "USB Cable", Price = 10.00m }
            };

            // LINQ Query to filter products by price
            var expensiveProducts = from p in products
                                    where p.Price > 100
                                    orderby p.Price descending
                                    select p;

            Console.WriteLine("Products with Price > 100 (Using LINQ Query):");
            foreach (var product in expensiveProducts)
            {
                Console.WriteLine(product);
            }

            // Lambda Expressions with LINQ
            var cheapProducts = products.Where(p => p.Price <= 100);
            Console.WriteLine("\nProducts with Price <= 100 (Using Lambda Expressions):");
            foreach (var product in cheapProducts)
            {
                Console.WriteLine(product);
            }

            // Using Extension Method IsExpensive
            var laptop = products.First(p => p.Name == "Laptop");
            Console.WriteLine($"\nIs the laptop expensive? {laptop.IsExpensive(1000)}");

            // Using Extension Method FilterByPrice
            var filteredProducts = products.FilterByPrice(300);
            Console.WriteLine("\nProducts with Price >= 300 (Using Extension Method):");
            foreach (var product in filteredProducts)
            {
                Console.WriteLine(product);
            }

            Console.WriteLine("\nC# 3.0 Features Demonstrated Successfully!");
        }
    }
}
