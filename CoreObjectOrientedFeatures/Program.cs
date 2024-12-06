using System;

// Namespace encapsulating the entire application
namespace CoreObjectOrientedFeatures
{
    // Abstract Class (base for all animals)
    public abstract class Animal
    {

    }

    // Interface defining behavior for flying animals
    public interface IFlyable
    {

    }

    // Derived Class for Dog
    public class Dog : Animal
    {

    }

    // Derived Class for Bird implementing an interface
    public class Bird : Animal, IFlyable
    {

    }

    // Another Interface for aquatic animals
    public interface ISwimmable
    {

    }

    // Derived Class for Fish implementing an interface
    public class Fish : Animal, ISwimmable
    {

    }

    // Main Program
    class Program
    {
        static void Main(string[] args)
        {

        }
    }

}