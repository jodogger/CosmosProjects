using Cosmos.System.FileSystem.Listing;
using System;

namespace Clios.CommandProcessor.Commands
{
    public class RmDir : BaseCommand
    {
        public RmDir()
        {
            Name = "rmdir";
            Description = "Remove an existing directory.";
            ParameterCount = 2;
        }

        public override void Execute(params string[] args)
        {
            try
            {
                DirectoryEntry de = Global.FileSystem.GetDirectory(Global.CurrentPath + args[1]);
                if (de != null)
                {
                    Global.FileSystem.DeleteDirectory(de);
                    Console.WriteLine("Directory removed: " + de.mFullPath);
                    Console.WriteLine("");
                }
                else
                {
                    Console.WriteLine("Directory '" + Global.CurrentPath + args[1] + "' does not exist.");
                }
            }
            catch(Exception ex)
            {
                CommandResult.AddErrorMessage("Unable to remove directory: " + ex.Message);
                CommandResult.Exception = ex;
            }
        }

        public override BaseCommand Create()
        {
            return new RmDir();
        }
    }
}
