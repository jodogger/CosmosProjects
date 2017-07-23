using System;
using System.IO;

namespace Clios.CommandProcessor.Commands
{
    public class Cd : BaseCommand
    {
        public Cd()
        {
            Name = "cd";
            Description = "Change the current directory.";
            ParameterCount = 2;
        }

        public override void Do(params string[] args)
        {
            string p = Global.CurrentPath;
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
                //if(Directory.Exists(Path.Combine(Global.CurrentPath, newPath)))
                try
                {
                    if (Global.FileSystem.GetDirectory(Path.Combine(Global.CurrentPath, newPath)) != null)
                    {
                        p = Path.Combine(Global.CurrentPath, newPath);
                    }
                    else
                    {
                        CommandResult.AddErrorMessage(DirNotExisit(newPath));
                    }
                }
                catch
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
