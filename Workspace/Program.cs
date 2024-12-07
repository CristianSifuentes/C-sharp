using MyProject;
using MyProject.Utilities;
using MyProject.Models;


//A using clause must precede all other elements defined in the namespace except extern alias declarations

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


