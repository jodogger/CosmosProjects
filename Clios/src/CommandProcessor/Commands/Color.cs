using System;

namespace Clios.CommandProcessor.Commands
{
    public class Color : BaseCommand
    {
        public Color()
        {
            Name = "color";
            Description = "Display current time.";
            ParameterCount = 2;
            Help = "COLOR [attr]\n" +
                   "\n" +
                   "Attr is two hex digits: \n" +
                   "\n" +
                   "0 = Black              8 = Dark Gray\n" +
                   "1 = Dark Blue          9 = Blue\n" +
                   "2 = Dark Green         A = Green\n" +
                   "3 = Dark Cyan          B = Cyan\n" +
                   "4 = Dark Red           C = Red\n" +
                   "5 = Dark Magenta       D = Magenta\n" +
                   "6 = Dark Yellow        E = Yellow\n" +
                   "7 = Gray               F = White\n";
        }

        // TODO: Update to use CommandResult instead of Console.
        public override void Execute(params string[] args)
        {
            if (args[1].Length == 2)
            {
                string s = args[1];
                ConsoleColor f = GetColor(s.Substring(0, 1));
                ConsoleColor b = GetColor(s.Substring(1, 1));

                Console.ForegroundColor = f;
                Console.BackgroundColor = b;
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine("Attr must be two hex characters (0-E).");
                Console.WriteLine();
            }
        }
        
        public override BaseCommand Create()
        {
            return new Color();
        }

        private ConsoleColor GetColor(string c)
        {
            switch (c.ToUpper())
            {
                case "0":
                    return ConsoleColor.Black;
                case "1":
                    return ConsoleColor.DarkBlue;
                case "2":
                    return ConsoleColor.DarkGreen;
                case "3":
                    return ConsoleColor.DarkCyan;
                case "4":
                    return ConsoleColor.DarkRed;
                case "5":
                    return ConsoleColor.DarkMagenta;
                case "6":
                    return ConsoleColor.DarkYellow;
                case "7":
                    return ConsoleColor.Gray;
                case "8":
                    return ConsoleColor.DarkGray;
                case "9":
                    return ConsoleColor.Blue;
                case "A":
                    return ConsoleColor.Green;
                case "B":
                    return ConsoleColor.Cyan;
                case "C":
                    return ConsoleColor.Red;
                case "D":
                    return ConsoleColor.Magenta;
                case "E":
                    return ConsoleColor.Yellow;
                default:
                    return ConsoleColor.White;
            }
        }
    }
}

