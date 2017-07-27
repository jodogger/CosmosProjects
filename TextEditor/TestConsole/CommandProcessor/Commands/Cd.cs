using System;
using System.IO;

namespace Kernel.CommandProcessor.Commands
{
    public class Cd : BaseCommand
    {
        public Cd()
        {
            name = "cd";
            description = "Change the current directory.";
            parameterCount = 2;
        }

        public override void Do(params string[] args)
        {
            string p = "";
            string newPath = args[1];

            if (newPath.StartsWith(@"\"))
            {
                if(Directory.Exists(newPath))
                {
                    p = newPath;
                }
                else
                {
                    CommandResult.AddErrorMessage(DirNotExisit(newPath));
                }
            }
            else if (newPath.StartsWith(@".."))
            {
                if (Global.CurrentPath != Global.STARTING_PATH)
                {
                    p = Global.CurrentPath.Substring(0, Global.CurrentPath.Length - 1);

                    int i = p.Length - 1;
                    while (p[i-1] != '\\' && i > 3)
                        i--;

                    p = Global.CurrentPath.Substring(0, i);
                }
                else
                {
                    p = Global.CurrentPath;
                }
            }
            else if (newPath.StartsWith(@"."))
            {
                p = Global.STARTING_PATH;
            }
            else
            {
                if(Directory.Exists(Path.Combine(Global.CurrentPath, newPath)))
                {
                    p = Path.Combine(Global.CurrentPath, newPath);
                }
                else
                {
                    CommandResult.AddErrorMessage(DirNotExisit(newPath));
                }
            }

            Global.CurrentPath = p;
        }

        private string DirNotExisit(string v)
        {
            return "Directory does not exist: " + v;
        }

        public override BaseCommand Create()
        {
            return new Cd();
        }
    }
}
