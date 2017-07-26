namespace Clios.CommandProcessor.Commands
{
    public class Rem : BaseCommand
    {
        public Rem()
        {
            Name = "rem";
            Description = "Comment a line.";
            ParameterCount = 2;
            VariableParms = true;
        }

        public override void Execute(params string[] args)
        {
        }

        public override BaseCommand Create()
        {
            return new Rem();
        }
    }
}
