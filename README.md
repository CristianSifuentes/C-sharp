# C-sharp
C# (pronounced "C-sharp") is a modern, object-oriented programming language developed by Microsoft, designed to run on the .NET platform. It combines the power of C++ with the simplicity of Visual Basic, making it versatile for a wide range of applications, including web, desktop, mobile, gaming, and cloud services.

### Versions and Features of C#

|Version	| Release |	Key Features |
| --- | --- | --- |
| C# 1.0 |	2002 |	Core object-oriented features (classes, inheritance, interfaces). |
| C# 2.0 |	2005 |	Generics, anonymous methods, nullable types, iterators (yield). |
| C# 3.0 |	2007 |	LINQ, extension methods, lambda expressions, automatic properties. |
| C# 4.0 |	2010 |	Dynamic binding (dynamic), named and optional parameters, covariance. |
| C# 5.0 |	2012 |	Asynchronous programming (async and await). |
| C# 6.0 |	2015 |	Expression-bodied members, interpolated strings, null-conditional operators (?.). |
| C# 7.0-7.3 |	2017-2018 |	Tuples, pattern matching, local functions, ref returns, in parameters. |
| C# 8.0 |	2019 |	Nullable reference types, async streams, default interface methods. |
| C# 9.0 |	2020 |	Records, init-only properties, top-level statements, improved pattern matching. |
| C# 10.0 |	2021 |	Global usings, file-scoped namespaces, enhanced interpolated strings. |
| C# 11.0 |	2022 |	Raw string literals, list patterns, generic attributes, and improvements to spans. |
| C# 12.0 |	2023  |	Primary Constructors in No-Record Classes and Structures, Collection Expressions, Inline Arrays, Using Aliases for Any Type, Interceptors (Experimental). |

### Explanation of Features

1. **C# 1.0**: Basic OOP features like classes, methods, and strings.
1. **C# 2.0**: Introduces generics for reusable data structures.
1. **C# 3.0**: LINQ and lambda expressions for querying data collections.
1. **C# 4.0**: Optional parameters for methods.
1. **C# 5.0**: Asynchronous programming with async and await.
1. **C# 6.0**: Expression-bodied members and string interpolation.
1. **C# 7.0/7.3**: Tuples, pattern matching, and in parameters.
1. **C# 8.0**: Async streams and switch expressions.
1. **C# 9.0**: Init-only properties and record types.
1. **C# 10.0**: Global usings and file-scoped namespaces.
1. **C# 11.0**: Raw string literals.
1. **C# 12.0**: Collection expressions and primary constructors.


### Creating Project

To create a solution and add a console project in Visual Studio Code using the .NET CLI, follow these steps:


1. Install .NET SDK

Before you begin, make sure you have the .NET SDK installed on your system. To verify, run:

```bash 
dotnet --version

```
If it is not installed, download it from [here](https://dotnet.microsoft.com/en-us/download)


2. Create a solution (.sln)

   1.  Open the terminal and navigate to the directory where you want to create the solution.

   1.  Create the solution using the command:
        ```bash 
         dotnet new sln -n YourSolutionName
        ```

       Example:

        ```bash 
        dotnet new sln -n MySolution
        ```


3. Create a console project

   1. Inside the solution directory or in a subdirectory, create the console project:

        ```bash 
            dotnet new console -n YourProjectName
        ```

      Example:

        ```bash 
        dotnet new console -n MyProjectConsole
        ```
        This will generate a folder with the project name, containing the initial files.


4. Add the project to the solution

   1. Go back to the root of your solution (where the `.sln` file is).

   1. Add the project to the solution with the command:

        ```bash 
        dotnet sln add ProjectPath

        ```
      Example:

        ```bash 
        dotnet sln add MyProjectConsole
        /MyProjectConsole.csproj

        ```

5. Open the project in Visual Studio Code

   1. Open the solution in Visual Studio Code:

        ```bash 
         code .
        ```

   1. Make sure you have the C# Extension Pack installed in Visual Studio Code to get IntelliSense and additional tools.


6. Restore and run the project

   1. Restore the necessary packages:

        ```bash 
        dotnet restore

        ```

   1. Run the project to test:

        ```bash 
        dotnet run --project ProjectPath


        ```

      Example

        ```bash 
        dotnet run --project MyProjectConsole

        ```


