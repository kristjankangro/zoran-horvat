namespace Demo;

public class SeparateDelegate
{
    public static void Run(MyDelegate myDelegate)
    {
        Console.WriteLine("----------------------------");
        myDelegate();
    }
}

public delegate void MyDelegate();

public class SomeDelegateContainer
{
    public static void DoDelegate()
    {
        Console.WriteLine("---------DoDelegate------------------");
    } 
    
    public static void DoDelegate2()
    {
        Console.WriteLine("---------DoDelegate2------------------");
    }
}

public class Generator
{
    public static MyDelegate Go
    {
        get { return () => Console.WriteLine("--------Generator-DoDelegate-----------------"); }
    }
}