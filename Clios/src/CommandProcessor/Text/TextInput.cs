using Clios.Extentions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Clios.CommandProcessor.Text
{
    public class TextInput
    {
        const int TAB_SIZE = 4;
        const int MAX_WIDTH = 80;

        ITextInputIO textInputIO = null;
        InputMode inputMode = InputMode.Insert;
        bool processTab = false;

        public TextInput(ITextInputIO textInputIO, bool processTab = true)
        {
            this.textInputIO = textInputIO;
            this.processTab = processTab;
        }

        public TextInputResult GetText(string value)
        {
            List<char> result = StringToCharList(value);
            int index = 0;
            bool loop = true;
            int curTop = textInputIO.GetCursorTop();
            int curLeft = textInputIO.GetCursorLeft();
            int startLeft = textInputIO.GetCursorLeft();
            ConsoleKeyInfo cki = new ConsoleKeyInfo();

            inputMode = InputMode.Insert;
            textInputIO.ChangeCursorToInsert();

            while (loop)
            {
                textInputIO.SetCursor(curTop, curLeft);
                cki = textInputIO.ReadKey(true);
                if (cki.KeyChar > 31 && cki.KeyChar < 127)
                {
                    switch (inputMode)
                    {
                        case InputMode.Insert:
                            result = result.InsertAtX(index, cki.KeyChar);
                            break;
                        case InputMode.Override:
                            if (index < result.Count)
                            {
                                result[index] = cki.KeyChar;
                            }
                            else
                            {
                                result = result.InsertAtX(index, cki.KeyChar);
                            }
                            break;
                    }
                    index++;
                    curLeft++;
                    textInputIO.SetCursor(curTop, startLeft);
                    textInputIO.WriteLine(CharListToString(result));
                    continue;
                }

                switch (cki.Key)
                {
                    case ConsoleKey.Backspace:
                        if (index > 0)
                            result = result.RemoveAtX(index - 1);
                        else
                            continue;
                        index--;
                        curLeft--;
                        break;
                    case ConsoleKey.C:
                        result = new List<char>();
                        loop = false;
                        break;
                    case ConsoleKey.Delete:
                        if (index < result.Count)
                            result = result.RemoveAtX(index);
                        break;
                    case ConsoleKey.DownArrow:
                        loop = false;
                        break;
                    case ConsoleKey.End:
                        index = result.Count;
                        curLeft = startLeft + result.Count;
                        break;
                    case ConsoleKey.Enter:
                        loop = false;
                        break;
                    case ConsoleKey.Home:
                        index = 0;
                        curLeft = startLeft;
                        break;
                    case ConsoleKey.Insert:
                        if (inputMode == InputMode.Insert)
                        {
                            inputMode = InputMode.Override;
                            textInputIO.ChangeCursorToOverride();
                        }
                        else
                        {
                            inputMode = InputMode.Insert;
                            textInputIO.ChangeCursorToInsert();
                        }
                        break;
                    case ConsoleKey.LeftArrow:
                        if (index > 0)
                        {
                            index--;
                            curLeft--;
                        }
                        break;
                    case ConsoleKey.RightArrow:
                        if (index < result.Count)
                        {
                            index++;
                            curLeft++;
                        }
                        break;
                    case ConsoleKey.Tab:
                        if (processTab)
                        {
                            switch (inputMode)
                            {
                                case InputMode.Insert:
                                    if (index + TAB_SIZE < MAX_WIDTH)
                                    {
                                        for (int i = index; i < index + TAB_SIZE; i++)
                                        {
                                            result.InsertAtX(i, ' ');
                                        }
                                        index += TAB_SIZE;
                                        curLeft += TAB_SIZE;
                                    }
                                    break;
                                case InputMode.Override:
                                    if (index + TAB_SIZE < result.Count)
                                    {
                                        for (int i = index; i < index + TAB_SIZE; i++)
                                        {
                                            result[i] = ' ';
                                        }
                                        index += TAB_SIZE;
                                        curLeft += TAB_SIZE;
                                    }
                                    break;
                            }
                        }
                        else
                        {
                            loop = false;
                        }
                        break;
                    case ConsoleKey.UpArrow:
                        loop = false;
                        break;
                }

                textInputIO.SetCursor(curTop, startLeft);
                textInputIO.WriteLine(CharListToString(result));
            }

            TextInputResult textInputResult = new TextInputResult();
            textInputResult.ConsoleKeyInfo = cki;
            textInputResult.Result = CharListToString(result);
            textInputResult.CursorLeft = curLeft;
            textInputResult.InputMode = inputMode;

            return textInputResult;
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
