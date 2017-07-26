using Clios.CommandProcessor.Commands;
using System;
using System.Collections.Generic;

namespace Clios.CommandProcessor
{
    public class CommandsManager
    {
        static List<BaseCommand> commands = new List<BaseCommand>();

        public static BaseCommand FindCommand(string cmd)
        {
            foreach (BaseCommand c in commands)
            {
                if (c.Name == cmd.ToLower())
                    return c.Create(); // TODO: Replace with correct code
            }
            return null;
        }

        public static void Add(BaseCommand command)
        {
            foreach (BaseCommand c in commands)
            {
                if (c.Name == command.Name)
                {
                    return;
                }
            }
            commands.Add(command);
        }

        public static void DisplayAllCommandsHelp()
        {
            foreach (BaseCommand c in commands)
            {
                Console.WriteLine(c.Name.PadRight(15) + c.Description);
            }
        }

        public static void DisplayCommandHelp(BaseCommand command)
        {
            Console.WriteLine(command.Help);
        }

        public static void DisplayCommandHelp(string command)
        {
            BaseCommand cmd = CommandsManager.FindCommand(command);
            if(cmd != null)
            {
                Console.WriteLine(cmd.Help);
            }
        }

        public static void Execute(BaseCommand command, string[] parms)
        {
            command.Execute(parms);
        }

        public static bool ValidateParams(BaseCommand command, string[] parms)
        {
            return command.ValidateParams(parms);
        }
    }
}
