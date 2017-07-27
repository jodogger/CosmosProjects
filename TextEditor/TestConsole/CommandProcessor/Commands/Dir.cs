using System;
using System.Collections.Generic;
using System.IO;

namespace Kernel.CommandProcessor.Commands
{
    public class Dir : BaseCommand
    {
        public Dir()
        {
            name = "dir";
            description = "Displays a list of files and directories in a directory";
            parameterCount = 1;
        }

        public override void Do(params string[] args)
        {
            long size = 0;
            IEnumerable<string> files = Directory.EnumerateFiles(Global.CurrentPath);
            IEnumerable<string> dirs = Directory.EnumerateDirectories(Global.CurrentPath);

            CommandResult.AddSuccessMessage("");
            CommandResult.AddSuccessMessage(" Listing for " + Global.CurrentPath);
            CommandResult.AddSuccessMessage("");

            try
            {
                int dc = 0, fc = 0;
                foreach (string s in dirs)
                {
                    dc++;
                    CommandResult.AddSuccessMessage(" <Dir>    " + Path.GetFileName(s));
                }

                foreach (string s in files)
                {
                    System.IO.FileInfo f = new FileInfo(s);
                    fc++;
                    CommandResult.AddSuccessMessage(f.Length.ToString().PadLeft(9, ' ') + " " + Path.GetFileName(s));
                }

                CommandResult.AddSuccessMessage("");
                CommandResult.AddSuccessMessage(" " + fc.ToString().PadLeft(8, ' ') + " File(s)");
                CommandResult.AddSuccessMessage(" " + dc.ToString().PadLeft(8, ' ') + " Dir(s)");
            }
            catch (Exception ex)
            {
                CommandResult.AddErrorMessage("Unable to get listings.");
                CommandResult.Exception = ex;
            }
        }

        public override BaseCommand Create()
        {
            return new Dir();
        }
    }
}
