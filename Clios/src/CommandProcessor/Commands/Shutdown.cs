using Sys = Cosmos.System;

namespace Clios.CommandProcessor.Commands
{
    public class Shutdown : BaseCommand
    {
        public Shutdown()
        {
            Name = "shutdown";
            Description = "Shut down system.";
            ParameterCount = 1;
        }

        public override void Execute(params string[] args)
        {
            Sys.Power.Shutdown();
        }

        public override BaseCommand Create()
        {
            return new Shutdown();
        }
    }
}
