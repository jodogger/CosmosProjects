using Cosmos.System.FileSystem.Listing;
using Clios.CommandProcessor.Commands;
using System;
using System.Collections.Generic;
using System.IO;

namespace Clios.CommandProcessor
{
    public class Processor
    {
        static ConsoleTextInput textInput = new ConsoleTextInput();
        static List<string> batchExtentions = new List<string> { ".bat", ".cmd" };

        static Processor()
        {
            // TODO: Use reflection and attributes?
            CommandsManager.Add(new Cd());
            CommandsManager.Add(new Cls());
            CommandsManager.Add(new Color());
            CommandsManager.Add(new Del());
            CommandsManager.Add(new Dir());
            CommandsManager.Add(new Echo());
            CommandsManager.Add(new Edit());
            CommandsManager.Add(new MkDir());
            CommandsManager.Add(new MkFile());
            CommandsManager.Add(new Reboot());
            CommandsManager.Add(new Rem());
            CommandsManager.Add(new RmDir());
            CommandsManager.Add(new Set());
            CommandsManager.Add(new Shutdown());
            CommandsManager.Add(new Commands.Type());
        }

        public static void GetCommand()
        {
            List<DirectoryEntry> directoryContents = new List<DirectoryEntry>();
            foreach (DirectoryEntry de in Global.FileSystem.GetDirectoryListing(Global.CurrentPath))
                directoryContents.Add(de);

            string[] parms = Split(textInput.GetText(directoryContents), false);
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
            BaseCommand cmd = CommandsManager.FindCommand(parms[0]);
            if (cmd == null)
            {
                if(!FindBatchFile(parms))
                {
                    Console.WriteLine("Unable to find command '" + parms[0] + "'");
                }
            }
            else
            {
                ProcessCommand(cmd, parms);
            }
        }

        private static void ProcessCommand(BaseCommand command, string[] parms)
        {
            if (CommandsManager.ValidateParams(command, parms))
            {
                CommandsManager.Execute(command, parms);
                DisplayCommandResult(command.CommandResult);
            }
            else
            {
                DisplayCommandResult(command.CommandResult);
            }
        }

        private static bool FindBatchFile(string[] parms)
        {
            foreach (DirectoryEntry de in Global.FileSystem.GetDirectoryListing(Global.CurrentPath))
            {
                // !BUGGED!  if (batchExtentions.Contains(de.mName.Substring(de.mName.Length - 4)))
                {
                    foreach (string s in batchExtentions)
                    {
                        if (s == de.mName.Substring(de.mName.Length - 4))
                        {
                            {
                                if (de.mName.ToLower().StartsWith(parms[0]))
                                {
                                    ProcessBatchFile(de.mName);
                                    return true;
                                }
                            }
                        }
                    }
                }
            }
            return false;
        }

        private static void ProcessBatchFile(string filename)
        {
            string[] lines = File.ReadAllLines(Path.Combine(Global.CurrentPath, filename));
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

        private static void DisplayCommandResult(CommandResult commandResult)
        {
            if (commandResult.ClearScreen)
            {
                Console.Clear();
                return;
            }

            if (commandResult.Success)
            {
                foreach (string s in commandResult.SuccessMsg)
                    Console.WriteLine(s);
            }
            else
            {
                foreach (string s in commandResult.ErrorMsg)
                    Console.WriteLine(s);
            }

            if (commandResult.Exception != null)
                Console.WriteLine("Exception: " + commandResult.Exception.Message);
        }

        private static string[] Split(string value, bool eatStartingSpaces)
        {
            List<string> p = new List<string>();
            string s = "";
            int cnt = 0;
            bool inQuotes = false;
            bool startingSpaces = true;

            foreach (char c in value)
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

        private static void DisplayHelp(string[] parms)
        {
            Console.WriteLine("");

            if (parms.Length > 1)
            {
                CommandsManager.DisplayCommandHelp(parms[1]);
                //BaseCommand cmd = CommandsManager.FindCommand(parms[1]);
                //if(cmd != null)
                //{
                //    Console.WriteLine(cmd.Help);
                //}
            }
            else
            {
                Console.WriteLine("For more information on a specific command, type 'HELP <command>'.");
                Console.WriteLine("");
                CommandsManager.DisplayAllCommandsHelp();
            }

            Console.WriteLine("");
        }
    }
}
