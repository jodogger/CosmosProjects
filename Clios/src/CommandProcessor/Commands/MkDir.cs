using System;
using System.IO;

namespace Clios.CommandProcessor.Commands
{
    public class MkDir : BaseCommand
    {
        public MkDir()
        {
            Name = "mkdir";
            Description = "Create a new directory.";
            ParameterCount = 2;
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
                    Global.FileSystem.CreateDirectory(dir); // Directory.CreateDirectory(dir);
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
