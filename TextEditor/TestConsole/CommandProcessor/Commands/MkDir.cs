using System;
using System.IO;

namespace Kernel.CommandProcessor.Commands
{
    public class MkDir : BaseCommand
    {
        public MkDir()
        {
            name = "mkdir";
            description = "Create a new directory.";
            parameterCount = 2;
        }

        public override void Do(params string[] args)
        {
            try
            {
                string dir = args[1];
                if(!dir.Contains(Global.CurrentPath))
                {
                    dir = Path.Combine(Global.CurrentPath, args[1]);
                }

                if(!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
                else
                {
                    CommandResult.AddErrorMessage("Unable to create directory, directory already exisit.");
                }
            }
            catch(Exception ex)
            {
                CommandResult.AddErrorMessage("Unable to create directory.");
                CommandResult.Exception = ex;
            }
        }

        public override BaseCommand Create()
        {
            return new MkDir();
        }
    }
}
