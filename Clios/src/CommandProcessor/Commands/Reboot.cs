using Sys = Cosmos.System;

namespace Clios.CommandProcessor.Commands
{
    public class Reboot : BaseCommand
    {
        public Reboot()
        {
            Name = "reboot";
            Description = "Reboot system.";
            ParameterCount = 1;
        }

        public override void Do(params string[] args)
        {
            Sys.Power.Reboot();
        }

        public override BaseCommand Create()
        {
            return new Reboot();
        }
    }
}