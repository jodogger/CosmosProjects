﻿using Clios.CommandProcessor.Text;
using System.Collections.Generic;
using System.IO;

namespace Clios.CommandProcessor.Commands
{
    public class Edit : BaseCommand
    {
        TextEditor editor = null;

        public Edit()
        {
            Name = "edit";
            Description = "Edit test file.";
            ParameterCount = 2;
            ConsoleTextInputIO consoleTextInputIO = new ConsoleTextInputIO();
            editor = new TextEditor(consoleTextInputIO, false);
        }

        public override void Execute(params string[] args)
        {
            List<List<char>> contents = new List<List<char>>();
            string file = Path.Combine(Global.CurrentPath, args[1]);
            if (!File.Exists(file))
            {
                File.Create(file);
            }
            else
            {
                string[] f = File.ReadAllLines(file);
                foreach (string s in f)
                    contents.Add(GetCharArray(s));
            }

            editor.Edit(file);
            CommandResult.ClearScreen = true;
        }

        private List<char> GetCharArray(string v)
        {
            List<char> l = new List<char>();
            foreach (char c in v)
                l.Add(c);
            return l;
        }

        public override BaseCommand Create()
        {
            return new Edit();
        }
    }
}
