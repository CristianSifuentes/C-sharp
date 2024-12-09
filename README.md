# C-sharp
C# (pronounced "C-sharp") is a modern, object-oriented programming language developed by Microsoft, designed to run on the .NET platform. It combines the power of C++ with the simplicity of Visual Basic, making it versatile for a wide range of applications, including web, desktop, mobile, gaming, and cloud services.

## Table of Contents
1. [C# Versions and Features](#c-versions-and-features)
2. [Explanation of Features](#explanation-of-features)
3. [Creating a Project with .NET CLI](#creating-a-project-with-net-cli)
4. [Additional Resources](#additional-resources)

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

1. **C# 1.0**: Introduced basic OOP features like classes, methods, and inheritance.
2. **C# 2.0**: Introduced generics for reusable data structures.
3. **C# 3.0**: Added LINQ and lambda expressions for querying data collections.
4. **C# 4.0**: Added support for optional parameters and dynamic typing.
5. **C# 5.0**: Simplified asynchronous programming with `async` and `await`.
6. **C# 6.0**: Improved code readability with expression-bodied members and string interpolation.
7. **C# 7.0-7.3**: Introduced tuples, pattern matching, and `in` parameters.
8. **C# 8.0**: Introduced nullable reference types and async streams.
9. **C# 9.0**: Introduced records and init-only properties for immutable data.
10. **C# 10.0**: Simplified namespaces with file-scoped syntax.
11. **C# 11.0**: Added raw string literals for cleaner multiline strings.
12. **C# 12.0**: Introduced primary constructors and collection expressions.

---

## Creating a Project with .NET CLI

Follow these steps to create a solution and add a console project in Visual Studio Code using the .NET CLI.

---

### 1. Install the .NET SDK
Verify if the .NET SDK is installed by running the command:

```bash 
dotnet --version
```
If the command outputs a version number, the SDK is installed. If not, download and install it from the official .NET website.

---

### 2. Create a Solution (.sln)
1. Open your terminal and navigate to the directory where you want to create the solution.
2. Run the command to create a new solution:

     ```bash 
     dotnet new sln -n YourSolutionName
     ```

---

### 3. Create a Console Project
1. Navigate to the solution directory or a subdirectory.
2. Run the command to create a console project:

     ```bash 
     dotnet new console -n YourProjectName
     ```

     This will generate a folder named after your project, containing the necessary files.

---

### 4. Add the Project to the Solution
1. Return to the root of your solution (where the `.sln` file is located).
2. Use the command to add the console project to the solution:

     ```bash 
     dotnet sln add YourProjectName/YourProjectName.csproj
     ```

---

### 5. Open the Project in Visual Studio Code
1. Open the project in Visual Studio Code using the command:

     ```bash 
     code .
     ```

2. Ensure that the **C# Extension Pack** is installed in Visual Studio Code to enable IntelliSense and additional features.

---

### 6. Restore and Run the Project
1. Restore the required packages by running the command:

     ```bash 
     dotnet restore
     ```

2. Run the project to test using the command:

     ```bash 
     dotnet run --project YourProjectName
     ```

---

## Example Code
Here’s a simple example of C# code to get started:
```csharp
using System;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Hello, world from C#!");
    }
}
