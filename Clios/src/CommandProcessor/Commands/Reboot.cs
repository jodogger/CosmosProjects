#if COSMOS
using Sys = Cosmos.System;
#else
using System;
#endif

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

        public override void Execute(params string[] args)
        {
#if COSMOS
            Sys.Power.Reboot();
#else
            Console.WriteLine("Rebooted");
#endif
        }

        public override BaseCommand Create()
        {
            return new Reboot();
        }
    }
}