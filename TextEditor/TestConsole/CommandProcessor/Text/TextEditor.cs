using Kernel.Extentions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kernel.CommandProcessor.Text
{
    public class TextEditor
    {
        ITextInputIO textInputIO = null;
        bool processTab = false;
        InputMode inputMode = InputMode.Insert;
        List<string> lines = new List<string>();

        public TextEditor(ITextInputIO textInputIO, bool processTab = true)
        {
            this.textInputIO = textInputIO;
            this.processTab = processTab;
        }

        public string Edit(string value)
        {
            Console.TreatControlCAsInput = true;
            bool loop = true;
            ConsoleTextInputIO consoleTextInputIO = new ConsoleTextInputIO();
            TextInput textInput = new TextInput(consoleTextInputIO, false);
            int top = 0, left = 0;
            int index = 0;

            consoleTextInputIO.Clear();

            while (loop)
            {
                consoleTextInputIO.SetCursor(top, left);
                TextInputResult textResult = textInput.GetText("");
                if(textResult.ConsoleKeyInfo.Key == System.ConsoleKey.Enter)
                {
                    lines = lines.InsertAtX(index, textResult.Result);
                    index++;
                    top++;
                    continue;
                }

                switch(textResult.ConsoleKeyInfo.Key)
                {
                    case System.ConsoleKey.C:
                        loop = false;
                        break;
                }
            }

            consoleTextInputIO.Clear();
            return StringListToString(lines);
        }

        private string StringListToString(List<string> rows)
        {
            StringBuilder sb = new StringBuilder();
            foreach (string s in rows)
            {
                sb.AppendLine(s);
            }
            return sb.ToString();
        }
    }
}
