using Kernel.CommandProcessor.Variables;

namespace Kernel.CommandProcessor.Commands
{
    public class Echo : BaseCommand
    {
        public Echo()
        {
            name = "echo";
            description = "Display text.";
            parameterCount = 2;
            variableParms = true;
        }

        public override void Do(params string[] args)
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
