namespace Kernel.CommandProcessor.Commands
{
    public class Rem : BaseCommand
    {
        public Rem()
        {
            name = "rem";
            description = "Comment a line.";
            parameterCount = 2;
            variableParms = true;
        }

        public override void Do(params string[] args)
        {
        }

        public override BaseCommand Create()
        {
            return new Rem();
        }
    }
}
