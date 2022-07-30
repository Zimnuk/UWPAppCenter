
using System;
using AppCenterConsole;
public class Program
{
    public static void Main()
    {
        Console.WriteLine("Hello, World!");
        var helper = new ActionHelper();
        helper.GetBranches();
        var action = Console.ReadLine();
        helper.ActionChoose(action);
        Console.ReadKey();
    }

}





