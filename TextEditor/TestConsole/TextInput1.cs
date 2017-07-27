using System;
using System.Collections.Generic;
using System.Text;

namespace Kernel
{
    public enum InputMode { Insert, Override };

    public class ConsoleTextInput : TextInput
    {
        protected override ConsoleKeyInfo ReadKey(bool truncate)
        {
            return Console.ReadKey(true);
        }

        protected override void Write(string value)
        {
            Console.Write(value);
        }

        protected override void Write(char value)
        {
            Console.Write(value);
        }

        protected override void WriteLine(string value)
        {
            Console.Write(value);
        }
    }

    public abstract class TextInput
    {
        public InputMode InputMode { get; set; }

        List<char> chars = new List<char>();
        int index = 0;
        bool loop = true;

        public TextInput()
        {
            InputMode = InputMode.Insert;
        }

        protected abstract ConsoleKeyInfo ReadKey(bool truncate);
        protected abstract void Write(string value);
        protected abstract void Write(char value);
        protected abstract void WriteLine(string value);

        public string GetText()
        {
            while (loop)
            {
                ConsoleKeyInfo cki = ReadKey(true);

                if (IsValidChar(cki))
                {
                    switch(InputMode)
                    {
                        case InputMode.Insert:
                            chars.Insert(index, cki.KeyChar);
                            break;
                        case InputMode.Override:
                            chars[index] = cki.KeyChar;
                            break;
                    }

                    Write(cki.KeyChar);
                    index++;
                    continue;
                }

                switch(cki.Key)
                {
                    case ConsoleKey.Enter:
                        loop = false;
                        break;
                }
            }

            return chars.ToArray().ToString();
        }

        private bool IsValidChar(ConsoleKeyInfo cki)
        {
            if (cki.KeyChar > 31 && cki.KeyChar < 127)
                return true;
            
            return false;
        }
    }
}
