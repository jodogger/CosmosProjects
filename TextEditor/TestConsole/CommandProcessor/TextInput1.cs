/*
using System;
using System.Collections.Generic;
using System.Text;

namespace Kernel.CommandProcessor
{
    public enum InputMode { Insert, Override };

    public class ConsoleTextInput : TextInput1
    {
        protected override ConsoleKeyInfo ReadKey(bool truncate)
        {
            Console.TreatControlCAsInput = true;
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

        protected override void Write(List<char> chars)
        {
            foreach (char c in chars)
                Console.Write(c);
        }

        protected override void WriteLine(string value)
        {
            Console.WriteLine(value);
        }

        protected override void MoveCursorLeft()
        {
            Console.CursorLeft--;
        }

        protected override void MoveCursorRight()
        {
            Console.CursorLeft++;
            if (Console.CursorLeft > Console.BufferWidth)
            {
                Console.CursorLeft = 1;
                Console.CursorTop++;
            }
        }

        protected override void SetCursorSize(int i)
        {
            Console.CursorSize = i;
        }

        protected override void SetCursorLeft(int top, int left)
        {
            if (left > Console.BufferWidth)
            {
                left = 1;
                Console.CursorTop++;
            }

            Console.CursorLeft = left;
            Console.CursorTop = top;
        }

        protected override int GetCursorLeft()
        {
            return Console.CursorLeft;
        }
    }

    public abstract class TextInput1
    {
        protected abstract ConsoleKeyInfo ReadKey(bool truncate);
        protected abstract void Write(string value);
        protected abstract void Write(char value);
        protected abstract void WriteLine(string value = "");
        protected abstract void MoveCursorLeft();
        protected abstract void MoveCursorRight();
        protected abstract void SetCursorSize(int i);
        protected abstract int GetCursorLeft();
        protected abstract void SetCursorLeft(int top, int left);
        protected abstract void Write(List<char> chars);

        private InputMode inputMode = InputMode.Insert;
        public InputMode InputMode
        {
            get
            {
                return inputMode;
            }
            set
            {
                inputMode = value;
                if (inputMode == InputMode.Insert)
                    SetCursorSize(25);
                else
                    SetCursorSize(100);
            }
        }

        private List<List<char>> history = new List<List<char>>();
        private List<string> tabItems = new List<string>();
        int historyIndex = 0;

        public string GetText1(List<string> tabItems)
        {
            int startLeft = GetCursorLeft();
            int startTop = Console.CursorTop;

            List<char> chars = new List<char>();
            int index = 0;
            int padsize = 0;
            bool loop = true;
            int tabSubItemIndex = 0;
            string tabItemsStartsWith = "";
            string tabCommand = "";
            List<string> subTabItems = new List<string>();

            this.tabItems = tabItems;
            historyIndex = history.Count - 1;
            InputMode = InputMode.Insert;

            while (loop)
            {
                SetCursorLeft(startTop, startLeft);
                Write(chars);
                if (padsize > chars.Count)
                    Write(new string(' ', padsize - chars.Count));
                padsize = chars.Count;
                SetCursorLeft(startTop, startLeft + index);
                ConsoleKeyInfo cki = ReadKey(true);

                if (cki.KeyChar > 31 && cki.KeyChar < 127)
                {
                    switch (InputMode)
                    {
                        case InputMode.Insert:
                            chars.Insert(index, cki.KeyChar);
                            break;
                        case InputMode.Override:
                            if (index < chars.Count)
                                chars[index] = cki.KeyChar;
                            else
                                chars.Insert(index, cki.KeyChar);
                            break;
                    }
                    Write(cki.KeyChar);
                    index++;
                    continue;
                }

                switch (cki.Key)
                {
                    case ConsoleKey.Backspace:
                        if (index > 0)
                        {
                            index--;
                            chars.RemoveAt(index);
                        }
                        tabItemsStartsWith = "";
                        subTabItems.Clear();
                        tabSubItemIndex = 0;
                        break;
                    case ConsoleKey.C:
                        chars.Clear();
                        WriteLine();
                        loop = false;
                        break;
                    case ConsoleKey.Delete:
                        if (index < chars.Count)
                            chars.RemoveAt(index);
                        break;
                    case ConsoleKey.DownArrow:
                        if (historyIndex > -1)
                        {
                            if (historyIndex < history.Count - 1)
                            {
                                historyIndex++;
                            }
                            chars = history[historyIndex];
                            index = chars.Count;
                        }
                        break;
                    case ConsoleKey.End:
                        SetCursorLeft(startTop, chars.Count);
                        index = chars.Count;
                        break;
                    case ConsoleKey.Enter:
                        loop = false;
                        break;
                    case ConsoleKey.Home:
                        SetCursorLeft(startTop, startLeft);
                        index = 0;
                        break;
                    case ConsoleKey.Insert:
                        if (InputMode == InputMode.Insert)
                        {
                            InputMode = InputMode.Override;
                            SetCursorSize(100);
                        }
                        else
                        {
                            InputMode = InputMode.Insert;
                            SetCursorSize(25);
                        }
                        break;
                    case ConsoleKey.LeftArrow:
                        if (index > 0)
                        {
                            index--;
                            MoveCursorLeft();
                        }
                        break;
                    case ConsoleKey.RightArrow:
                        if (index < chars.Count)
                        {
                            index++;
                            MoveCursorRight();
                        }
                        break;
                    case ConsoleKey.Tab:
                        if (tabItemsStartsWith.Length == 0 && subTabItems.Count == 0) // get substring to search on
                        {
                            subTabItems.Clear();
                            if (chars.Contains(' '))
                            {
                                string c = CharListToString(chars);
                                int i = c.IndexOf(' ');
                                tabCommand = c.Substring(0, i + 1);
                                tabItemsStartsWith = c.Substring(i + 1);
                            }
                            else
                            {
                                tabItemsStartsWith = CharListToString(chars);
                                tabCommand = "";
                            }

                            foreach (string s in tabItems)
                            {
                                if (tabItemsStartsWith.Length > 0)
                                {
                                    if (s.ToLower().StartsWith(tabItemsStartsWith.ToLower()))
                                        subTabItems.Add(s);
                                }
                                else
                                {
                                    subTabItems.Add(s);
                                }
                            }
                        }

                        if (tabSubItemIndex >= subTabItems.Count)
                            tabSubItemIndex = 0;
                        else if (tabSubItemIndex < 0)
                            tabSubItemIndex = subTabItems.Count - 1;

                        if (subTabItems.Count > 0)
                        {
                            chars = StringToCharList(tabCommand + subTabItems[tabSubItemIndex]);
                            index = chars.Count;
                            if (cki.Modifiers.HasFlag(ConsoleModifiers.Shift))
                            {
                                if (tabSubItemIndex > 0)
                                    tabSubItemIndex--;
                                else
                                    tabSubItemIndex = subTabItems.Count - 1;
                            }
                            else
                            {
                                if (tabSubItemIndex < subTabItems.Count - 1)
                                    tabSubItemIndex++;
                                else
                                    tabSubItemIndex = 0;
                            }
                        }
                        break;
                    case ConsoleKey.UpArrow:
                        if (historyIndex > -1)
                        {
                            if (historyIndex > 0)
                            {
                                historyIndex--;
                            }
                            chars = history[historyIndex];
                            index = chars.Count;
                        }
                        break;
                }
            }

            if (chars.Count > 0)
            {
                if (history.Count > 0)
                {
                    if (!IsSame(history[history.Count - 1], chars)) // make sure current command is not in the top of history, so it's not added twice in a row
                    {
                        history.Add(chars);
                        historyIndex = history.Count - 1;
                    }
                }
                else
                {
                    history.Add(chars);
                    historyIndex = history.Count - 1;
                }
            }

            return CharListToString(chars);
        }

        private bool IsSame(List<char> one, List<char> two)
        {
            if (one.Count != two.Count)
                return false;

            for (int i = 0; i < one.Count; i++)
                if (one[i] != two[i])
                    return false;
            return true;
        }

        private List<char> StringToCharList(string v)
        {
            List<char> cs = new List<char>();
            for (int i = 0; i < v.Length; i++)
                cs.Add(v[i]);
            return cs;
        }

        private string CharListToString(List<char> cs)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in cs)
                sb.Append(c);
            return sb.ToString();
        }
    }
}
*/