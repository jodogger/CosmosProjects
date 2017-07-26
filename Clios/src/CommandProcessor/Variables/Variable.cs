namespace Clios.CommandProcessor.Variables
{
    public class Variable
    {
        public string Name { get; set; }
        public VariableType Type { get; set; }
        public object Value { get; set; }
        public bool Environment { get; set; }
    }
}
