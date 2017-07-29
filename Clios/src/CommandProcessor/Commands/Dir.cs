#if COSMOS
using Cosmos.System.FileSystem.Listing;
#else
using Clios.Helpers;
#endif

using System;
using System.Collections.Generic;
using System.IO;

namespace Clios.CommandProcessor.Commands
{
    public class Dir : BaseCommand
    {
        public Dir()
        {
            Name = "dir";
            Description = "Displays a list of files and directories in a directory";
            ParameterCount = 1;
        }

        public override void Execute(params string[] args)
        {
            long size = 0;
            List<string> files = new List<string>();
            List<string> dirs = new List<string>();

            List<DirectoryEntry> listing = new List<DirectoryEntry>();
            listing = Global.FileSystem.GetDirectoryListing(Global.CurrentPath);

            foreach (DirectoryEntry de in listing)
            {
                if (de.mEntryType == DirectoryEntryTypeEnum.Directory)
                    dirs.Add(de.mName);
                else
                    files.Add(de.mName);
            }
            
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
                    //System.IO.FileInfo f = new FileInfo(s);
                    fc++;
                    CommandResult.AddSuccessMessage("         " + " " + Path.GetFileName(s));
                    //CommandResult.AddSuccessMessage(f.Length.ToString().PadLeft(9, ' ') + " " + Path.GetFileName(s));
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
