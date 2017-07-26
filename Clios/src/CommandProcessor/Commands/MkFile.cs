using System;
using System.IO;

namespace Clios.CommandProcessor.Commands
{
    public class MkFile : BaseCommand
    {
        public MkFile()
        {
            Name = "mkfile";
            Description = "Create a new file.";
            ParameterCount = 2;
        }

        public override void Execute(params string[] args)
        {
            string file = args[1];
            try
            {
                if (!file.Contains(Global.CurrentPath))
                {
                    file = Path.Combine(Global.CurrentPath, args[1]);
                }

                try
                {
                    Global.FileSystem.CreateFile(file);
                }
                catch (Exception ex)
                {
                    CommandResult.AddErrorMessage("Unable to write file [" + file + "].");
                    CommandResult.Exception = ex;
                }
            }
            catch (Exception ex)
            {
                CommandResult.AddErrorMessage("Unable to create file [" + file + "].");
                CommandResult.Exception = ex;
            }
        }

        public override BaseCommand Create()
        {
            return new MkFile();
        }
    }
}
