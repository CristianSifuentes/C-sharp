# C-sharp
C# (pronounced "C-sharp") is a modern, object-oriented programming language developed by Microsoft, designed to run on the .NET platform. It combines the power of C++ with the simplicity of Visual Basic, making it versatile for a wide range of applications, including web, desktop, mobile, gaming, and cloud services.

## Table of Contents
1. [C# Versions and Features](#c-versions-and-features)
2. [Explanation of Features](#explanation-of-features)
    - [C# 1.0 Features](#c-10-features)
    - [C# 2.0 Features](#c-20-features)
    - [C# 3.0 Features](#c-30-features)
    - [C# 4.0 Features](#c-40-features)
    - [C# 5.0 Features](#c-50-features)
    - [C# 6.0 Features](#c-60-features)
    - [C# 7.0-7.3 Features](#c-70-73-features)
    - [C# 8.0 Features](#c-80-features)
    - [C# 9.0 Features](#c-90-features)
    - [C# 10.0 Features](#c-100-features)
    - [C# 11.0 Features](#c-110-features)
    - [C# 12.0 Features](#c-120-features)
3. [Creating a Project with .NET CLI](#creating-a-project-with-net-cli)
4. [Multi Entry Point](#multi-entry-point)
5. [Additional Resources](#additional-resources)

---

## C# Versions and Features

| **Version** | **Release** | **Key Features** |
| --- | --- | --- |
| **C# 1.0** | 2002 | Core object-oriented features (classes, inheritance, interfaces). |
| **C# 2.0** | 2005 | Generics, anonymous methods, nullable types, iterators (`yield`). |
| **C# 3.0** | 2007 | LINQ, extension methods, lambda expressions, automatic properties. |
| **C# 4.0** | 2010 | Dynamic binding (`dynamic`), named and optional parameters, covariance. |
| **C# 5.0** | 2012 | Asynchronous programming (`async` and `await`). |
| **C# 6.0** | 2015 | Expression-bodied members, interpolated strings, null-conditional operators (`?.`). |
| **C# 7.0-7.3** | 2017-2018 | Tuples, pattern matching, local functions, `ref` returns, `in` parameters. |
| **C# 8.0** | 2019 | Nullable reference types, async streams, default interface methods. |
| **C# 9.0** | 2020 | Records, init-only properties, top-level statements, improved pattern matching. |
| **C# 10.0** | 2021 | Global usings, file-scoped namespaces, enhanced interpolated strings. |
| **C# 11.0** | 2022 | Raw string literals, list patterns, generic attributes, span improvements. |
| **C# 12.0** | 2023 | Primary constructors, collection expressions, inline arrays, type aliases, interceptors (experimental). |

---

## Explanation of Features

1. **[C# 1.0](#c-10-features):** Introduced basic OOP features like classes, methods, and inheritance.
2. **[C# 2.0](#c-20-features):** Introduced generics for reusable data structures.
3. **[C# 3.0](#c-30-features):** Added LINQ and lambda expressions for querying data collections.
4. **[C# 4.0](#c-40-features):** Added support for optional parameters and dynamic typing.
5. **[C# 5.0](#c-50-features):** Simplified asynchronous programming with `async` and `await`.
6. **[C# 6.0](#c-60-features):** Improved code readability with expression-bodied members and string interpolation.
7. **[C# 7.0-7.3](#c-70-73-features):** Introduced tuples, pattern matching, and `in` parameters.
8. **[C# 8.0](#c-80-features):** Introduced nullable reference types and async streams.
9. **[C# 9.0](#c-90-features):** Introduced records and init-only properties for immutable data.
10. **[C# 10.0](#c-100-features):** Simplified namespaces with file-scoped syntax.
11. **[C# 11.0](#c-110-features):** Added raw string literals for cleaner multiline strings.
12. **[C# 12.0](#c-120-features):** Introduced primary constructors and collection expressions.

---

## C# 1.0 Features
Basic object-oriented programming (OOP) features were introduced, including:
- **Classes:** Define blueprints for objects. Classes are the fundamental building block in C#. They encapsulate data (fields) and behavior (methods) into a single entry.
  *  Characteristics:
     * Support for encapsulation through access modifiers (public, private, protected, etc).
     * Can contain fields, methods, constructors, and properties.
     * Enable the creation of objects (instances).
- **Inheritance:** Allow objects to inherit properties from other classes. Enables a class to inherit members (fields and methods) from another class, promoting code reuse and creating a hierarchical structure.
     * Key Features:
          * A class can inherit from one base class (single inheritance).
          * Use the `: base-class-name` syntax.
          * Override base class methods using the `virtual` and `override` keywords.
- **Interfaces:** Define contracts for classes to implement. They allow multiple types to share a common API without requiring inheritance.
    * Declared using the `interface` keyword.
    * Can only contain method signatures, properties, events, or indexers (no implementation).
    * A class can implement multiple interfaces.
- **Polymorphism** Allows objects to be treated as instances of their base type, enabling dynamic behavior at runtime.
     * Key Features:
          * Achieved through method overriding (`virtual` and `override` keywords).
          * Enables late binding using base class references to derived objects.
- **Encapsulation** Encapsulation hides the internal state of an object and allows controlled access through methods or properties.
     * Key Features: 
          * Access modifiers (`public`, `private`, `protected`, `internal`).
          * Use properties to control field access.
- **Why It Was Significant?** C# 1.0 laid the foundation for what would become one of the most versatile programming languages
     * It offered a clean and modern syntax, ideal for enterprise and web applications.
     * It integrated seamlessly with the .NET Framework, enabling rapid development.
     * Its object-oriented design principles made it intuitive and powerful for developers transitioning from C++ or Java.  
- **Limitations in C# 1.0** While robust, C# 1.0 lacked some features developers rely on today:
     * No generics (introduced in C# 2.0).
     * Limited support for anonymous methods and delegates.
     * Basic error handling without more advanced features like `try-catch-finally`

- **Key Takeaways** C# 1.0 introduced the core object-oriented features that define modern programming: encapsulation, inheritance, polymorphism, and abstraction (via interfaces). These concepts remain fundamental to mastering C# and serve as the building blocks for all advanced language features introduced in subsequent versions.

## C# 2.0 Features
Introduced Generics, enabling reusable classes and methods for different data types. Also added iterators with `yield` 
- **Characteristics:**
     * Focused on reusability and efficiency.
     * Compatible with .NET Framework 2.0
- **Generics:** Enable type-safe data structures and methods. 
     * **Program.cs:** 
          ```csharp
               using System;
               using System.Collections.Generic;

               // Namespace encapsulating the application
               namespace GenericsDemo
               {
                    // Generic Class Example
                    public class GenericRepository<T>
                    {
                         private readonly List<T> _items = new();

                         // Add item to the repository
                         public void Add(T item)
                         {
                              _items.Add(item);
                         }

                         // Get all items
                         public IEnumerable<T> GetAll()
                         {
                              return _items;
                         }

                         // Generic method to find an item by predicate
                         public T Find(Predicate<T> match)
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

          ```  
     * **Generic Class:** 
          * `GenericRepository<T>` is a generic class that works with any type `T.
          * Provides reusable methods like `Add`, `GetAll`, and `Find`.
     * **Generic Methods:**
          * The `Find` method in `GenericRepository<T>` uses a `Predicate<T>` delegate to filter items. 
     * **Generic Interfaces:**
          * `IComparableEntity<T>` defines a contract for comparing two objects of type `T`.
          * Implemented in the `Product` class to compare prices between products.
     * **Custom Generic Type:**
          * `Product` demonstrates the use of a generic interface (`IComparableEntity<T>`) to compare objects of its own type.
     * **Reusability:**
          * The GenericRepository<T> can work with any type, making it highly reusable across different scenarios.
     * **Why Generics Are Important:**
         * **Type Safety:** Prevents runtime errors by ensuring type correctness at compile time.
         * **Performance:** Avoid boxing and unboxing for value types.
         * **Reusability:** Write once, reuse across different types.
- **Anonymous Methods:** Define inline unnamed functions.
- **Nullable Types:** Handle `null` for value types.
- **Iterators (`yield`):** Simplify collection iteration.
- **Why It Was Significant:**
     * Made generic programming robust and type-safe.
     * Enhanced support for collections and algorithms.
- **Limitations:**
     * Generics were limited in runtime reflection capabilities.
     * No generic support for primitive types in runtime.
- **Takeaways:**
     * Laid the foundation for LINQ and advanced data handling in the future versions.

---

## C# 3.0 Features
- **Characteristics:**
      * Focused on `data manipulation and expressive code`.  
      * Compatible with .NET Framework 3.0.
- **Program.cs:**

     ```csharp
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
     ```

- **LINQ:** Query data collections in a SQL-like manner.
- **Lambda Expressions:** Simplify the syntax for anonymous functions.
- **Extension Methods:** Add new methods to existing types without modifying them.
- **Automatic Properties:** Simplify property declarations.
- **Why It Was Significant:**
      * Unified querying across different data sources (SQL, XML, etc.).
      * Improved code readability and consistency.
- **Limitations:**
      * LINQ queries can have performance overhead.
      * Complex scenarios required manual optimization.
- **Takeaways:**
      * Established LINQ as the standard for querying and data handling.
---

## C# 4.0 Features
- **Dynamic Binding (`dynamic`):** Allow dynamic types for runtime flexibility.
- **Named and Optional Parameters:** Simplify method calls with optional arguments.
- **Covariance and Contravariance:** Enhance type safety in generics and delegates.

---

## C# 5.0 Features
- **Async and Await:** Simplify asynchronous programming with cleaner syntax.
     * Key Features:
          * `async`: Marks a method as asynchronous.
          * `await`: Suspends the execution of the method until the awaited task is complete, without blocking the main thread.
- **Task-Based Asynchronous Pattern (TAP):** 
     * Key Features:
          * The program users `Task` and `Task<T>` to represent asynchronous operations.
          * Asynchronous methods return a `Task` or `Task<T>`
- **Sequential vs Parallel Execution:** 
     * Sequential execution (using `Result`) blocks the main thread until a task completes.
     * Parallel execution (using `await`) allows other tasks to run concurrently without blocking
- **Asynchronous Methods Returning Values:**
     * Demonstrates using Task<T> to return a computed value asynchronously.
- **Real-World Scenarios:**
     * Fetching data for multiple URLs.
     * Offloading CPU-bound task (e.g., calculating factorials) to background threads.
- **Caller Information Attributes:** Provide information about the caller to methods.
- **Why These Features Are Important?** 
     * Improved Responsiveness:
          * Async/await enables non-blocking operations, making applications more responsive.
     * Simplified Asynchronous Code:
          * Async/await eliminates the need for complex callback structures, making asynchronous programming more intuitive.
     * Real-World Usability:
          * Ideal for I/O-bound tasks (e.g., web requests, file I/O) and CPU-bound tasks offloaded to background threads.
---

## C# 6.0 Features
- **Expression-Bodied Members:** Simplify methods and properties. 
     * Reduces boilerplate code, making it more concise and readable.
     * Allows concise definitions for methods, properties, and constructors.
- **String Interpolation:** Enhance string formatting with embedded expressions. 
     * Allows embedding expressions directly in string literals using `$"..."`.
     * Simplifies string formatting, improving clarity and maintainability.
- **Null-Conditional Operators (`?.`):** Simplify null checks in code. 
     * Simplifies null checks by safely accessing members of an object without throwing a `NullReferenceException`.
     * Enhances safety and reduces repetitive null checks. 
- **Null-Coalescing Operator (`??`):** Provides a fallback value when a nullable expression evaluates to null.


---

## C# 7.0-7.3 Features
- **Tuples:** Return multiple values from methods with ease.
     * Tuples provide a lightweight way to return multiple values from a method. 
- **Pattern Matching:** Simplify conditional checks with patterns.
     * Simplifies type-checking and conditional logic with `is` and `switch`. 
- **Local Functions:** Define functions inside methods for better encapsulation.
     * Functions declared within a method, useful for encapsulating logic.
- **`Ref` Returns and `In` Parameters:** Optimize memory usage in performance-critical code.
     * Allows returning a reference to a variable or array element, enabling modifications to the original source.
     * Passes arguments by reference for performance without allowing modifications.
---

## C# 8.0 Features
- **Nullable Reference Types:** Improve null safety by distinguishing nullable and non-nullable types. 
     * Adds compile-time safety for null reference handling. 
     * Reduces potential bugs from unhandled null references.
- **Async Streams:** Simplify asynchronous data streaming with `IAsyncEnumerable`. 
     * Enables efficient handling of asynchronous data streams, useful for real-time applications, APIs, and data processing.
- **Default Interface Methods:** Provide default implementations for interface methods. 
     * Simplifies the evolution of interfaces by allowing default behavior, reducing breaking changes when adding new methods.

---

## C# 9.0 Features
- **Records:** Simplify immutable data structures. 
     * Ideal for creating immutable data models with concise syntax. 
     * Built-in equality checks simplify comparisons.
- **Init-Only Properties:** Set properties during initialization only. 
     * Enforces immutability after initialization, making code safer and easier to maintain.
- **Top-Level Statements:** Write minimal code for simple applications. 
     * Removes boilerplate for simpler scripts and educational use cases.
- **Improved Pattern Matching:** Enhance readability with logical patterns. 
     * Enables more expressive and powerful matching scenarios.


---

## C# 10.0 Features
- **Global Usings:** Simplify common `using` directives across the project. 
    * Reduces repetitive imports across files in a project, simplifying code maintenance.
- **File-Scoped Namespaces:** Reduce indentation for better readability. 
    * Improves code readability by reducing indentation and unnecessary braces.
- **Enhanced Interpolated Strings:** Improve string formatting capabilities. 
    * Adds powerful formatting options directly into string interpolation, making code cleaner and easier to write.

---

## C# 11.0 Features
- **Raw String Literals:** Simplify multiline and escape-heavy strings. 
    * Simplify working with multi-line text and preserve formatting, making code cleaner and easier to maintain.
- **List Patterns:** Match and deconstruct elements in lists easily. 
    * Enable expressive pattern matching for collections, improving readability and reducing boilerplate code.
- **Generic Attributes:** Support generic types in attributes. 
    * Enhance the flexibility of attributes, allowing them to adapt to different types dynamically
- **Span Improvements:** Optimize memory and performance for `Span` and `Memory`. 
    * Improve performance for string and array manipulation by avoiding unnecessary allocations.

---

## C# 12.0 Features
- **Primary Constructors:** Declare constructors directly in class headers. 
    * Simplifies code by integrating constructor parameters into the class or struct declaration.
     ```csharp 
          public class Person(string name, int age) {
          public string Name { get; } = name;
          public int Age { get; } = age;
          }
          var person = new Person("Alice", 30);
          Console.WriteLine(person.Name); // Output: Alice
     ```
- **Collection Expressions:** Simplify collection initialization and manipulation. 
    * Enhances readability and reduces boilerplate when initializing collections.
     ```csharp 
          var numbers = [1, 2, 3, 4];
          Console.WriteLine(numbers[2]); // Output: 3
     ```
- **Inline Arrays:** Define array literals inline for better readability. 
    * Improves performance and simplifies the definition of fixed-size data structures.
     ```csharp 
          int[] array = [1, 2, 3, 4];
     ```    
- **Type Aliases:** Provide meaningful names for complex types. 
    * Improves code clarity by providing custom, meaningful names for complex types.
     ```csharp 
          using MyString = System.String;

          MyString message = "Hello, world!";
          Console.WriteLine(message);
     ```    
- **Interceptors (Experimental):** Add hooks for method execution control. 
    * Enables advanced scenarios like logging, validation, or security checks dynamically.
    * Allows developers to intercept and modify method behavior (currently experimental and subject to change).
- **Why It Was Significant?:** 
    * Simplified object initialization and collection handling.
    * Reduced boilerplate for data-centric applications.
     ```csharp 
          var matrix = [[1, 2], [3, 4], [5, 6]];
          foreach (var row in matrix) {
          Console.WriteLine(string.Join(", ", row));
          }
     ```   
- **Limitations:** Benefits mostly data-heavy applications.
    * Experimental features may undergo changes in future updates.
    * Experimental features like interceptors are not finalized and may change in future versions.
- **Key Takeaways:** Ideal for modern, clean, and scalable architectures.
    * Focused on simplifying object-oriented and collection-based programming.
    * Focused on improving the ease of working with collections and simplifying object-oriented patterns.


---

## Creating a Project with .NET CLI

To create a new C# project using the .NET CLI, follow these steps:

1. **Install the .NET SDK**:
   Ensure you have the .NET SDK installed. You can download it from [Microsoft's official site](https://dotnet.microsoft.com/).

2. **Create a Solution (.sln)**:
   Open your terminal and navigate to the directory where you want to create the solution:
   ```bash
   dotnet new sln -n MySln
   ```
   Replace `MySln` with the desired name of your solution.

3. **Create a New Project**:
   Run the following command to create a new console application:
   ```bash
   dotnet new console -n MyApp
   ```
   Replace `MyApp` with the desired name of your project.

4. **Add the Project to the Solution**:
   Return to the root of your solution (where the `.sln` file is located).
   ```bash
   dotnet sln add MyApp/MyApp.csproj
   ```
   Or
   ```bash
    
   dotnet sln MySln.sln add MyApp/MyApp.csproj
   ```
5. **Open the Project in Visual Studio Code**:
   Open the project in Visual Studio Code using the next command.
   ```bash
    code .
   ```
   Ensure that the **C# Extension Pack** is installed in Visual Studio Code to enable IntelliSense and additional features.

6. **Restore and Run the Project**:
   Restore the required packages by running the next command:
   ```bash
     dotnet restore
   ```
   Run the project to test using the next command:
   ```bash
     dotnet run --project YourProjectName
   ```
   This will execute the `Program.cs` file and display the output.

---


## Multi Entry Point

C# projects traditionally have a single entry point defined in the `Main` method. However, it's possible to create multiple entry points and choose which one to execute. Here's how:

1. **Define Multiple `Main` Methods**:
   Create different classes with their own `Main` method:
   ```csharp
   class Program1
   {
       public static void Main(string[] args)
       {
           Console.WriteLine("This is Program 1");
       }
   }

   class Program2
   {
       public static void Main(string[] args)
       {
           Console.WriteLine("This is Program 2");
       }
   }
   ```

2. **Specify the Entry Point**:
   Use the `dotnet run` command with the `--launch-profile` option or modify the `launchSettings.json` file in the `Properties` folder to set the desired entry point.

   Alternatively, compile the project using the `csc` (C# Compiler) command and specify the desired class:
   ```bash
   csc Program1.cs
   ```

---


## Additional Resources

- [Official C# Documentation](https://learn.microsoft.com/en-us/dotnet/csharp/)
- [C# Programming Guide](https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/)
- [C# Language Specification](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/)
- [Roslyn GitHub Repository](https://github.com/dotnet/roslyn)
