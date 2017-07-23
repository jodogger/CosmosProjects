using Clios.CommandProcessor;
using System;
using System.Collections.Generic;
using Sys = Cosmos.System;

namespace Clios
{
    public class Kernel: Sys.Kernel
    {
        protected override void BeforeRun()
        {
            Sys.FileSystem.VFS.VFSManager.RegisterVFS(Global.FileSystem);
            Global.FileSystem.Initialize();
            //Tests();
            Console.Clear();
        }

        protected override void Run()
        {
            while (true)
            {
                string prompt = GetPrompt();
                Console.Write(prompt);
                Processor.GetCommand();
                Console.WriteLine();
            }
        }

        private string GetPrompt()
        {
            string p = "[" + "admin" + "] " + Global.CurrentPath + ">";
            return p;
        }
                
        private void Tests()
        {
            List<char> l = new List<char> { '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', };
            l.Insert(5, 'a');
            foreach (char c in l)
                Console.Write(c);
            Console.ReadKey();
            l.RemoveAt(5);
            foreach (char c in l)
                Console.Write(c);
            Console.ReadKey();
        }
    }
}
