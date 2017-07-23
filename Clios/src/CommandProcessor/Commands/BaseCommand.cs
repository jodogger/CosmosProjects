namespace Clios.CommandProcessor.Commands
{
    public abstract class BaseCommand
    {
        public string Name { get; protected set; }
        public string Description { get; protected set; }
        public string Help { get; protected set; }
        public bool VariableParms { get; protected set; } = false;
        public CommandResult CommandResult = new CommandResult();
        protected int ParameterCount = 0;

        public abstract void Do(params string[] args);

        public bool ValidateParams(string[] cmd)
        {
            if ((cmd.Length != ParameterCount && !VariableParms) || (VariableParms && (cmd.Length < ParameterCount)))
            {
                CommandResult.AddErrorMessage("Invalid params. Number should be '" + ParameterCount + "'");
                return false;
            }

            return true;
        }

        public abstract BaseCommand Create();
    }
}
