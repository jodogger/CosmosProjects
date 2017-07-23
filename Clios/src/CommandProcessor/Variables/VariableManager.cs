using System.Collections.Generic;

namespace Clios.src.CommandProcessor.Variables
{
    public class VariableManager
    {
        private static List<Variable> variables = new List<Variable>();

        public static void Add(Variable variable)
        {
            variables.Add(variable);
        }

        public static Variable Get(string name)
        {
            Variable var = null;

            foreach (Variable v in variables)
            {
                if (v.Name == name)
                {
                    var = v;
                    break;
                }
            }
            return var;
        }

        public static void Set(string name, object value)
        {
            foreach (Variable v in variables)
            {
                if (v.Name == name)
                {
                    v.Value = value;
                    return;
                }
            }
        }

        public static bool Contains(string name)
        {
            foreach (Variable v in variables)
                if (v.Name == name)
                    return true;
            return false;
        }

        public static void GetType(Variable v, string value)
        {
            try
            {
                int i = int.Parse(value.ToString());
                v.Type = VariableType.Int;
                v.Value = i;
                return;
            }
            catch
            {
            }

            //try
            //{
            //    double d = double.Parse(value.ToString());
            //    v.Type = VariableType.Double;
            //    v.Value = d;
            //    return;
            //}
            //catch
            //{
            //}

            //try
            //{
            //    bool b = bool.Parse(value.ToString());
            //    v.Type = VariableType.Bool;
            //    v.Value = b;
            //    return;
            //}
            //catch
            //{
            //}

            v.Type = VariableType.String;
            v.Value = value;
        }
    }
}
