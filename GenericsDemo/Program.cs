using System;
using System.Collections.Generic;

// Namespace encapsulating the application
namespace GenericsDemo
{

    /*
    It is designed to work with specific data 
    types explicitly defined in its implementation.
    If you need to work with different types, you must create
    multiple versions of the same class or use inheritance.
    
    It allows you to work with any type of data without
    having to write multiple version of the same class.
    It uses a generic type parameter that is
    defined with <T> and can be replaced by
    any type when the class is instantiated.
    Promotes code reusability and flexibility.

    */
    // Generic Class Example
    public class GenericRepository<T>: IRepository<T>
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

        private readonly List<T> _items = new();
        public readonly int ReadOnlyField; // readonly field
        public const int ConstField = 100; // const field 
        public readonly DateTime ReadOnlyDate; // readonly field for a reference type
        
        private readonly string _connectionString;
        private const string TableName = "Entities";
        
        /*
        CS8618: Non-nullable field '_connectionString' must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring the field as nullable.
        */
        public GenericRepository(string connectionString){
            _connectionString = connectionString;
        }

                // interface System.Collections.Generic.IEnumerable<out T>
        // Get all items
        public IEnumerable<T> GetAll()
        {
            Console.WriteLine($"Fetching all records from table {TableName} using {_connectionString}");
            return _items;
        }

        public T GetById(int id)
        {
            Console.WriteLine($"Fetching record with ID {id} from table {TableName}");
            // throw new NotImplementedException();
            return default(T);
        }
        // Add item to the repository
        // Change T for W
        public void Add(T entity)
        {
            Console.WriteLine($"Adding new record to table {TableName}");
            _items.Add(entity);
        }

        // Generic method to find an item by predicate
        //delegate bool System.Predicate<in T>(T obj)
        public T Find(Predicate<T> match)
        {
            return _items.Find(match);
        }

        public void Update(T entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }
    }

    // Generic Interface Example
    public interface IComparableEntity<T>
    {
        bool Compare(T other);
    }
    
    public interface IRepository<T>{
        IEnumerable<T> GetAll();
        T GetById(int id);
        void Add(T entity);
        void Update(T entity);
        void Delete(int id);
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

    public class ReadOnlyExample{
        public readonly int ReadOnlyField;
        public const int ConstField = 100;
        public readonly DateTime ReadOnlyDate;

        public ReadOnlyExample(int initialValue){
            ReadOnlyField = initialValue;
            ReadOnlyDate = DateTime.Now;
        }
        public void DisplayValues(){
            Console.WriteLine($"ReadOnlyField: {ReadOnlyField}");
            Console.WriteLine($"ConstField: {ConstField}");
            Console.WriteLine($"ReadOnlyDate: {ReadOnlyDate}");
        }
    }

    // Program demonstrating the use of generics
    class Program
    {
        static void Main(string[] args)
        {
            string connectionString = "Server=myServer;Database=myDb;User=myUser;Password=myPassword;";

            var example = new ReadOnlyExample(42);

            example.DisplayValues();
            
            //CS0191: A readonly field cannot be assigned to 
            //(except in a constructor or init-only setter of the type 
            //in which the field is defined or a variable initializer)
            // example.ReadOnlyField = 50;
            
            // CS0131: The left-hand side of an assignment must be a variable, property or indexer

            // ReadOnlyExample.ConstField = 200;
             
            /*
            However, the internal properties of an object referenced by 
            readonly field can change:
            ReadOnlyDate is struct (immutable), so it has no internal properties
            that can be modified. 
            */ 
            /*
            CS0191: A readonly field cannot be assigned to (except in a constructor or init-only setter of the type in which the field is defined or a variable initializer)
            */
            // example.ReadOnlyDate = DateTime.Now;
            example.ReadOnlyDate.AddHours(1);

            // Using GenericRepository with integers
            var intRepo = new GenericRepository<int>(connectionString);

            intRepo.Add(1);
            intRepo.Add(2);
            intRepo.Add(3);

            Console.WriteLine("Integer Repository:");
            foreach (var item in intRepo.GetAll())
            {
                Console.WriteLine(item);
            }

            // Using GenericRepository with custom type (Product)
            var productRepo = new GenericRepository<Product>(connectionString);
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
