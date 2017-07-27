using Kernel.CommandProcessor.Commands;
using Kernel.CommandProcessor.Text;
using System;
using System.Collections.Generic;
using System.IO;

namespace Kernel.CommandProcessor
{
    public class Processor
    {
        static List<BaseCommand> Commands = new List<BaseCommand>();
        static TextInput textInput = null;
        static List<string> batchExtentions = new List<string> { ".bat", ".cmd" };

        static Processor()
        {
            Commands.Add(new Cd());
            Commands.Add(new Del());
            Commands.Add(new Dir());
            Commands.Add(new Echo());
            Commands.Add(new MkDir());
            Commands.Add(new MkFile());
            Commands.Add(new Rem());
            Commands.Add(new RmDir());
            Commands.Add(new Set());
            Commands.Add(new Commands.Type());
        }

        public static string GetCommand()
        {
            List<string> dirContents = new List<string>();
            foreach (string s in Directory.EnumerateFileSystemEntries(Global.CurrentPath))
                dirContents.Add(Path.GetFileName(s));

            ConsoleTextInputIO consoleTextInputIO = new ConsoleTextInputIO();
            textInput = new TextInput(consoleTextInputIO, false);

            TextInputResult tir = textInput.GetText("");
            if (tir.ConsoleKeyInfo.Key == ConsoleKey.Enter)
            {
                if (tir.Result.Length > 0)
                {
                    string[] parms = Split(tir.Result, false);
                    Console.WriteLine();

                    switch (parms[0].ToLower())
                    {
                        case "help":
                            DisplayHelp();
                            break;
                        default:
                            DetermineCommandType(parms);
                            break;
                    }
                }
            }
            else
            {
                // process special keys : TAB, Down\Up arrow, Function keys.
            }

            return tir.Result;
        }

        private static void DetermineCommandType(string[] parms)
        {
            BaseCommand cmd = FindCommand(parms[0]);
            if (cmd == null)
            {
                FindExecutable(parms);
            }
            else
            {
                ProcessCommand(cmd, parms);
            }
        }

        private static void ProcessCommand(BaseCommand cmd, string[] parms)
        {
            if (cmd.ValidateParams(parms))
            {
                cmd.Do(parms);
                DisplayCommandResult(cmd.CommandResult);
            }
            else
            {
                DisplayCommandResult(cmd.CommandResult);
            }
        }

        private static void FindExecutable(string[] parms)
        {
            foreach (string file in Directory.EnumerateFiles(Global.CurrentPath))
            {
                string s = Path.GetFileName(file);
                if (s.Length > 4)
                {
                    if (batchExtentions.Contains(s.Substring(s.Length - 4)))
                    {
                        if (s.ToLower().StartsWith(parms[0]))
                        {
                            ExecuteBatch(s);
                        }
                    }
                }
            }
        }

        private static void ExecuteBatch(string file)
        {
            string[] lines = File.ReadAllLines(Path.Combine(Global.CurrentPath, file));
            int curLine = 0;
            bool loop = true;

            while (loop && curLine < lines.Length)
            {
                string[] parms = Split(lines[curLine], true);

                if (parms.Length == 0 || parms[0][0] == ':')
                {
                    curLine++;
                    continue;
                }

                switch (parms[0].ToLower())
                {
                    case "goto":
                        if (parms.Length > 1)
                        {
                            int lc = 0;
                            foreach (string s in lines)
                            {
                                if (s[0] == ':')
                                {
                                    if (s.Substring(1) == parms[1])
                                    {
                                        curLine = lc;
                                        break;
                                    }
                                }
                                lc++;
                            }
                        }
                        break;
                    case "exit":
                        loop = false;
                        break;
                    default:
                        DetermineCommandType(parms);
                        curLine++;
                        break;
                }
            }
        }

        private static void DisplayCommandResult(CommandResult cr)
        {
            if (cr.Success)
            {
                foreach (string s in cr.SuccessMsg)
                    Console.WriteLine(s);
            }
            else
            {
                foreach (string s in cr.ErrorMsg)
                    Console.WriteLine(s);
            }

            if (cr.Exception != null)
                Console.WriteLine("Exception: " + cr.Exception.Message);
        }

        private static string[] Split(string v, bool eatStartingSpaces)
        {
            List<string> p = new List<string>();

            string s = "";
            int cnt = 0;
            bool inQuotes = false;
            bool startingSpaces = true;

            foreach (char c in v)
            {
                switch (c)
                {
                    case ' ':
                        if (eatStartingSpaces && startingSpaces)
                            continue;
                        if (inQuotes)
                        {
                            s += c;
                        }
                        else
                        {
                            p.Add(s);
                            s = "";
                            cnt++;
                        }
                        break;
                    case '\'':
                        if (!inQuotes)
                        {
                            inQuotes = true;
                        }
                        else
                        {
                            p.Add(s);
                            s = "";
                            cnt++;
                        }
                        startingSpaces = false;
                        break;
                    default:
                        s += c;
                        startingSpaces = false;
                        break;
                }
            }

            if (s.Trim().Length > 0)
            {
                p.Add(s.Trim());
                cnt++;
            }

            string[] result = new string[cnt];
            cnt = 0;
            foreach (string ss in p)
                result[cnt++] = ss;

            return result;
        }

        private static BaseCommand FindCommand(string cmd)
        {
            foreach (BaseCommand c in Commands)
            {
                if (c.Name == cmd.ToLower())
                    return c.Create();
            }
            return null;
        }

        private static void DisplayHelp()
        {
            Console.WriteLine("");
            Console.WriteLine("For more information on a specific command, type 'HELP <command>'.");
            Console.WriteLine("");
            foreach (BaseCommand c in Commands)
            {
                Console.WriteLine(c.Name.PadRight(15) + c.Description);
            }
            Console.WriteLine("");
        }
    }
}
