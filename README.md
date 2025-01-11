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
     * Defined using the `class` keyword.
     * Encapsulate data using fields and properties (`_name` and `Name`).
     * Support for encapsulation through access modifiers (`public`, `private`, `protected`, etc).
     * Can contain fields, methods, constructors, and properties.
     * Enable the creation of objects (instances).
- **Inheritance:** Allow objects to inherit properties from other classes. Enables a class to inherit members (fields and methods) from another class, promoting code reuse and creating a hierarchical structure.
     * A class can inherit from one base class (single inheritance).
     * Use the `: base-class-name` syntax.
     * Override base class methods using the `virtual` and `override` keywords.
       ```csharp 
          public class Animal {
          public virtual void Speak() {
               Console.WriteLine("Animal speaks");
          }
          }

          public class Dog : Animal {
          public override void Speak() {
               Console.WriteLine("Dog barks");
          }
          }
       ```  
- **Interfaces:** Define contracts for classes to implement. They allow multiple types to share a common API without requiring inheritance.
     * Declared using the `interface` keyword.
     * Can only contain method signatures, properties, events, or indexers (no implementation).
     * A class can implement multiple interfaces.
       ```csharp 
          public interface IAnimal {
          void Speak();
          }

          public class Dog : IAnimal {
          public void Speak() {
               Console.WriteLine("Dog speaks");
          }
          }
       ```     
- **Polymorphism** Allows objects to be treated as instances of their base type, enabling dynamic behavior at runtime.
     * Achieved through method overriding (`virtual` and `override` keywords).
     * Enables late binding using base class references to derived objects.
- **Encapsulation** Encapsulation hides the internal state of an object and allows controlled access through methods or properties.
     * Access modifiers (`public`, `private`, `protected`, `internal`).
     * Use properties to control field access.
- **Why It Was Significant?** C# 1.0 laid the foundation for what would become one of the most versatile programming languages
     * It offered a clean and modern syntax, ideal for enterprise and web applications.
     * It integrated seamlessly with the .NET Framework, enabling rapid development.
     * Its object-oriented design principles made it intuitive and powerful for developers transitioning from C++ or Java. 
     * Introduced a modern, type-safe, object-oriented programming paradigm for .NET.
     * Provided the foundation for the .NET framework with core features like assemblies and namespaces.
       ```csharp 
          Dog dog = new Dog();
          dog.Speak(); // Output: Dog barks
       ```  
- **Limitations in C# 1.0** While robust, C# 1.0 lacked some features developers rely on today:
     * No generics (introduced in C# 2.0).
     * No support for generics, making type safety in collections cumbersome.
     * Limited support for anonymous methods and delegates.
     * Basic error handling without more advanced features like `try-catch-finally`
     * Asynchronous programming required manual threading:
       ```csharp 
          // Without generics:
          ArrayList list = new ArrayList();
          list.Add(42);
          int value = (int)list[0]; // Explicit cast required
       ```  
- **Key Takeaways** C# 1.0 introduced the core object-oriented features that define modern programming: encapsulation, inheritance, polymorphism, and abstraction (via interfaces). These concepts remain fundamental to mastering C# and serve as the building blocks for all advanced language features introduced in subsequent versions.
     * Established the baseline for C# development, focusing on object-oriented principles.

## C# 2.0 Features
Introduced Generics, enabling reusable classes and methods for different data types. Also added iterators with `yield` 
- **Characteristics:**
     * Focused on reusability and efficiency.
     * Compatible with .NET Framework 2.0
- **Generics:** Enable type-safe data structures and methods.
     * **Generic Class:** A Generic Class allows you to define a class with placeholders for data types. This makes the class reusable and type-safe without having to rewrite it for different data types.
          * The class is a generic when that works with any type `T`.
          * Provides reusable methods.
            ```csharp 
               public class GenericClass<T> {
               public T Data { get; set; }

               public void Display() {
                    Console.WriteLine($"Data: {Data}");
               }
               }

               // Usage
               var intInstance = new GenericClass<int> { Data = 42 };
               intInstance.Display(); // Output: Data: 42

               var stringInstance = new GenericClass<string> { Data = "Hello" };
               stringInstance.Display(); // Output: Data: Hello

            ```             
     * **Generic Methods:** A Generic Method allows you to create a method that can operate on different data types without duplicating code. These methods are defined with type parameters.
          * Reusable methods that work with any data type `T`. 
            ```csharp 
               public class Utility {
               public static void PrintArray<T>(T[] array) {
                    foreach (var item in array) {
                         Console.WriteLine(item);
                    }
               }
               }

               // Usage
               Utility.PrintArray(new int[] { 1, 2, 3 }); // Output: 1 2 3
               Utility.PrintArray(new string[] { "A", "B", "C" }); // Output: A B C

            ``` 
     * **Generic Interfaces:** A Generic Interface defines an interface with type parameters, making it flexible for different implementations.
          * A Generic interface defines a contract for comparing two objects of type `T`.
          * Flexible contract for various implementations.
            ```csharp 
               public interface IRepository<T> {
               void Add(T item);
               T Get(int id);
               }

               public class Repository<T> : IRepository<T> {
               private readonly Dictionary<int, T> _data = new();

               public void Add(T item) {
                    _data[_data.Count + 1] = item;
               }

               public T Get(int id) {
                    return _data.ContainsKey(id) ? _data[id] : default;
               }
               }

               // Usage
               var intRepo = new Repository<int>();
               intRepo.Add(42);
               Console.WriteLine(intRepo.Get(1)); // Output: 42

               var stringRepo = new Repository<string>();
               stringRepo.Add("Hello");
               Console.WriteLine(stringRepo.Get(1)); // Output: Hello

            ``` 
     * **Custom Generic Type:** User-defined types like structs, delegates, or classes leveraging generics for type safety and reusability.
          * A Custom Generic Type is any user-defined type (class, struct, or delegate) that uses generics for flexibility and type safety.
             * **Custom Generic Delegate:**
               ```csharp 
                    public delegate T Operation<T>(T a, T b);

                    // Usage
                    Operation<int> add = (a, b) => a + b;
                    Console.WriteLine(add(3, 4)); // Output: 7

                    Operation<string> concatenate = (a, b) => a + b;
                    Console.WriteLine(concatenate("Hello", " World")); // Output: Hello World
               ``` 
             * **Custom Generic Struct:**
               ```csharp 
                    public struct KeyValuePair<K, V> {
                    public K Key { get; set; }
                    public V Value { get; set; }

                    public void Display() {
                         Console.WriteLine($"Key: {Key}, Value: {Value}");
                    }
                    }

                    // Usage
                    var pair = new KeyValuePair<int, string> { Key = 1, Value = "One" };
                    pair.Display(); // Output: Key: 1, Value: One

               ``` 
     * **Reusability:**
          * The GenericRepository<T> can work with any type, making it highly reusable across different scenarios.
     * **Why Generics Are Important:**
         * **Type Safety:** Prevents runtime errors by ensuring type correctness at compile time.
         * **Performance:** Avoid boxing and unboxing for value types.
         * **Reusability:** Write once, reuse across different types.
- **Anonymous Methods:** Anonymous methods are inline methods defined without a name, often used with delegates. They provide a way to create short, concise methods directly in the context where they are needed, reducing the need to define separate methods.
     * Define inline unnamed functions.
       ```csharp 
               Action<string> print = delegate (string message) {
               Console.WriteLine(message);
               };
               print("Hello");
       ```  
- **Nullable Types:** Nullable types allow value types (like `int`, `bool`, `DateTime`) to hold null values, enabling better handling of scenarios where "no value" is meaningful.
     * Handle `null` for value types.
       ```csharp 
               int? nullableInt = null;
               Console.WriteLine(nullableInt.HasValue); // Output: False
       ```  
- **Iterators (`yield`):** Iterators are methods that use the yield keyword to return a sequence of elements one at a time. They simplify the creation of custom enumerators.
     * Simplify collection iteration.
       ```csharp 
               public IEnumerable<int> GetNumbers() {
               for (int i = 1; i <= 5; i++) {
                    yield return i;
               }
               }
       ```  
- **Why It Was Significant:**
     * Made generic programming robust and type-safe.
     * Enhanced support for collections and algorithms.
     * Enhanced type safety and performance with generics:
       ```csharp 
          Dictionary<string, int> dictionary = new Dictionary<string, int> {
          { "One", 1 },
          { "Two", 2 }
          };
       ```  
- **Limitations:**
     * Generics were limited in runtime reflection capabilities.
     * No generic support for primitive types in runtime.
     * LINQ and advanced functional programming capabilities were not yet introduced.
- **Takeaways:**
     * Laid the foundation for LINQ and advanced data handling in the future versions.
     * Generics transformed type-safe data structures and algorithms.

---

## C# 3.0 Features
- **Characteristics:**
     * Focused on `data manipulation and expressive code`.  
     * Compatible with .NET Framework 3.0.
- **LINQ:** Is a powerful feature in C# that enables querying data from in-memory collections, databases, XML, and more using a unified, type-safe syntax. It integrates query capabilities directly into the language.
     * Query data collections in a SQL-like manner
       ```csharp 
          var numbers = new List<int> { 1, 2, 3, 4, 5 };
          var evens = from num in numbers
                    where num % 2 == 0
                    select num;
       ```  
- **Lambda Expressions:** Lambda expressions are concise, inline expressions or functions that can be passed as arguments to methods or used in LINQ queries. They are defined using the => (goes to) operator.
     * Simplify the syntax for anonymous functions.
       ```csharp 
          Func<int, int> square = x => x * x;
          Console.WriteLine(square(4)); // Output: 16
       ```  
- **Extension Methods:** Add new methods to existing types without modifying them.
     * Extension methods allow developers to add new methods to existing types (classes, structs, interfaces) without modifying the source code or creating a derived type.
       ```csharp 
          public static class StringExtensions {
          public static bool IsNullOrEmpty(this string value) {
               return string.IsNullOrEmpty(value);
          }
          }
          Console.WriteLine("".IsNullOrEmpty()); // Output: True
       ``` 
- **Automatic Properties:** Automatic properties simplify the declaration of properties by automatically generating a backing field.
     * Simplify property declarations.
- **Why It Was Significant:**
     * Unified querying across different data sources (SQL, XML, etc.).
     * Improved code readability and consistency.
     * LINQ unified data querying across in-memory collections, databases, and XML:
       ```csharp 
          var names = new[] { "Alice", "Bob", "Charlie" };
          var filtered = names.Where(name => name.StartsWith("A"));
       ``` 
- **Limitations:**
     * LINQ queries can have performance overhead.
     * Complex scenarios required manual optimization.
     * Asynchronous LINQ operations were not supported.
- **Takeaways:**
     * Established LINQ as the standard for querying and data handling.
     * Revolutionized data querying and manipulation within C#.

---

## C# 4.0 Features
- **Dynamic Binding (`dynamic`):** Allow dynamic types for runtime flexibility.
     * The `dynamic` keyword allows variables to bypass compile-time type checking, deferring it to runtime.
     * Useful for scenarios where the type is unknown until runtime, such as COM interop or working with dynamic languages like Python.
       ```csharp 
          dynamic obj = "Hello";
          Console.WriteLine(obj.Length); // Output: 5
       ```  

- **Named and Optional Parameters:** Simplify method calls with optional arguments.
     * Optional parameters allow specifying default values for method parameters.
     * Named parameters let you specify arguments by name instead of position, enhancing readability.
       ```csharp 
          void Print(string message, bool urgent = false) {
          Console.WriteLine(urgent ? "URGENT: " + message : message);
          }
          Print("Hello", urgent: true);
       ```  
- **Covariance and Contravariance:** Enhance type safety in generics and delegates.
     * Allows a method or interface to return a more derived type than specified in its declaration.
     * Introduced with the `out` keyword for generic type parameters in interfaces and delegates.
- **Why It Was Significant?:** 
     * Simplified interoperation with dynamic libraries (e.g., COM and Python).
- **Limitations:**
     * Overuse of `dynamic` could lead to runtime errors.
- **Key Takeaways:**
     * Enhanced flexibility for dynamic scenarios and simplified APIs.

---

## C# 5.0 Features
- **Async and Await:** Simplify asynchronous programming with cleaner syntax.
     * `async`: Marks a method as asynchronous.
     * `await`: Suspends the execution of the method until the awaited task is complete, without blocking the main thread.
       ```csharp 
          public async Task<int> GetDataAsync() {
          await Task.Delay(1000);
          return 42;
          }
       ```      
- **Task-Based Asynchronous Pattern (TAP):** 
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
- **Caller Information Attributes:**
     * Provide information about the caller to methods
       ```csharp 
          void Log(string message, [CallerMemberName] string memberName = "") {
          Console.WriteLine($"[{memberName}]: {message}");
          }
       ```   
- **Why These Features Are Important?** 
     * Improved Responsiveness:
          * Async/await enables non-blocking operations, making applications more responsive.
     * Simplified Asynchronous Code:
          * Async/await eliminates the need for complex callback structures, making asynchronous programming more intuitive.
     * Real-World Usability:
          * Ideal for I/O-bound tasks (e.g., web requests, file I/O) and CPU-bound tasks offloaded to background threads.
- **Why It Was Significant?:** 
     * Simplified asynchronous programming, making it more accessible and readable.
- **Limitations:**
     * Could lead to deadlocks if misused in certain synchronization contexts.
- **Key Takeaways:**
     * Set the standard for modern asynchronous programming.

---

## C# 6.0 Features
- **Expression-Bodied Members:** Simplify methods and properties. 
     * Reduces boilerplate code, making it more concise and readable.
     * Allows concise definitions for methods, properties, and constructors.
       ```csharp 
          public class Person {
          public string Name { get; set; }
          public string GetName() => $"Name: {Name}";
          }
       ``` 
- **String Interpolation:** Enhance string formatting with embedded expressions. 
     * Allows embedding expressions directly in string literals using `$"..."`.
     * Simplifies string formatting, improving clarity and maintainability.
       ```csharp 
          string name = "Alice";
          int age = 30;
          Console.WriteLine($"Name: {name}, Age: {age}");
       ``` 
- **Null-Conditional Operators (`?.`):** Simplify null checks in code. 
     * Simplifies null checks by safely accessing members of an object without throwing a `NullReferenceException`.
     * Enhances safety and reduces repetitive null checks. 
       ```csharp 
          string? message = null;
          int length = message?.Length ?? 0;
          Console.WriteLine(length); // Output: 0
       ``` 
- **Null-Coalescing Operator (`??`):** Provides a fallback value when a nullable expression evaluates to null.
     * Provides a fallback value when a nullable expression evaluates to null.
- **Static `using` Directives:** Provides a fallback value when a nullable expression evaluates to null.
       ```csharp 
          using static System.Console;
          WriteLine("Hello World!");
       ``` 
- **Why It Was Significant?:** 
     * Simplified and modernized syntax, reducing boilerplate code.
       ```csharp 
          string name = "Bob";
          Console.WriteLine($"Hello, {name}!"); // Output: Hello, Bob!
       ``` 
- **Limitations:**
     * Focused mainly on syntactic sugar; no significant performance improvements. 
- **Key Takeaways:**
     * Enhanced developer productivity and readability with concise syntax.

---

## C# 7.0-7.3 Features
- **Tuples:** Return multiple values from methods with ease.
     * Provides a quick way to handle multiple return values without defining custom types.
     * Tuples provide a lightweight way to return multiple values from a method. 
       ```csharp 
          (int x, int y) point = (3, 4);
          Console.WriteLine($"X: {point.x}, Y: {point.y}");
       ``` 
- **Pattern Matching:** Simplify conditional checks with patterns.
     * Simplifies conditional logic, making code more expressive and type-safe.
     * Simplifies type-checking and conditional logic with `is` and `switch`. 
       ```csharp 
          object obj = 42;
          if (obj is int number) {
          Console.WriteLine($"The number is {number}");
          }
       ``` 
- **Local Functions:** Define functions inside methods for better encapsulation.
     * Encapsulates functionality within a method for better readability and maintainability.
     * Functions declared within a method, useful for encapsulating logic.
       ```csharp 
          void PrintMessage(string message) {
          Console.WriteLine(message);
          }
          PrintMessage("Hello");

       ``` 
- **`Ref` Returns and `In` Parameters:** Optimize memory usage in performance-critical code.
     * Allows returning a reference to a variable or array element, enabling modifications to the original source.
     * Enables efficient updates to data structures without copying values.
     * Improves performance for large structures by avoiding unnecessary copying.
     * Passes arguments by reference for performance without allowing modifications.
       ```csharp 
          public ref int Find(int[] numbers, int value) {
          for (int i = 0; i < numbers.Length; i++) {
               if (numbers[i] == value) return ref numbers[i];
          }
          throw new Exception("Not found");
          }

       ``` 
- **Why It Was Significant?:** 
     * Introduced functional programming capabilities and advanced syntax for modern programming paradigms.
       ```csharp 
          var (x, y) = (10, 20);
          Console.WriteLine($"X: {x}, Y: {y}");
       ``` 
- **Limitations:**
     * Pattern matching was basic compared to other functional languages like F#.
- **Key Takeaways:**
     * Expanded the versatility of the language with functional programming elements.
---

## C# 8.0 Features
- **Nullable Reference Types:** Improve null safety by distinguishing nullable and non-nullable types. 
     * Adds compile-time safety for null reference handling. 
     * Introduced to make reference types nullable explicitly, reducing potential NullReferenceException.
     * Nullable reference types are denoted with a `?` (e.g., `string?`).
     * Non-nullable reference types must be initialized and cannot be assigned `null`.
     * Reduces potential bugs from unhandled null references.
       ```csharp 
          string? nullable = null;
          Console.WriteLine(nullable?.Length); // Safe null access
       ``` 
- **Async Streams:** Simplify asynchronous data streaming with `IAsyncEnumerable`.
     * Enables asynchronous iteration over a collection of data using `IAsyncEnumerable<T>`. 
     * Combines `async` and `yield` return to produce values asynchronously.
     * Enables efficient handling of asynchronous data streams, useful for real-time applications, APIs, and data processing.
       ```csharp 
          public async IAsyncEnumerable<int> GenerateNumbers() {
          for (int i = 1; i <= 5; i++) {
               await Task.Delay(100); // Simulate async work
               yield return i;
          }
          }
       ``` 
- **Default Interface Methods:** Provide default implementations for interface methods. 
     * Allows interfaces to define default implementations for methods, reducing the need for implementing boilerplate code in derived classes.
     * Simplifies the evolution of interfaces by allowing default behavior, reducing breaking changes when adding new methods.
       ```csharp 
          public interface ILogger {
          void Log(string message);
          void LogError(string error) => Console.WriteLine($"Error: {error}");
          }
       ``` 
- **Why It Was Significant?:** 
     * Addressed null safety comprehensively, making codebases more reliable:
       ```csharp 
          string? nullableName = null;
          Console.WriteLine(nullableName ?? "Unknown");
       ```
- **Limitations:**
     * Enabling nullable reference types required significant codebase changes.
- **Key Takeaways:**
     * Brought reliability and scalability to modern codebases with null safety and async streams.
---

## C# 9.0 Features
- **Records:** Simplify immutable data structures. 
     * Introduced for immutable data models.
     * Records provide built-in value equality, meaning two records are considered equal if their properties have the same values.
     * Supports `with` expressions for creating a modified copy of a record.
     * Ideal for creating immutable data models with concise syntax. 
     * Built-in equality checks simplify comparisons.
       ```csharp 
          public record Person(string Name, int Age);
          var person = new Person("Alice", 25);
          Console.WriteLine(person);
       ``` 
- **Init-Only Properties:** Set properties during initialization only. 
     * Allows properties to be initialized only during object creation or initialization.
     * Immutable after initialization, enhancing safety and clarity.
     * Enforces immutability after initialization, making code safer and easier to maintain.
       ```csharp 
          public class Person {
          public string Name { get; init; }
          }
          var person = new Person { Name = "Alice" };
       ``` 
- **Top-Level Statements:** Write minimal code for simple applications. 
     * Simplifies console applications by removing boilerplate Main method and class.
     * Useful for small programs or educational purposes.
     * Removes boilerplate for simpler scripts and educational use cases.
       ```csharp 
          Console.WriteLine("Hello World!");
       ``` 
- **Improved Pattern Matching:** Enhance readability with logical patterns. 
     * Adds support for positional patterns, property patterns, and combinatorial logic.
     * Enables more expressive and powerful matching scenarios.
       ```csharp 
          object obj = "Hello";
          if (obj is string s && s.Length > 3) {
          Console.WriteLine(s);
          }
       ``` 
- **Why It Was Significant?:** 
  * Simplified immutable data patterns with records:
       ```csharp 
          var alice = new Person("Alice", 30);
          Console.WriteLine(alice); // Output: Person { Name = Alice, Age = 30 }
       ```  
- **Limitations:**
  * Limited support for advanced functional paradigms compared to other languages like F#.
- **Key Takeaways:**
  * Advanced immutability and concise code structures for modern applications.
---

## C# 10.0 Features
- **Global Usings:** Simplify common `using` directives across the project. 
  * Introduced to reduce boilerplate by allowing common using directives to be shared across the entire project.
  * Reduces repetitive imports across files in a project, simplifying code maintenance.
     ```csharp 
          global using System;
          global using System.Collections.Generic;
     ```  
- **File-Scoped Namespaces:** Reduce indentation for better readability. 
  * Simplifies namespace declarations by removing the need for braces and indentation.
  * Improves code readability by reducing indentation and unnecessary braces.
     ```csharp 
          namespace MyApp;

          public class Program {
          public static void Main() {
               Console.WriteLine("Hello World!");
          }
          }
     ```  
- **Enhanced Interpolated Strings:** Improve string formatting capabilities. 
  * Enhances string formatting within interpolated strings, including alignment and formatting options.
  * Adds powerful formatting options directly into string interpolation, making code cleaner and easier to write.
     ```csharp 
          var value = 42;
          Console.WriteLine($"The value is {value:N}");
     ```  
- **Why It Was Significant?:** 
  * Improved project-wide consistency and code organization:
     ```csharp 
          // File-scoped namespaces reduce indentation clutter.
          namespace MyNamespace;

          public class MyClass {
          public void MyMethod() => Console.WriteLine("Hello");
          }
     ```    
- **Limitations:**
  * Focused mainly on usability and organization; fewer runtime performance enhancements.
- **Key Takeaways:**
  * Simplified project configuration and improved developer experienced for larger teams.
---

## C# 11.0 Features
- **Raw String Literals:** Simplify multiline and escape-heavy strings. 
  * Allow multi-line strings that preserve formatting and whitespace exactly as written.
  * Triple quotes `(""")` denote raw string literals.
  * Simplify working with multi-line text and preserve formatting, making code cleaner and easier to maintain.
     ```csharp 
          string rawString = """
          This is a raw string.
          It preserves indentation and formatting.
          """;
          Console.WriteLine(rawString);
     ```
- **List Patterns:** Match and deconstruct elements in lists easily. 
  * Enable matching and deconstructing arrays or lists into specific patterns.
  * Supports the `..` operator for slicing and `_` for ignoring elements.
  * Enable expressive pattern matching for collections, improving readability and reducing boilerplate code.
     ```csharp 
          int[] numbers = { 1, 2, 3, 4, 5 };
          if (numbers is [1, 2, .., 5]) {
          Console.WriteLine("Pattern matched!");
          }
     ```
- **Generic Attributes:** Support generic types in attributes. 
   * Allow attribute to take generic types, making them more flexible and reusable.
   * Enhance the flexibility of attributes, allowing them to adapt to different types dynamically.
     ```csharp 
          public class MyAttribute<T> : Attribute { }

          [MyAttribute<int>]
          public class MyClass { }
     ```
- **Span Improvements:** Optimize memory and performance for `Span` and `Memory`. 
   * `Span<T>` and `ReadOnlySpan<T>` enable efficient slicing of strings or arrays without allocations.
   * Improve performance for string and array manipulation by avoiding unnecessary allocations.
     ```csharp 
          Span<int> span = stackalloc int[] { 1, 2, 3 };
          foreach (var num in span) {
          Console.WriteLine(num);
          }
     ```
- **Why It Was Significant?:** 
   * Made working with strings, patterns, and generics more expressive and concise:
     ```csharp 
          string json = """
          {
          "Name": "Alice",
          "Age": 30
          }
          """;
          Console.WriteLine(json);
     ```   
- **Limitations:**
   * Generic attributes still have limited scenarios compared to runtime-based attribute logic.
- **Key Takeaways:**
   * Enhanced the expressiveness of the language, particularly for raw strings and pattern matching.
---

## C# 12.0 Features
- **Primary Constructors in Non-Record Classes and Structures:** Declare constructors directly in class headers. 
    * Allows defining constructor parameters directly in the class or struct declaration.
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
    * Provides a clean and concise syntax for initializing arrays and collections.
    * Enhances readability and reduces boilerplate when initializing collections.
      ```csharp 
          var numbers = [1, 2, 3, 4];
          Console.WriteLine(numbers[2]); // Output: 3
      ```
- **Inline Arrays:** Define array literals inline for better readability. 
    * Allows defining a fixed-size array directly within a structure.
    * Improves performance and simplifies the definition of fixed-size data structures.
      ```csharp 
          int[] array = [1, 2, 3, 4];
      ```    
- **Using Aliases for Any Type:** Provide meaningful names for complex types. 
    * Extends using directives to alias any type, improving code clarity and maintainability.
    * Improves code clarity by providing custom, meaningful names for complex types.
      ```csharp 
          using MyString = System.String;

          MyString message = "Hello, world!";
          Console.WriteLine(message);
      ```    
- **Interceptors (Experimental):** Add hooks for method execution control. 
    * Enables advanced scenarios like logging, validation, or security checks dynamically.
    * Allows dynamically intercepting method calls to modify their behavior.
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
