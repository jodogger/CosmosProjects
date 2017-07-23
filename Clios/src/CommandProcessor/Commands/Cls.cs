using System;

namespace Clios.CommandProcessor.Commands
{
    public class Cls : BaseCommand
    {
        public Cls()
        {
            Name = "cls";
            Description = "Clear the screen.";
            ParameterCount = 1;
        }

        public override void Do(params string[] args)
        {
            CommandResult.ClearScreen = true;
        }
        
        public override BaseCommand Create()
        {
            return new Cls();
        }
    }
}
