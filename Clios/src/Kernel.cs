using Clios.CommandProcessor;
using System;
using System.Collections.Generic;
#if COSMOS
using Sys = Cosmos.System;
#endif

namespace Clios
{
#if COSMOS
    public class Kernel: Sys.Kernel
#else
    public class Kernel
#endif
    {
#if COSMOS
        protected override void BeforeRun()
        {
            Sys.FileSystem.VFS.VFSManager.RegisterVFS(Global.FileSystem);
            Global.FileSystem.Initialize();
        }

        protected override void Run()
        {
            run();
        }
#else
        public void Run()
        {
            run();
        }
#endif

        private void run()
        {
            //Tests();

            Console.Clear();
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

            Console.WriteLine();

            Console.ReadKey();
            l.RemoveAt(5);
            foreach (char c in l)
                Console.Write(c);
            Console.ReadKey();
        }
    }
}
