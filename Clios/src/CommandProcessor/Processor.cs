using Cosmos.System.FileSystem.Listing;
using Clios.CommandProcessor.Commands;
using System;
using System.Collections.Generic;
using System.IO;

namespace Clios.CommandProcessor
{
    public class Processor
    {
        static List<BaseCommand> Commands = new List<BaseCommand>();
        static ConsoleTextInput TextInput = new ConsoleTextInput();
        static List<string> BatchExtentions = new List<string> { ".bat", ".cmd" };

        static Processor()
        {
            Commands.Add(new Cd());
            Commands.Add(new Cls());
            Commands.Add(new Color());
            Commands.Add(new Del());
            Commands.Add(new Dir());
            Commands.Add(new Echo());
            Commands.Add(new Edit());
            Commands.Add(new MkDir());
            Commands.Add(new MkFile());
            Commands.Add(new Reboot());
            Commands.Add(new Rem());
            Commands.Add(new RmDir());
            Commands.Add(new Set());
            Commands.Add(new Shutdown());
            Commands.Add(new Commands.Type());
        }

        public static void GetCommand()
        {
            List<DirectoryEntry> dirContents = new List<DirectoryEntry>();
            foreach (DirectoryEntry de in Global.FileSystem.GetDirectoryListing(Global.CurrentPath))
                dirContents.Add(de);

            string[] parms = Split(TextInput.GetText(dirContents), false);
            Console.WriteLine();

            switch (parms[0].ToLower())
            {
                case "help":
                    DisplayHelp(parms);
                    break;
                default:
                    ProcessParms(parms);
                    break;
            }
        }

        private static void ProcessParms(string[] parms)
        {
            BaseCommand cmd = FindCommand(parms[0]);

            if (cmd == null)
            {
                FindBatchOrCommandFile(parms);
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

        private static void FindBatchOrCommandFile(string[] parms)
        {
            foreach (DirectoryEntry de in Global.FileSystem.GetDirectoryListing(Global.CurrentPath))
            {
                // !BUGGED!  if (batchExtentions.Contains(de.mName.Substring(de.mName.Length - 4)))
                {
                    foreach (string s in BatchExtentions)
                    {
                        if (s == de.mName.Substring(de.mName.Length - 4))
                        {
                            {
                                if (de.mName.ToLower().StartsWith(parms[0]))
                                {
                                    ProcessBatchOrCommandFile(de.mName);
                                    return;
                                }
                            }
                        }
                    }
                }
            }
        }

        private static void ProcessBatchOrCommandFile(string file)
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
                    case "for":
                        //parms[1] = variable : must be int
                        //parms[2] = operator '='
                        //parms[3] = start
                        //parms[4] = 'to'
                        //parms[5] = end
                        curLine++;
                        break;
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
                        ProcessParms(parms);
                        curLine++;
                        break;
                }
            }
        }

        private static void DisplayCommandResult(CommandResult cr)
        {
            if (cr.ClearScreen)
            {
                Console.Clear();
                return;
            }

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
                    case '\"':
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

        private static void DisplayHelp(string[] parms)
        {
            Console.WriteLine("");

            if (parms.Length > 1)
            {
                BaseCommand cmd = FindCommand(parms[1]);
                if(cmd != null)
                {
                    Console.WriteLine(cmd.Help);
                }
            }
            else
            {
                Console.WriteLine("For more information on a specific command, type 'HELP <command>'.");
                Console.WriteLine("");
                foreach (BaseCommand c in Commands)
                {
                    Console.WriteLine(c.Name.PadRight(15) + c.Description);
                }
            }

            Console.WriteLine("");
        }
    }
}
