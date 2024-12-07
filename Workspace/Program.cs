
using System;
using System.Collections.Generic;
using MyProject;
using MyProject.Utilities;
using MyProject.Models;
using AliasModels = MyProject.Models;
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


