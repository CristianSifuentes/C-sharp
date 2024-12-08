
using System;
using System.Collections.Generic;
using MyProject;
using MyProject.Utilities;
using MyProject.Models;
using AliasModels = MyProject.Models;

#region Namespaces

//???
//A using clause must precede all other elements defined in the namespace except extern alias declarations

//namespace is a way to organize and group classes, 
//interfaces, structures, enumerations, delegates, and 
//other code elements.

//if two developers create classes with the same name in different parts
//using namespaces correctly you can avoid conflicts

//Using namespaces you can reuse code in different parts

//Namespaces can be nested which allows for a hirarchical structure
//from an application or library

//namespaces System, System.Collections.Generic, System.IO, System.Threading etc

//Makes it easy to breake code into manageable chunks
//Ideal for large projects where organization is key.
//Allows multiple developers to work on the same project without name conflicts
//Promotes the sharing of code in different projects

class Program{
    static void Main(){
        MyClass obj = new MyClass();
        obj.ShowMessage();
        Tools tool = new Tools();
        tool.Hi();
        User user = new User();
        user.Name = "Artur";
        Product product = new Product();
        product.Name = "Apple";

        Console.WriteLine(product.Name);
        Console.WriteLine(user.Name);

        List<string> names = new List<string>{
            "Ana", "Marie", "Sofia"
        };
        foreach (var name in names){
            Console.WriteLine(name);
        }
        
        AliasModels.Product objAliasProduct = new AliasModels.Product();
        AliasModels.User objAliasUser = new AliasModels.User();
        objAliasProduct.Name = "PC";
        objAliasUser.Name = "Jose";
        Console.WriteLine(objAliasProduct.Name);
        Console.WriteLine(objAliasUser.Name);

        Circle circle = new Circle("Color");
        circle.Name = "Circle";
        circle.CalculateArea();
        Console.WriteLine(circle.Name);
        Console.WriteLine(circle.CalculateArea());


    }
}

namespace MyProject{
    namespace Utilities{
        class Tools{
            public void Hi(){
               Console.WriteLine("Hi from Utilities.Tools");
 
            }
        }
    }
}



namespace MyProject
{
    class MyClass
    {
        public void ShowMessage(){
            Console.WriteLine("Hi fom MyClass in MyProject");
        }
    }
}



#endregion

#region AbstractClasses
//It cannot be instantiated directly
//You can only use it as a base class
//Code reuse: You can define common behavior in the abstract class and extend it in derived classes
//Consistency: Ensures that all derived classes implement certain methods and properties.
//Flexibility: It allow you to define a base structure without specifying all the details, leaving room for customization int derived classes


abstract class Figure{
    //Abstract properties have no body
    public abstract string Name { get; set;} // Abstract property
    //Non-abstract properties have body
    public int Sides { get; set; } = 4; // Non-abstract property
    // Abstract methods have no implementation and must be implemented
    // by derived classes
    public abstract double CalculateArea();
    // Non-abstract methods have implementation and can be inherited directly
    public void ShowMessage(){
        Console.WriteLine("I am a figure");
    }
    //Abstract class 
    public Figure(string color){
        color = color;
        //error CS7036: There is no argument given that corresponds to the required parameter 'color' of 'Figure.Figure(string)'
    }
}

class Circle: Figure {
     //Derived classes must provide implementations for all abstract members
     public override global::System.String Name { 
        get => throw new System.NotImplementedException(); 
        set => throw new System.NotImplementedException(); 
    }
     
     //error CS0534: 'Circle' does not implement inherit ted abstract member 'Figure.Name.get'
     public override global::System.Double CalculateArea() => throw new System.NotImplementedException();
     //Although it cannot be instantiated, it can have a constructor that will be called by derived classes.   
     public Circle(string color): base(color) { }
 }

//You can´t do this 
//Figure myFigure = new Figure(); //Error
#endregion
