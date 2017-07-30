using System;
using System.Collections.Generic;
using System.Text;

namespace Clios.CommandProcessor.Text
{
    public class TextEditor
    {
        ITextInputIO textInputIO = null;
        bool processTab = false;
        InputMode inputMode = InputMode.Insert;
        List<string> lines = new List<string>();
        ConsoleTextInputIO consoleTextInputIO = new ConsoleTextInputIO();
        bool saved = false;
        int curLine = 0;
        int top = 0, left = 0;

        public TextEditor(ITextInputIO textInputIO, bool processTab = true)
        {
            this.textInputIO = textInputIO;
            this.processTab = processTab;
        }

        public string Edit(string value)
        {
            bool loop = true;
            TextInput textInput = new TextInput(consoleTextInputIO, false);
            top = 0;
            left = 0;
            curLine = 0;

            consoleTextInputIO.Clear();

            while (loop)
            {
                DisplayStatusBar(top, left, curLine);
                consoleTextInputIO.SetCursor(top, left);
                TextInputResult textResult = textInput.GetText("");
                if (textResult.ConsoleKeyInfo.Key == System.ConsoleKey.Enter)
                {
                    lines.Insert(curLine, textResult.Result);
                    curLine++;
                    top++;
                    continue;
                }

                switch (textResult.ConsoleKeyInfo.Key)
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

        private void DisplayStatusBar(int t, int l, int rowCount)
        {
            consoleTextInputIO.SetCursor(24, 0);
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write(saved ? " " : "*");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            string s = "     Ln:" + Pad(curLine, 4) + "  Col:" + Pad(curLine, 3) + "  Top:" + Pad(t, 2) + "  Left:" + Pad(l, 2) + "  " + (inputMode == InputMode.Insert ? "INS" : "OVR") + "  Rows " + rowCount;
            Console.Write(s);
            Console.ForegroundColor = ConsoleColor.White;
        }

        private string Pad(int value, int length)
        {
            string r = value.ToString();
            for (int i = 0; i < 4 - r.Length; i++)
                r = "0" + r;
            return r;
        }
    }
}
