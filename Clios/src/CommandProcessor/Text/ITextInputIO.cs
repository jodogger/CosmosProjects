using System;

namespace Clios.CommandProcessor.Text
{
    public interface ITextInputIO
    {
        void ChangeCursorToInsert();
        void ChangeCursorToOverride();
        int GetCursorLeft();
        int GetCursorTop();
        ConsoleKeyInfo ReadKey(bool truncate);
        void SetCursor(int top, int left);
        void Write(string s);
        void WriteLine(string v);
    }
}
