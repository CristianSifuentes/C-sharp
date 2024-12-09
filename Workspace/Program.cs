
#region AbstractClassesNamespaces
// Namespace for shapes
namespace Geometry.Shapes{
    public abstract class Shape{
        // Abstract property 
        public abstract string Name { get; }
        // Abstract method
        public abstract double CalculateArea();
        //Non-abstract method
        public void ShowInfo(){
            Console.WriteLine($"I am a shape called {Name}");
        }
    }

    public class Circle : Shape
    {
        public double Radius { get; set; }
        public override string Name => "Circle";

        public override double CalculateArea()
        {
            return Math.PI * Radius * Radius;
        }
        public Circle(double radius){
            Radius = radius;
        }
    }
    public class Square : Shape
    {
        public double Side { get; set; }
        public override string Name => "Square";
        
        public Square(double side){
            Side = side;
        }

        public override double CalculateArea()
        {
            return Side * Side;
        }
    }
}
// Namespace for utilities
namespace Geometry.Utilities{
  public static class MathUtilities {
    public static double CalculateCircleCircumference(double radius){
        return 2 * Math.PI * radius;
    }
    public static double CalculateSquarePerimeter(double side){
        return 4 * side;
    }
  }
}
// Main Program
namespace GeometryApp
{
    using Geometry.Shapes;
    using Geometry.Utilities;

    class Program1
    {
        static void Main()
        {
            Shape circle = new Circle(5);
            Shape square = new Square(4);

            // Display shape info and areas
            circle.ShowInfo();
            Console.WriteLine($"Area: {circle.CalculateArea()}");
            Console.WriteLine($"Circumference: {MathUtilities.CalculateCircleCircumference(5)}");

            square.ShowInfo();
            Console.WriteLine($"Area: {square.CalculateArea()}");
            Console.WriteLine($"Perimeter: {MathUtilities.CalculateSquarePerimeter(4)}");
        }
    }
}
#endregion