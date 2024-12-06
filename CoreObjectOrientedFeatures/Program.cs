using System;

// Namespace encapsulating the entire application
namespace CoreObjectOrientedFeatures
{
    // Abstract Class (base for all animals)
    public abstract class Animal
    {
        // Encapsulated field
        private string _name;

        // Property to access the encapsulated field
        public string Name
        {
            get => _name;
            set => _name = value ?? "Unknown"; // Default to "Unknown" if null
        }

        // Constructor to initialize the name
        public Animal(string name)
        {
            Name = name;
        }

                // Virtual method for polymorphism
        public virtual void Speak()
        {
            Console.WriteLine($"{Name} makes a sound.");
        }

    }

    // Interface defining behavior for flying animals
    public interface IFlyable
    {

    }

    // Derived Class for Dog
    public class Dog : Animal
    {
       public Dog(string name) : base(name) { }


    }

    // Derived Class for Bird implementing an interface
    public class Bird : Animal, IFlyable
    {
        public Bird(string name) : base(name)
        {
        }
    }

    // Another Interface for aquatic animals
    public interface ISwimmable
    {

    }

    // Derived Class for Fish implementing an interface
    public class Fish : Animal, ISwimmable
    {
        public Fish(string name) : base(name)
        {
        }
    }

    // Main Program
    class Program
    {
        static void Main(string[] args)
        {
               // Creating instances of different animals
            Animal dog = new Dog("Buddy");
            Animal bird = new Bird("Tweety");
            Animal fish = new Fish("Goldie");

        }
    }

}