using System;
using System.Collections.Generic;

// Namespace encapsulating the application
namespace GenericsDemo
{
    // Generic Class Example
    public class GenericRepository<W>
    {
        //class System.Collections.Generic.List<T>
        //readonly is used to declare fields that can only be
        //assigned when initializing them or in the class constructor.
        //Unlike const, it allows dynamic or calculated values at run time.

        /*
        readonly:
        You can assign its value in the declaration or in the
        constructor. Useful for values thar are calculated at run time.
        Its is immutable at all times and cannot be modified.
        
        const: 
        The value must be known at compile time.
        */

        private readonly List<W> _items = new();
        public readonly int ReadOnlyField; // readonly field
        public const int ConstField = 100; // Campo const
        public readonly DateTime ReadOnlyDate; // Campo readonly para un tipo de referencia
        


        // Add item to the repository
        // Change T for W
        public void Add(W item)
        {
            _items.Add(item);
        }

        // interface System.Collections.Generic.IEnumerable<out T>
        // Get all items
        public IEnumerable<W> GetAll()
        {
            return _items;
        }

        // Generic method to find an item by predicate
        //delegate bool System.Predicate<in T>(T obj)
        public W Find(Predicate<W> match)
        {
            return _items.Find(match);
        }
    }

    // Generic Interface Example
    public interface IComparableEntity<T>
    {
        bool Compare(T other);
    }

    // Example of implementing a generic interface
    public class Product : IComparableEntity<Product>
    {
        public string Name { get; set; }
        public decimal Price { get; set; }

        public bool Compare(Product other)
        {
            return other != null && Price == other.Price;
        }

        public override string ToString()
        {
            return $"Product: {Name}, Price: {Price:C}";
        }
    }

    // Program demonstrating the use of generics
    class Program
    {
        static void Main(string[] args)
        {
            // Using GenericRepository with integers
            var intRepo = new GenericRepository<int>();
            intRepo.Add(1);
            intRepo.Add(2);
            intRepo.Add(3);

            Console.WriteLine("Integer Repository:");
            foreach (var item in intRepo.GetAll())
            {
                Console.WriteLine(item);
            }

            // Using GenericRepository with custom type (Product)
            var productRepo = new GenericRepository<Product>();
            productRepo.Add(new Product { Name = "Laptop", Price = 1500.00m });
            productRepo.Add(new Product { Name = "Mouse", Price = 25.00m });

            Console.WriteLine("\nProduct Repository:");
            foreach (var product in productRepo.GetAll())
            {
                Console.WriteLine(product);
            }

            // Using the generic method to find a product
            var expensiveProduct = productRepo.Find(p => p.Price > 1000);
            Console.WriteLine($"\nExpensive Product: {expensiveProduct}");

            // Using the generic interface
            var laptop = new Product { Name = "Laptop", Price = 1500.00m };
            var anotherLaptop = new Product { Name = "Laptop Pro", Price = 1500.00m };
            Console.WriteLine($"\nAre the products equal in price? {laptop.Compare(anotherLaptop)}");

            Console.WriteLine("\nC# 2.0 Generics Demonstrated Successfully!");
        }
    }
}
