using System;

namespace CSharp6FeaturesDemo
{
    // Class demonstrating Expression-Bodied Members and Interpolated Strings
    public class Product
    {
        // Automatic Properties
        public string Name { get; set; }
        public decimal Price { get; set; }

        // Constructor using Expression-Bodied Member
        public Product(string name, decimal price) => (Name, Price) = (name, price);

        // ToString overridden with Expression-Bodied Member
        public override string ToString() => $"Product: {Name}, Price: {Price:C}";
    }

    // Main Program
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("C# 6.0 Features Demo");

            // 1. Interpolated Strings
            var product = new Product("Laptop", 1500.00m);
            Console.WriteLine($"\nUsing Interpolated Strings: The {product.Name} costs {product.Price:C}.");

            // 2. Null-Conditional Operator
            Product nullProduct = null;

            // Safe access using null-conditional operator
            var productName = nullProduct?.Name;
            Console.WriteLine("\nUsing Null-Conditional Operator:");
            Console.WriteLine($"Product name: {productName ?? "No product available"}"); // Fallback using null-coalescing

            // Chaining null-conditional operators
            var upperCaseName = nullProduct?.Name?.ToUpper();
            Console.WriteLine($"Upper-case product name: {upperCaseName ?? "No product available"}");

            // 3. Expression-Bodied Members
            Console.WriteLine("\nUsing Expression-Bodied Members:");
            Console.WriteLine(product);

            Console.WriteLine("\nC# 6.0 Features Demonstrated Successfully!");
        }
    }
}
