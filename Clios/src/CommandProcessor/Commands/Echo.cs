using Clios.CommandProcessor.Variables;

namespace Clios.CommandProcessor.Commands
{
    public class Echo : BaseCommand
    {
        public Echo()
        {
            Name = "echo";
            Description = "Display text.";
            ParameterCount = 2;
            VariableParms = true;
        }

        public override void Execute(params string[] args)
        {
            string key = args[1];

            if (VariableManager.Contains(key))
            {
                Variable v = VariableManager.Get(key);
                CommandResult.AddSuccessMessage(v.Value.ToString());
            }
            else
            {
                CommandResult.AddSuccessMessage(key);
            }
        }

        public override BaseCommand Create()
        {
            return new Echo();
        }
    }
}
