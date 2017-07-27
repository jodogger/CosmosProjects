//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace Kernel
//{
//    public enum InputMode { Insert, Override };

//    public class TextInput
//    {
//        protected ConsoleKeyInfo ReadKey(bool truncate)
//        {
//            Console.TreatControlCAsInput = true;
//            return Console.ReadKey(true);
//        }

//        protected void Write(string value)
//        {
//            Console.Write(value);
//        }

//        protected void Write(char value)
//        {
//            Console.Write(value);
//        }

//        protected void Write(List<char> chars)
//        {
//            foreach (char c in chars)
//                Console.Write(c);
//        }

//        protected void WriteLine(string value)
//        {
//            Console.WriteLine(value);
//        }

//        protected void SetCursorSize(int i)
//        {
//            Console.CursorSize = i;
//        }

//        protected void SetCursorLeft(int top, int left)
//        {
//            Console.CursorLeft = left;
//            Console.CursorTop = top;
//        }

//        protected int GetCursorLeft()
//        {
//            return index;
//        }

//        private InputMode inputMode = InputMode.Insert;
//        public InputMode InputMode
//        {
//            get
//            {
//                return inputMode;
//            }
//            set
//            {
//                inputMode = value;
//                if (inputMode == InputMode.Insert)
//                    SetCursorSize(25);
//                else
//                    SetCursorSize(100);
//            }
//        }

//        int index = 0;
//        int rowIndex = 0;
//        bool loop = true;
//        int padsize = 0;

//        public string GetText(List<List<char>> rows)
//        {
//            void SetCursor(int t, int l)
//            {
//                Console.CursorTop = t;
//                Console.CursorLeft = l;
//            }

//            void ScrollConsoleUpOne()
//            {
//                Console.MoveBufferArea(0, 1, 80, 23, 0, 0);
//            }

//            void ScrollConsoleDownOne()
//            {
//                Console.MoveBufferArea(0, 0, 80, 23, 0, 1);
//            }

//            string result = "";
//            int top = 0, left = 0;
//            ConsoleKeyInfo cki;

//            Console.Clear();

//            if (rows.Count == 0)
//                rows.Add(new List<char>());
//            else
//            {
//                int i = 0;
//                foreach (List<char> c in rows)
//                {
//                    Console.WriteLine(CharListToString(c));
//                    i++;
//                    if (i > 23)
//                        break;
//                }
//                SetCursorLeft(rowIndex, index);
//            }

//            while (loop)
//            {
//                SetCursor(24, 0);
//                DisplayStatusBar(top, left);
//                SetCursor(top, left);
//                cki = ReadKey(true);

//                if (cki.KeyChar > 31 && cki.KeyChar < 127)
//                {
//                }
//                else
//                {
//                    switch (cki.Key)
//                    {
//                        case ConsoleKey.C:
//                            loop = false;
//                            break;
//                        case ConsoleKey.DownArrow:
//                            if (rowIndex < rows.Count - 1)
//                            {
//                                if (top < 23)
//                                {
//                                    top++;
//                                    rowIndex++;
//                                }
//                                else
//                                {
//                                    if (rowIndex < rows.Count - 1)
//                                    {
//                                        ScrollConsoleUpOne();
//                                        rowIndex++;
//                                        SetCursor(23, 0);
//                                        Write(rows[rowIndex]);
//                                    }
//                                }
//                            }
//                            break;
//                        case ConsoleKey.LeftArrow:
//                            if (left > 0)
//                            {
//                                left--;
//                                index--;
//                            }
//                            break;
//                        case ConsoleKey.RightArrow:
//                            if (left < 79)
//                            {
//                                left++;
//                                index++;
//                            }
//                            break;
//                        case ConsoleKey.UpArrow:
//                            if (rowIndex > 0)
//                            {
//                                if (top > 0)
//                                {
//                                    top--;
//                                    rowIndex--;
//                                }
//                                else
//                                {
//                                    ScrollConsoleDownOne();
//                                    rowIndex--;
//                                    SetCursor(0, 0);
//                                    Write(rows[rowIndex]);
//                                }
//                            }
//                            break;
//                    }
//                }
//            }

//            return result;
//        }

//        public string GetText1(List<List<char>> rows)
//        {
//            index = 0;
//            rowIndex = 0;
//            loop = true;

//            if (rows.Count == 0)
//                rows.Add(new List<char>());
//            else
//            {
//                int i = 0;
//                foreach (List<char> c in rows)
//                {
//                    Console.WriteLine(CharListToString(c));
//                    i++;
//                    if (i > 23)
//                        break;
//                }
//                SetCursorLeft(rowIndex, index);
//            }

//            InputMode = InputMode.Insert;

//            while (loop)
//            {
//                DisplayStatusBar(0, 0);
//                SetCursorLeft(rowIndex, 0);
//                Write(rows[rowIndex]);

//                if (padsize > rows[rowIndex].Count)
//                    Write(new string(' ', padsize - rows[rowIndex].Count));
//                padsize = rows[rowIndex].Count;

//                SetCursorLeft(rowIndex, index);
//                ConsoleKeyInfo cki = ReadKey(true);

//                if (cki.KeyChar > 31 && cki.KeyChar < 127)
//                {
//                    switch (InputMode)
//                    {
//                        case InputMode.Insert:
//                            if (index < 79)
//                                rows[rowIndex].Insert(index, cki.KeyChar);
//                            else
//                            {
//                                if (rows[rowIndex].Count - 1 < 79)
//                                    rows[rowIndex].Add(cki.KeyChar);
//                                else
//                                    rows[rowIndex][index] = cki.KeyChar;
//                            }
//                            break;
//                        case InputMode.Override:
//                            if (index < rows[rowIndex].Count)
//                                rows[rowIndex][index] = cki.KeyChar;
//                            else
//                                rows[rowIndex].Insert(index, cki.KeyChar);
//                            break;
//                    }
//                    if (index < 79)
//                        index++;
//                    continue;
//                }

//                switch (cki.Key)
//                {
//                    case ConsoleKey.Backspace:
//                        if (index > 0)
//                        {
//                            index--;
//                            rows[rowIndex].RemoveAt(index);
//                        }
//                        break;
//                    case ConsoleKey.C:
//                        loop = false;
//                        break;
//                    case ConsoleKey.Delete:
//                        if (index < rows[rowIndex].Count)
//                            rows[rowIndex].RemoveAt(index);
//                        break;
//                    case ConsoleKey.DownArrow:
//                        if (rowIndex < rows.Count - 1)
//                            rowIndex++;
//                        if (index > rows[rowIndex].Count - 1)
//                            index = rows[rowIndex].Count;
//                        break;
//                    case ConsoleKey.End:
//                        SetCursorLeft(rowIndex, rows[rowIndex].Count);
//                        index = rows[rowIndex].Count;
//                        break;
//                    case ConsoleKey.Enter:
//                        switch (InputMode)
//                        {
//                            case InputMode.Insert:
//                                if (index == 0)
//                                {
//                                    SetCursorLeft(rowIndex, 0);
//                                    Write(new string(' ', rows[rowIndex].Count));
//                                    rows.Insert(rowIndex, new List<char>());
//                                    Console.MoveBufferArea(0, rowIndex + 1, 80, 23 - rowIndex, 0, rowIndex + 2);
//                                }
//                                else
//                                {
//                                    if (index < rows[rowIndex].Count)
//                                    {
//                                        List<char> c = rows[rowIndex].GetRange(index, rows[rowIndex].Count - index);
//                                        Write(new string(' ', rows[rowIndex].Count));
//                                        rows[rowIndex].RemoveRange(index, rows[rowIndex].Count - index);
//                                        rows.Insert(rowIndex + 1, c);
//                                        Console.MoveBufferArea(0, rowIndex + 1, 80, 23 - rowIndex, 0, rowIndex + 2);
//                                    }
//                                    else
//                                    {
//                                        rows.Add(new List<char>());
//                                        Console.MoveBufferArea(0, rowIndex + 1, 80, 23 - rowIndex, 0, rowIndex + 2);
//                                    }
//                                }
//                                rowIndex++;
//                                index = 0;
//                                break;
//                            case InputMode.Override:
//                                break;
//                        }
//                        break;
//                    case ConsoleKey.Home:
//                        index = 0;
//                        SetCursorLeft(rowIndex, index);
//                        break;
//                    case ConsoleKey.Insert:
//                        if (InputMode == InputMode.Insert)
//                        {
//                            InputMode = InputMode.Override;
//                            SetCursorSize(100);
//                        }
//                        else
//                        {
//                            InputMode = InputMode.Insert;
//                            SetCursorSize(25);
//                        }
//                        break;
//                    case ConsoleKey.LeftArrow:
//                        if (index > 0)
//                        {
//                            index--;
//                        }
//                        break;
//                    case ConsoleKey.RightArrow:
//                        if (index < rows[rowIndex].Count)
//                        {
//                            if (index < 79)
//                                index++;
//                        }
//                        break;
//                    case ConsoleKey.Tab:
//                        switch (InputMode)
//                        {
//                            case InputMode.Insert:
//                                for (int i = 0; i < 4; i++)
//                                    rows[rowIndex].Insert(index, ' ');
//                                index += 4;
//                                break;
//                            case InputMode.Override:
//                                break;
//                        }
//                        break;
//                    case ConsoleKey.UpArrow:
//                        if (rowIndex > 0)
//                            rowIndex--;
//                        if (index > rows[rowIndex].Count - 1)
//                            index = rows[rowIndex].Count;

//                        break;
//                }
//            }

//            return CharListToString(rows);
//        }

//        private void DisplayStatusBar(int t, int l)
//        {
//            //SetCursorLeft(24, 0);
//            Console.ForegroundColor = ConsoleColor.Yellow;
//            string s = " Ln " + rowIndex + "  Col " + index + "  Top " + t + "  Left " + l + "    " + (InputMode == InputMode.Insert ? "INS" : "OVR") + "      ";
//            Console.Write(s);
//            Console.ForegroundColor = ConsoleColor.Gray;
//        }

//        private bool IsSame(List<char> one, List<char> two)
//        {
//            if (one.Count != two.Count)
//                return false;

//            for (int i = 0; i < one.Count; i++)
//                if (one[i] != two[i])
//                    return false;
//            return true;
//        }

//        private List<char> StringToCharList(string v)
//        {
//            List<char> cs = new List<char>();
//            for (int i = 0; i < v.Length; i++)
//                cs.Add(v[i]);
//            return cs;
//        }

//        private string CharListToString(List<List<char>> rows)
//        {
//            StringBuilder sb = new StringBuilder();
//            foreach (List<char> cs in rows)
//            {
//                foreach (char c in cs)
//                    sb.Append(c);
//                sb.AppendLine();
//            }
//            return sb.ToString();
//        }

//        private string CharListToString(List<char> chars)
//        {
//            StringBuilder sb = new StringBuilder();
//            foreach (char c in chars)
//                sb.Append(c);
//            return sb.ToString();
//        }
//    }
//}
