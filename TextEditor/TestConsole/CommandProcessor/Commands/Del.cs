using System;
using System.IO;

namespace Kernel.CommandProcessor.Commands
{
    public class Del : BaseCommand
    {
        public Del()
        {
            name = "del";
            description = "Delete file.";
            parameterCount = 2;
        }

        public override void Do(params string[] args)
        {
            string file = args[1];
            if (!file.Contains(Global.CurrentPath))
                file = Global.CurrentPath + args[1];

            if (!File.Exists(file))
            {
                CommandResult.AddErrorMessage("File does not exist.");
            }
            else
            {
                try
                {
                    File.Delete(file);
                    CommandResult.AddSuccessMessage(file + " file deleted.");
                }
                catch (Exception ex)
                {
                    CommandResult.AddErrorMessage("Error creating file:" + ex.Message);
                    CommandResult.Exception = ex;
                }
            }
        }
        
        public override BaseCommand Create()
        {
            return new Del();
        }
    }
}
