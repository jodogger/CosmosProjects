using System;

namespace Kernel.CommandProcessor.Text
{
    public class ConsoleTextInputIO : ITextInputIO
    {
        public void Clear()
        {
            Console.Clear();
        }

        public void ChangeCursorToInsert()
        {
            Console.CursorSize = 25;
        }

        public void ChangeCursorToOverride()
        {
            Console.CursorSize = 100;
        }

        public int GetCursorLeft()
        {
            return Console.CursorLeft;
        }

        public int GetCursorTop()
        {
            return Console.CursorTop;
        }

        public ConsoleKeyInfo ReadKey(bool truncate)
        {
            return Console.ReadKey(true);
        }

        public void SetCursor(int top, int left)
        {
            Console.SetCursorPosition(left, top);
        }

        public void Write(string s)
        {
            Console.Write(s);
        }

        public void WriteLine(string v)
        {
            Console.Write(v);
            string pad = new string(' ', 80 - v.Length);
            Console.WriteLine(pad);
        }
    }
}
