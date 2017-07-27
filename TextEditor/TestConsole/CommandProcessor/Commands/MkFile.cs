using System;
using System.IO;

namespace Kernel.CommandProcessor.Commands
{
    public class MkFile : BaseCommand
    {
        public MkFile()
        {
            name = "mkfile";
            description = "Create a new file.";
            parameterCount = 2;
        }

        public override void Do(params string[] args)
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
                    File.Create(file);
                    File.WriteAllText(file, "Test.");
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
