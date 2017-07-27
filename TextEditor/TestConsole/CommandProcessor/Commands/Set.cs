using Kernel.CommandProcessor.Variables;

namespace Kernel.CommandProcessor.Commands
{
    public class Set : BaseCommand
    {
        public Set()
        {
            name = "set";
            description = "Display contents of a text file.";
            parameterCount = 4;
        }

        public override void Do(params string[] args)
        {
            string name = args[1];
            string op = args[2];
            string value = args[3];

            if (op != "=")
            {
                CommandResult.AddErrorMessage("Error: Must use '=' to set value.");
                return;
            }

            if (VariableManager.Contains(name))
            {
                VariableManager.Set(name, value);
            }
            else
            {
                Variable nv = new Variable();
                nv.Name = name;
                nv.Environment = false;
                VariableManager.GetType(nv, value);
                VariableManager.Add(nv);
            }
        }

        public override BaseCommand Create()
        {
            return new Set();
        }
    }
}
