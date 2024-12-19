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
- **Generics:** Enable type-safe data structures and methods.
- **Anonymous Methods:** Define inline unnamed functions.
- **Nullable Types:** Handle `null` for value types.
- **Iterators (`yield`):** Simplify collection iteration.



---

## C# 3.0 Features
- **LINQ:** Query data collections in a SQL-like manner.
- **Lambda Expressions:** Simplify the syntax for anonymous functions.
- **Extension Methods:** Add new methods to existing types without modifying them.
- **Automatic Properties:** Simplify property declarations.

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
     * Key Features
          * Reduces boilerplate code, making it more concise and readable.
          * Allows concise definitions for methods, properties, and constructors.
- **String Interpolation:** Enhance string formatting with embedded expressions. 
     * Key Features
          * Allows embedding expressions directly in string literals using `$"..."`.
          * Simplifies string formatting, improving clarity and maintainability.
- **Null-Conditional Operators (`?.`):** Simplify null checks in code. 
     * Key Features
          * Simplifies null checks by safely accessing members of an object without throwing a `NullReferenceException`.
          * Enhances safety and reduces repetitive null checks. 
- **Null-Coalescing Operator (`??`):** Provides a fallback value when a nullable expression evaluates to null.


---

## C# 7.0-7.3 Features
- **Tuples:** Return multiple values from methods with ease.
     * Key Features
        * Tuples provide a lightweight way to return multiple values from a method. 
- **Pattern Matching:** Simplify conditional checks with patterns.
     * Key Features
        * Simplifies type-checking and conditional logic with `is` and `switch`. 
- **Local Functions:** Define functions inside methods for better encapsulation.
     * Key Features
        * Functions declared within a method, useful for encapsulating logic.
- **`Ref` Returns and `In` Parameters:** Optimize memory usage in performance-critical code.
     * Key Features
        * Allows returning a reference to a variable or array element, enabling modifications to the original source.
        * Passes arguments by reference for performance without allowing modifications.
---

## C# 8.0 Features
- **Nullable Reference Types:** Improve null safety by distinguishing nullable and non-nullable types.
- **Async Streams:** Simplify asynchronous data streaming with `IAsyncEnumerable`.
- **Default Interface Methods:** Provide default implementations for interface methods.

---

## C# 9.0 Features
- **Records:** Simplify immutable data structures.
- **Init-Only Properties:** Set properties during initialization only.
- **Top-Level Statements:** Write minimal code for simple applications.
- **Improved Pattern Matching:** Enhance readability with logical patterns.

---

## C# 10.0 Features
- **Global Usings:** Simplify common `using` directives across the project.
- **File-Scoped Namespaces:** Reduce indentation for better readability.
- **Enhanced Interpolated Strings:** Improve string formatting capabilities.

---

## C# 11.0 Features
- **Raw String Literals:** Simplify multiline and escape-heavy strings.
- **List Patterns:** Match and deconstruct elements in lists easily.
- **Generic Attributes:** Support generic types in attributes.
- **Span Improvements:** Optimize memory and performance for `Span` and `Memory`.

---

## C# 12.0 Features
- **Primary Constructors:** Declare constructors directly in class headers.
- **Collection Expressions:** Simplify collection initialization and manipulation.
- **Inline Arrays:** Define array literals inline for better readability.
- **Type Aliases:** Provide meaningful names for complex types.
- **Interceptors (Experimental):** Add hooks for method execution control.

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
