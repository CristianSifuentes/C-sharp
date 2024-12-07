namespace CSharp10FeaturesDemo;

// Main Program
class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("C# 10.0 Features Demo");

        // 1. Global Usings
        Console.WriteLine("\nUsing Global Usings:");
        var list = new List<string> { "C#", ".NET", "Features" };
        Console.WriteLine($"Items in the list: {string.Join(", ", list)}");

        // 2. Enhanced Interpolated Strings
        Console.WriteLine("\nUsing Enhanced Interpolated Strings:");
        var name = "Alice";
        var age = 30;
        var formattedMessage = $"Hello, {name.ToUpper()}! You are {age:D2} years old.";
        Console.WriteLine(formattedMessage);

        // Interpolated string alignment and formatting
        var products = new List<(string Name, decimal Price)>
        {
            ("Laptop", 1500.00m),
            ("Mouse", 25.50m),
            ("Keyboard", 75.99m)
        };
        Console.WriteLine("\nProduct List:");
        foreach (var (productName, productPrice) in products)
        {
            Console.WriteLine($"{productName,-10} | {productPrice,10:C}");
        }

        Console.WriteLine("\nC# 10.0 Features Demonstrated Successfully!");
    }
}
