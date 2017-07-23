using System;
using System.Collections.Generic;
using System.IO;

namespace Clios.CommandProcessor
{
    public class TextEditor
    {
        protected ConsoleKeyInfo ReadKey(bool truncate)
        {
            //Console.TreatControlCAsInput = true;
            return Console.ReadKey(true);
        }

        protected void Write(string value)
        {
            Console.Write(value);
        }

        protected void Write(List<char> chars)
        {
            foreach (char c in chars)
                Console.Write(c);
        }

        protected void SetCursorSize(int i)
        {
            Console.CursorSize = i;
        }

        int colIndex = 0;
        int rowIndex = 0;
        bool loop = true;
        int padsize = 0;
        bool saved = true;
        string filename = "";

        InputMode InputMode = InputMode.Insert;

        void SetCursor(int t, int l)
        {
            Console.CursorTop = t;
            Console.CursorLeft = l;
        }

        void ScrollConsoleUpOne()
        {
            // TODO: BUGGED! Console.MoveBufferArea(0, 1, 80, 23, 0, 0);
        }

        void ScrollConsoleDownOne()
        {
            // TODO: BUGGED! Console.MoveBufferArea(0, 0, 80, 23, 0, 1);
        }

        public string GetText(string filename, List<List<char>> rows)
        {
            int top = 0, left = 0;
            ConsoleKeyInfo cki;
            rowIndex = 0;
            colIndex = 0;
            this.filename = filename;

            Console.Clear();

            if (rows.Count == 0)
            {
                rows.Add(new List<char>());
            }
            else
            {
                int i = 0;
                foreach (List<char> c in rows)
                {
                    Console.WriteLine(CharListToString(c));
                    i++;
                    if (i > 23)
                        break;
                }
            }

            while (loop)
            {
                DisplayStatusBar(top, left, rows.Count);

                SetCursor(top, 0);
                Write(rows[rowIndex]);
                if (padsize > rows[rowIndex].Count)
                    Write(new string(' ', padsize - rows[rowIndex].Count));
                padsize = rows[rowIndex].Count;

                SetCursor(top, left);
                cki = ReadKey(true);

                if (cki.KeyChar > 31 && cki.KeyChar < 127)
                {
                    saved = false;
                    switch (InputMode)
                    {
                        case InputMode.Insert:
                            if (colIndex < 79)
                            {
                                {
                                    //rows[rowIndex].Insert(colIndex, cki.KeyChar); 
                                    rows[rowIndex] = ListInsert(rows[rowIndex], colIndex, cki.KeyChar);
                                }
                            }
                            else
                            {
                                if (rows[rowIndex].Count - 1 < 79)
                                    rows[rowIndex].Add(cki.KeyChar);
                                else
                                    rows[rowIndex][colIndex] = cki.KeyChar;
                            }
                            break;
                        case InputMode.Override:
                            if (colIndex < rows[rowIndex].Count)
                            {
                                rows[rowIndex][colIndex] = cki.KeyChar;
                            }
                            else
                            {
                                {
                                    //rows[rowIndex].Insert(colIndex, cki.KeyChar);
                                    rows[rowIndex] = ListInsert(rows[rowIndex], colIndex, cki.KeyChar);
                                }
                            }
                            break;
                    }
                    if (colIndex < 79)
                        colIndex++;
                    if (left < 79)
                        left++;
                    continue;
                }
                else
                {
                    switch (cki.Key)
                    {
                        case ConsoleKey.Backspace:
                            if (colIndex > 0)
                            {
                                colIndex--;
                                if (left > 0)
                                {
                                    left--;
                                }

                                {// TODO: !BUGGED rows[rowIndex].RemoveAt(index);
                                    rows[rowIndex] = ListRemoveAt(rows[rowIndex], colIndex);
                                }
                                saved = false;
                            }
                            break;
                        case ConsoleKey.C:
                            loop = false;
                            break;
                        case ConsoleKey.Delete:
                            if (colIndex < rows[rowIndex].Count)
                            {
                                {// TODO: !BUGGED rows[rowIndex].RemoveAt(index);
                                    rows[rowIndex] = ListRemoveAt(rows[rowIndex], colIndex);
                                }
                                saved = false;
                            }
                            break;
                        case ConsoleKey.DownArrow:
                            if (rowIndex < rows.Count - 1)
                            {
                                if (top < 23)
                                {
                                    top++;
                                    rowIndex++;
                                }
                                else
                                {
                                    if (rowIndex < rows.Count - 1)
                                    {
                                        {
                                            //ScrollConsoleUpOne();
                                            int j = 0;
                                            for (int ii = rowIndex - 22; ii < rows.Count - 1; ii++)
                                            {
                                                SetCursor(j, 0);
                                                Write(rows[ii]);
                                                if (++j > 22)
                                                    break;
                                            }
                                        }

                                        rowIndex++;
                                        SetCursor(23, 0);
                                        Write(rows[rowIndex]);
                                    }
                                }
                            }
                            break;
                        case ConsoleKey.End:
                            SetCursor(top, rows[rowIndex].Count);
                            colIndex = rows[rowIndex].Count;
                            left = rows[rowIndex].Count;
                            break;
                        case ConsoleKey.Enter:
                            switch (InputMode)
                            {
                                case InputMode.Insert:
                                    if (colIndex == 0)
                                    {
                                        SetCursor(top, 0);
                                        Write(new string(' ', 80));

                                        {
                                            //rows.Insert(rowIndex, new List<char> { ' ' });
                                            rows = ListInsert(rows, rowIndex, new List<char> { ' ' });
                                        }

                                        {// TODO: BUGGED! : Console.MoveBufferArea(0, rowIndex + 1, 80, 23 - rowIndex, 0, rowIndex + 2);
                                            Console.Clear();
                                            for (int i = 0; i < rows.Count; i++)
                                            {
                                                Console.WriteLine(CharListToString(rows[i]));
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (colIndex < rows[rowIndex].Count)
                                        {
                                            List<char> c = null;
                                            {// TODO: BUGGED! List<char> c = rows[rowIndex].GetRange(index, rows[rowIndex].Count - index);
                                                c = GetRange(rows[rowIndex], colIndex, rows[rowIndex].Count - colIndex);
                                            }

                                            {// TODO: BUGGED! rows[rowIndex].RemoveRange(index, rows[rowIndex].Count - index);
                                                rows[rowIndex] = RemoveRange(rows[rowIndex], colIndex, rows[rowIndex].Count - colIndex);
                                            }

                                            if (rowIndex < rows.Count - 1)
                                            {
                                                {
                                                    //rows.Insert(rowIndex, c);
                                                    rows = ListInsert(rows, rowIndex + 1, c);
                                                }
                                            }
                                            else
                                            {
                                                rows.Add(c);
                                            }

                                            {// TODO: BUGGED! Console.MoveBufferArea(0, rowIndex + 1, 80, 23 - rowIndex, 0, rowIndex + 2);
                                                Console.Clear();
                                                for (int i = 0; i < rows.Count; i++)
                                                {
                                                    Console.WriteLine(CharListToString(rows[i]));
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (rowIndex < rows.Count - 1)
                                            {
                                                rows = ListInsert(rows, rowIndex + 1, (new List<char>()));
                                            }
                                            else
                                            {
                                                rows.Add(new List<char>());
                                            }

                                            {// TODO: BUGGED! Console.MoveBufferArea(0, rowIndex + 1, 80, 23 - rowIndex, 0, rowIndex + 2);
                                                Console.Clear();
                                                for (int i = 0; i < rows.Count; i++)
                                                {
                                                    Console.WriteLine(CharListToString(rows[i]));
                                                }
                                            }
                                        }
                                    }
                                    rowIndex++;
                                    top++;
                                    colIndex = 0;
                                    left = 0;
                                    SetCursor(top, 0);
                                    break;
                                case InputMode.Override:
                                    break;
                            }
                            saved = false;
                            break;
                        case ConsoleKey.Home:
                            colIndex = 0;
                            left = 0;
                            SetCursor(top, left);
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
                            if (colIndex > 0)
                                colIndex--;
                            if (left > 0)
                                left--;
                            break;
                        case ConsoleKey.RightArrow:
                            if (colIndex < rows[rowIndex].Count)
                            {
                                if (colIndex < 79)
                                    colIndex++;
                                if (left < 79)
                                    left++;
                            }
                            break;
                        case ConsoleKey.S:
                            File.WriteAllText(filename, CharListToString(rows));
                            saved = true;
                            break;
                        case ConsoleKey.Tab:
                            switch (InputMode)
                            {
                                case InputMode.Insert:
                                    for (int i = 0; i < 4; i++)
                                    {
                                        {
                                            //rows[rowIndex].Insert(colIndex, ' ');
                                            ListInsert(rows[rowIndex], colIndex, ' ');
                                        }
                                    }
                                    colIndex += 4;
                                    left += 4;
                                    saved = false;
                                    break;
                                case InputMode.Override:
                                    break;
                            }
                            break;
                        case ConsoleKey.UpArrow:
                            if (rowIndex > 0)
                            {
                                if (top > 0)
                                {
                                    top--;
                                    rowIndex--;
                                }
                                else
                                {
                                    //ScrollConsoleDownOne();
                                    rowIndex--;

                                    int j = 0;
                                    for (int ii = rowIndex; ii < rows.Count - 1; ii++)
                                    {
                                        SetCursor(j, 0);
                                        Write(rows[ii]);
                                        if (++j > 23)
                                            break;
                                    }

                                    SetCursor(0, 0);
                                }
                            }
                            break;
                    }
                }
            }

            return CharListToString(rows);
        }

        private void DisplayStatusBar(int t, int l, int rowCount)
        {
            SetCursor(24, 0);
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write(saved ? " " : "*");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            string s = "     Ln:" + Pad(rowIndex, 4) + "  Col:" + Pad(colIndex, 3) + "  Top:" + Pad(t, 2) + "  Left:" + Pad(l, 2) + "  " + (InputMode == InputMode.Insert ? "INS" : "OVR") + "  Rows " + rowCount;
            Console.Write(s);
            Console.ForegroundColor = ConsoleColor.White;
        }

        public List<List<char>> ListInsert(List<List<char>> list, int index, List<char> characters)
        {
            List<List<char>> newList = new List<List<char>>();
            int j = 0;

            for (; j < index; j++)
                newList.Add(list[j]);

            newList.Add(characters);

            for (; j < list.Count; j++)
                newList.Add(list[j]);

            return newList;
        }

        private List<char> RemoveRange(List<char> list, int index, int count)
        {
            List<char> newList = new List<char>();

            int i = 0;
            for (; i < index; i++)
                newList.Add(list[i]);

            i += count;
            for (; i < list.Count - 1; i++)
                newList.Add(list[i]);

            return newList;
        }

        private List<char> ListInsert(List<char> list, int index, char character)
        {
            List<char> newList = new List<char>();
            int i = 0;

            for (; i < index; i++)
                newList.Add(list[i]);

            newList.Add(character);

            for (; i < list.Count; i++)
                newList.Add(list[i]);

            return newList;
        }

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

        private List<char> GetRange(List<char> list, int index, int count)
        {
            List<char> newList = new List<char>();
            for (int i = index; i < list.Count; i++)
                newList.Add(list[i]);

            return newList;
        }

        private string Pad(int value, int length)
        {
            string r = value.ToString();
            for (int i = 0; i < 4 - r.Length; i++)
                r = "0" + r;
            return r;
        }

        private string CharListToString(List<List<char>> rows)
        {
            string sb = "";
            foreach (List<char> cs in rows)
            {
                foreach (char c in cs)
                    sb += c;
                sb += Environment.NewLine;
            }
            return sb;
        }

        private string CharListToString(List<char> list)
        {
            string s = "";
            foreach (char c in list)
                s += c;
            return s;
        }
    }
}
