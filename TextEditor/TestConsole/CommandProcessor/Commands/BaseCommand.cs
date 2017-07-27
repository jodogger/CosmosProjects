namespace Kernel.CommandProcessor.Commands
{
    public abstract class BaseCommand
    {
        protected string name = "";
        protected int parameterCount = 0;
        protected string description = "";
        protected bool variableParms = false;
        public CommandResult CommandResult = new CommandResult();
        public string Name { get { return name; } }
        public string Description {  get { return description; } }
        public bool VariableParms {  get { return variableParms; } }

        public abstract void Do(params string[] args);

        public bool ValidateParams(string[] cmd)
        {
            if ((cmd.Length != parameterCount && !VariableParms) || (VariableParms && (cmd.Length < parameterCount)))
            {
                CommandResult.AddErrorMessage("Invalid params. Number should be '" + parameterCount + "'");
                return false;
            }

            return true;
        }

        public abstract BaseCommand Create();
    }
}
