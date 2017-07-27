using System;

namespace Kernel.CommandProcessor.Commands
{
    public class RmDir : BaseCommand
    {
        public RmDir()
        {
            name = "rmdir";
            description = "Remove an existing directory.";
            parameterCount = 2;
        }

        public override void Do(params string[] args)
        {
            try
            {
                //DirectoryEntry de = fs.GetDirectory(Global.CurrentPath + args[1]);
                //if (de != null)
                //{
                //    fs.DeleteDirectory(de);
                //    Console.WriteLine("Directory removed: " + de.mFullPath);
                //    Console.WriteLine("");
                //}
                //else
                //{
                //    Console.WriteLine("Directory '" + Global.CurrentPath + args[1] + "' does not exist.");
                //}
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
