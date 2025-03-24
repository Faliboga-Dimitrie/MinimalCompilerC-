using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinimalCompiler
{
    public class ProgramData
    {
        public enum Type
        {
            Int,
            Float,
            Double,
            String
        }

        public enum ReturnType
        {
            Int,
            Float,
            Double,
            String,
            Void
        }

        public class Variable
        {

            public Type VariableType { get; set; }
            public dynamic? Value { get; set; }

            public string Name { get; set; } = string.Empty;

            public string FromFunction { get; set; } = "";
        }
        public List<Variable> Variables { get; set; } = new List<Variable>();

        public string CurrentVariable { get; set; } = "";

        public class Function
        {
            public ReturnType ReturnType { get; set; }
            public string Name { get; set; } = string.Empty;
            public List<Variable> Parameters { get; set; } = new List<Variable>();

            public List<Variable> LocalVariables { get; set; } = new List<Variable> { };

            public bool currentFunction = false;
        }

        public List<Function> Functions { get; set; } = new List<Function>();

        public string CurrentFunction { get; set; } = "";

        public List<Variable> FunctionCallArgumentsExpressions { get; set; } = new List<Variable>();

        public dynamic? ExpresionValue { get; set; }

        public bool IsVariable(string name)
        {
            return Variables.Any(v => v.Name == name);
        }

        public Variable GetVariable(string name)
        {
            return Variables.First(v => v.Name == name);
        }

        public Function GetFunction(string name)
        {
            return Functions.FirstOrDefault(f => f.Name == name);
        }

        public Function GetFunctionCurrent(string name)
        {
            for (int i = 0; i < Functions.Count; i++)
            {
                if (Functions[i].Name == name && Functions[i].currentFunction == false)
                {
                    return Functions[i];
                }
            }
            return null;
        }

        public bool CheckFunctionParameters()
        {
            if (FunctionCallArgumentsExpressions.Count == 0)
            {
                return false;
            }

            Function function1 = GetFunction(CurrentFunction);

            if (function1 == null)
            {
                return false;
            }

            for (int i = 0; i < function1.Parameters.Count; i++)
            {
                if (function1.Parameters[i].VariableType == Type.Int && FunctionCallArgumentsExpressions[i].VariableType == Type.Int)
                {
                    continue;
                }
                else if (function1.Parameters[i].VariableType == Type.Float && FunctionCallArgumentsExpressions[i].VariableType == Type.Float)
                {
                    continue;
                }
                else if (function1.Parameters[i].VariableType == Type.Double && FunctionCallArgumentsExpressions[i].VariableType == Type.Double)
                {
                    continue;
                }
                else if (function1.Parameters[i].VariableType == Type.String && FunctionCallArgumentsExpressions[i].VariableType == Type.String)
                {
                    continue;
                }
                else
                {
                    return false;
                }
            }

            return true;
        }
    }
}
