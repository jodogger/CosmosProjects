﻿using System;
using System.IO;

namespace Clios.CommandProcessor.Commands
{
    public class Type : BaseCommand
    {
        public Type()
        {
            Name = "type";
            Description = "Display contents of a text file.";
            ParameterCount = 2;
        }

        public override void Execute(params string[] args)
        {
            string file = args[1];

            if(!file.Contains(Global.CurrentPath))
                file = Path.Combine(Global.CurrentPath, args[1]);

            string[] content;

            try
            {
                if(File.Exists(file))
                {
                    content = File.ReadAllLines(file);
                    foreach (string s in content)
                        CommandResult.AddSuccessMessage(s);
                }
                else
                {
                    CommandResult.AddErrorMessage("Invalid filename '" + file + "'");
                }
            }
            catch (Exception ex)
            {
                CommandResult.AddErrorMessage("Unable to type file.");
                CommandResult.Exception = ex;
            }
        }

        public override BaseCommand Create()
        {
            return new Type();
        }
    }
}
