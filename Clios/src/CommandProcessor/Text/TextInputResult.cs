using System;

namespace Clios.CommandProcessor.Text
{
    public class TextInputResult
    {
        public ConsoleKeyInfo ConsoleKeyInfo { get; set; }
        public string Result { get; set; }
        public int CursorLeft { get; set; }
        public InputMode InputMode { get; set; }
    }
}
