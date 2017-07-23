using Cosmos.System.FileSystem.Listing;
using System;
using System.Collections.Generic;

namespace Clios.CommandProcessor
{
    public enum InputMode { Insert, Override };

    public class ConsoleTextInput
    {
        protected void Write(List<char> chars)
        {
            foreach (char c in chars)
                Console.Write(c);
        }

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
                {
                    Console.CursorSize = 25;
                }
                else
                {
                    Console.CursorSize = 100;
                }
            }
        }

        private List<List<char>> history = new List<List<char>>();
        private List<DirectoryEntry> tabItems = new List<DirectoryEntry>();
        int historyIndex = 0;

        public string GetText(List<DirectoryEntry> tabItems)
        {
            int startLeft = Console.CursorLeft;
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
                Console.CursorLeft = startLeft;
                Write(chars);

                if (padsize > chars.Count) // clear to end of line
                {
                    Console.Write(new string(' ', padsize - chars.Count));
                }

                padsize = chars.Count;
                Console.CursorLeft = startLeft + index;
                ConsoleKeyInfo cki = Console.ReadKey(true);

                if (IsValidChar(cki))
                {
                    switch (InputMode)
                    {
                        case InputMode.Insert:
                            {// TODO: BUGGED! chars.Insert(index, cki.KeyChar);
                                chars = ListInsert(chars, index, cki.KeyChar);
                            }
                            break;
                        case InputMode.Override:
                            if (index < chars.Count)
                            {
                                chars[index] = cki.KeyChar;
                            }
                            else
                            {
                                {// TODO: BUGGED! chars.Insert(index, cki.KeyChar);
                                    chars = ListInsert(chars, index, cki.KeyChar);
                                }
                            }
                            break;
                    }
                    Console.Write(cki.KeyChar);
                    index++;
                    continue;
                }

                switch (cki.Key)
                {
                    case ConsoleKey.Backspace:
                        if (index > 0)
                        {
                            index--;
                            {// TODO: BUGGED! chars.RemoveAt(index);
                                chars = ListRemoveAt(chars, index);
                            }
                        }
                        tabItemsStartsWith = "";

                        {// TODO: BUGGED! subTabItems.Clear();
                            if (subTabItems.Count > 0)
                                subTabItems = new List<string>();
                        }

                        tabSubItemIndex = 0;
                        break;
                    case ConsoleKey.C:
                        //chars.Clear();
                        //WriteLine("");
                        loop = false;
                        break;
                    case ConsoleKey.Delete:
                        if (index < chars.Count)
                        {
                            {// TODO: BUGGED! chars.RemoveAt(index);
                                chars = ListRemoveAt(chars, index);
                            }
                        }
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
                        Console.CursorLeft = chars.Count;
                        index = chars.Count;
                        break;
                    case ConsoleKey.Enter:
                        loop = false;
                        Console.WriteLine("");
                        break;
                    case ConsoleKey.Home:
                        Console.CursorLeft = startLeft;
                        index = 0;
                        break;
                    case ConsoleKey.Insert:
                        if (InputMode == InputMode.Insert)
                        {
                            InputMode = InputMode.Override;
                            Console.CursorSize = 100;
                        }
                        else
                        {
                            InputMode = InputMode.Insert;
                            Console.CursorSize = 25;
                        }
                        break;
                    case ConsoleKey.LeftArrow:
                        if (index > 0)
                        {
                            index--;
                            Console.CursorLeft--;
                        }
                        break;
                    case ConsoleKey.PageUp:
                        break;
                    case ConsoleKey.RightArrow:
                        if (index < chars.Count)
                        {
                            index++;
                            Console.CursorLeft++;
                        }
                        break;
                    case ConsoleKey.Tab:
                        if (tabItemsStartsWith.Length == 0 && subTabItems.Count == 0) // get substring to search on
                        {
                            subTabItems.Clear();
                            if (ListContains(chars, ' '))
                            {
                                string c = CharToString(chars);
                                int i = c.IndexOf(' ');
                                tabCommand = c.Substring(0, i + 1);
                                tabItemsStartsWith = c.Substring(i + 1);
                            }
                            else
                            {
                                tabItemsStartsWith = CharToString(chars);
                                tabCommand = "";
                            }

                            string s = "";
                            for (int i = 0; i < tabItems.Count; i++)
                            {
                                // TODO: Get currently typed command and filter on command type: If dir command, only display dirs. if a file command, only display files
                                s = tabItems[i].mName;
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
                            if (cki.Modifiers == ConsoleModifiers.Shift)
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

            return CharToString(chars);
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

        private string CharToString(List<char> cs)
        {
            string s = "";
            foreach (char c in cs)
                s += c;
            return s;
        }

        private bool IsValidChar(ConsoleKeyInfo cki)
        {
            if (cki.KeyChar > 31 && cki.KeyChar < 127)
                return true;
            return false;
        }

        // TODO: BUG WORKAROUND
        private List<char> ListInsert(List<char> list, int index, char c)
        {
            List<char> newList = new List<char>();
            int i = 0;

            for (; i < index; i++)
                newList.Add(list[i]);

            newList.Add(c);

            for (; i < list.Count; i++)
                newList.Add(list[i]);

            return newList;
        }

        // TODO: BUG WORKAROUND
        private List<char> ListRemoveAt(List<char> list, int index)
        {
            List<char> newList = new List<char>();
            int i = 0;

            for (; i < index; i++)
                newList.Add(list[i]);

            i++;

            for (; i < list.Count; i++)
                newList.Add(list[i]);

            return newList;
        }

        // TODO: BUG WORKAROUND
        private bool ListContains(List<char> list, char c)
        {
            foreach (char chr in list)
                if (chr == c)
                    return true;
            return false;
        }
    }
}
