using Kernel;
using Kernel.CommandProcessor;
using Kernel.CommandProcessor.Text;
using System;
using System.IO;

namespace TestConsole
{
    public class Program
    {
        static void Main(string[] args)
        {
            ConsoleTextInputIO consoleTextInputIO = new ConsoleTextInputIO();
            TextEditor te = new TextEditor(consoleTextInputIO);
            string s = te.Edit("");

            Console.WriteLine(s);
            Console.ReadLine();


            //Global.CurrentPath = Directory.GetCurrentDirectory();
            //while (true)
            //{
            //    Console.Write(Global.CurrentPath + ">");
            //    string s = Processor.GetCommand();
            //    Console.WriteLine(s + "\n");
            //}
        }
    }
}
