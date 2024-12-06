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
        void Fly();
    }

    // Derived Class for Dog
    public class Dog : Animal
    {
        public Dog(string name) : base(name) { }

        // Overriding the Speak method
        public override void Speak()
        {
            Console.WriteLine($"{Name} barks.");
        }
    }

    // Derived Class for Bird implementing an interface
    public class Bird : Animal, IFlyable
    {
        public Bird(string name) : base(name) { }

        // Overriding the Speak method
        public override void Speak()
        {
            Console.WriteLine($"{Name} chirps.");
        }

        // Implementing the Fly method from IFlyable
        public void Fly()
        {
            Console.WriteLine($"{Name} is flying high in the sky!");
        }
    }

    // Another Interface for aquatic animals
    public interface ISwimmable
    {
        void Swim();
    }

    // Derived Class for Fish implementing an interface
    public class Fish : Animal, ISwimmable
    {
        public Fish(string name) : base(name) { }

        // Overriding the Speak method
        public override void Speak()
        {
            Console.WriteLine($"{Name} makes bubbling sounds.");
        }

        // Implementing the Swim method from ISwimmable
        public void Swim()
        {
            Console.WriteLine($"{Name} is swimming in the water.");
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

            // Polymorphism: Using base class reference to call overridden methods
            dog.Speak();
            bird.Speak();
            fish.Speak();

            // Interface usage
            IFlyable flyableBird = bird as IFlyable;
            ISwimmable swimmableFish = fish as ISwimmable;

            flyableBird?.Fly();
            swimmableFish?.Swim();

            // Demonstrating encapsulation with properties
            var strayDog = new Dog(null);
            Console.WriteLine($"Stray dog's name: {strayDog.Name}");

            // Final message
            Console.WriteLine("\nC# 1.0 Core Object-Oriented Features Demonstrated Successfully!");
        }
    }
}
