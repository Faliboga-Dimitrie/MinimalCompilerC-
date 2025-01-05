using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using System.Xml.Linq;


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

            for(int i = 0; i < function1.Parameters.Count; i++)
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

    public class MiniLanguageVisitor : MiniLanguageBaseVisitor<ProgramData>
    {
        private ProgramData _result = new ProgramData();

        public override ProgramData VisitProgram([Antlr4.Runtime.Misc.NotNull] MiniLanguageParser.ProgramContext context)
        {
            // Vizitează fiecare declarație sau altă secțiune din program
            foreach (var statement in context.children)
            {
                Visit(statement);  // Vizitează fiecare copil
            }
            return _result;
        }

        public override ProgramData VisitDeclaration([Antlr4.Runtime.Misc.NotNull] MiniLanguageParser.DeclarationContext context)
        {
            var variableName = context.VARIABLE_NAME().GetText();

            if(_result.IsVariable(variableName))
            {
                throw new Exception("Variable already exists.");
            }

            var variableType = ParseType(context.type());

            _result.Variables.Add(new ProgramData.Variable
            {
                Name = variableName,
                VariableType = variableType
            });

            if(_result.GetFunction("main") == null)
            {
                if(variableType == ProgramData.Type.String)
                {
                    _result.Variables.Last().Value = "";
                }
                else
                {
                    _result.Variables.Last().Value = 0;
                }
            }

            if (_result.CurrentFunction != "")
            {
                _result.GetFunction(_result.CurrentFunction).LocalVariables.Add(new ProgramData.Variable
                {
                    Name = variableName,
                    VariableType = variableType
                });

                _result.Variables.Last().FromFunction = _result.CurrentFunction;
            }

            return _result;
        }

        private ProgramData.Type ParseType(MiniLanguageParser.TypeContext context)
        {
            if (context.INTEGER_TYPE() != null) return ProgramData.Type.Int;
            if (context.FLOAT_TYPE() != null) return ProgramData.Type.Float;
            if (context.STRING_TYPE() != null) return ProgramData.Type.String;
            if (context.DOUBLE_TYPE() != null) return ProgramData.Type.Double;

            throw new Exception("Unknown variable type");
        }

        public override ProgramData VisitDeclaration_and_assignment([Antlr4.Runtime.Misc.NotNull] MiniLanguageParser.Declaration_and_assignmentContext context)
        {
            // 1. Preia tipul variabilei
            var variableType = ParseType(context.type());

            // 2. Preia numele variabilei
            var variableName = context.VARIABLE_NAME().GetText();

            if(_result.IsVariable(variableName))
            {
                throw new Exception("Variable already exists.");
            }

            _result.CurrentVariable = variableName;

            _result.Variables.Add(new ProgramData.Variable
            {
                Name = variableName,
                VariableType = variableType,
                Value = null
            });

            // 3. Preia valoarea atribuită variabilei
            var valueContext = context.value();
            var value = ParseValue(valueContext);

            // 4. Adaugă variabila și valoarea în rezultatul final
            if (_result.Variables.Last().Value == null)
            {
                _result.Variables.Last().Value = value;
            }

            if(_result.CurrentFunction != "")
            {
                _result.GetFunction(_result.CurrentFunction).LocalVariables.Add(new ProgramData.Variable
                {
                    Name = variableName,
                    VariableType = variableType,
                    Value = value
                });

                _result.Variables.Last().FromFunction = _result.CurrentFunction;
            }

            _result.CurrentVariable = "";
            return _result;
        }

        public override ProgramData VisitFunction_declaration([Antlr4.Runtime.Misc.NotNull] MiniLanguageParser.Function_declarationContext context)
        {
            int contor = 0;
            var functionName = context.VARIABLE_NAME().GetText();

            if(functionName == null)
            {
                throw new Exception("Function name not found.");
            }

            if(functionName == "main")
            {
                throw new Exception("Function name cannot be 'main'.");
            }

            var returnType = FunctionParseType(context.return_type());
            var blockContext = context.block();
            var blockNewLineContext = context.new_line_block();

            // Adaugă funcția la lista de funcții
            _result.Functions.Add(new ProgramData.Function
            {
                Name = functionName,
                ReturnType = returnType
            });

            // Vizitează parametrii
            var parametersContext = context.parameters();
            if (parametersContext != null)
            {
                // Iterăm prin parametri
                for (int i = 0; i < parametersContext.type().Count(); i++)
                {
                    var paramTypeContext = parametersContext.type()[i]; // Tipul parametrului
                    var paramNameContext = parametersContext.VARIABLE_NAME()[i]; // Numele parametrului

                    // Accesăm corect tipul parametrului
                    var paramType = ParseType(paramTypeContext);
                    var paramName = paramNameContext.GetText();

                    // Adăugăm parametrul la funcție
                    _result.Functions.Last().Parameters.Add(new ProgramData.Variable
                    {
                        Name = paramName,
                        VariableType = paramType
                    });

                    if (_result.GetFunctionCurrent("main") == null)
                    {
                        if (paramType == ProgramData.Type.String)
                        {
                            _result.Functions.Last().Parameters.Last().Value = "";
                        }
                        else
                        {
                            _result.Functions.Last().Parameters.Last().Value = 0;
                        }
                    }

                    _result.Functions.Last().LocalVariables.Add(new ProgramData.Variable
                    {
                        Name = paramName,
                        VariableType = paramType
                    });

                    if (_result.GetFunctionCurrent("main") == null)
                    {
                        if (paramType == ProgramData.Type.String)
                        {
                            _result.Functions.Last().LocalVariables.Last().Value = "";
                        }
                        else
                        {
                            _result.Functions.Last().LocalVariables.Last().Value = 0;
                        }
                    }

                    _result.Variables.Add(new ProgramData.Variable
                    {
                        Name = paramName,
                        VariableType = paramType
                    });

                    if (_result.GetFunctionCurrent("main") == null)
                    {
                        if (paramType == ProgramData.Type.String)
                        {
                            _result.Variables.Last().Value = "";
                        }
                        else
                        {
                            _result.Variables.Last().Value = 0;
                        }
                    }

                    contor++;
                }
            }

            _result.Functions.Last().currentFunction = true;
            int index = 0;
            bool sameSignature = false;

            if (_result.GetFunctionCurrent(functionName) != null)
            {
                ProgramData.Function function = _result.GetFunction(functionName);
                if (function.ReturnType == returnType && function.Parameters.Count == contor)
                {
                    sameSignature = true;
                    foreach (var variable in function.Parameters)
                    {
                        if(variable.VariableType != _result.Functions.Last().Parameters[index].VariableType)
                        {
                            sameSignature = false;
                        }

                        index++;
                    }
                }
            }

            if(sameSignature)
            {
                throw new Exception("Function already exists.");
            }

            _result.CurrentFunction = functionName;

            if (blockContext != null)
            {
                VisitBlock(blockContext);
            }
            else if (blockNewLineContext != null)
                VisitNew_line_block(blockNewLineContext);

            while (contor > 0)
            {
                _result.Variables.RemoveAt(_result.Variables.Count - 1);
                contor--;
            }

            _result.CurrentFunction = "";
            _result.Functions.Last().currentFunction = false;
            return _result;
        }

        public override ProgramData VisitMain_declaration([Antlr4.Runtime.Misc.NotNull] MiniLanguageParser.Main_declarationContext context)
        {
            var returnType = FunctionParseType(context.return_type());
            var functionName = context.MAIN_FUNCTION().GetText();
            var blockContext = context.block();
            var blockNewLineContext = context.new_line_block();

            // Adaugă funcția la lista de funcții
            _result.Functions.Add(new ProgramData.Function
            {
                Name = functionName,
                ReturnType = returnType
            });

            _result.CurrentFunction = functionName;

            if (blockContext != null)
            {
                VisitBlock(blockContext);
            }
            else if (blockNewLineContext != null)
                VisitNew_line_block(blockNewLineContext);

            _result.CurrentFunction = "";

            return _result;
        }

        public override ProgramData VisitDeclaration_and_assignment_no_semicolon([NotNull] MiniLanguageParser.Declaration_and_assignment_no_semicolonContext context)
        {
            // 1. Preia tipul variabilei
            var variableType = ParseType(context.type());

            // 2. Preia numele variabilei
            var variableName = context.VARIABLE_NAME().GetText();

            if (_result.IsVariable(variableName))
            {
                throw new Exception("Variable already exists.");
            }

            _result.CurrentVariable = variableName;

            _result.Variables.Add(new ProgramData.Variable
            {
                Name = variableName,
                VariableType = variableType,
                Value = null
            });

            // 3. Preia valoarea atribuită variabilei
            var valueContext = context.value();
            var value = ParseValue(valueContext);

            // 4. Adaugă variabila și valoarea în rezultatul final
            if (_result.Variables.Last().Value == null)
            {
                _result.Variables.Last().Value = value;
            }

            if (_result.CurrentFunction != "")
            {
                _result.GetFunction(_result.CurrentFunction).LocalVariables.Add(new ProgramData.Variable
                {
                    Name = variableName,
                    VariableType = variableType,
                    Value = value
                });

                _result.Variables.Last().FromFunction = _result.CurrentFunction;
            }

            _result.CurrentVariable = "";
            return _result;
        }

        public override ProgramData VisitAssignment_op_no_semicolon([NotNull] MiniLanguageParser.Assignment_op_no_semicolonContext context)
        {
            if (_result.GetVariable(context.VARIABLE_NAME().GetText()) == null)
            {
                throw new Exception("Variable not found.");
            }

            var variableName = _result.GetVariable(context.VARIABLE_NAME().GetText()).Name;
            if (context.value().VARIABLE_NAME() != null)
                _result.CurrentVariable = context.value().VARIABLE_NAME().GetText();
            else
                _result.CurrentVariable = variableName;

            if (context.ADD_EQUALS() != null)
            {
                _result.Variables.First(v => v.Name == variableName).Value += ParseValue(context.value());
            }
            else if (context.SUB_EQUALS() != null)
            {
                _result.Variables.First(v => v.Name == variableName).Value -= ParseValue(context.value());
            }
            else if (context.MUL_EQUALS() != null)
            {
                _result.Variables.First(v => v.Name == variableName).Value *= ParseValue(context.value());
            }
            else if (context.DIV_EQUALS() != null)
            {
                _result.Variables.First(v => v.Name == variableName).Value /= ParseValue(context.value());
            }

            _result.CurrentVariable = "";
            return _result;
        }

        public override ProgramData VisitPost_inccrement_or_decrement_no_semicolon([NotNull] MiniLanguageParser.Post_inccrement_or_decrement_no_semicolonContext context)
        {
            var variableName = context.VARIABLE_NAME().GetText(); // Extrage numele variabilei

            if(!_result.IsVariable(variableName))
            {
                throw new Exception("Variable not found.");
            }

            var newResult = _result;

            if (context.INC() != null)
            {
                newResult.Variables.First(v => v.Name == variableName).Value++;
            }
            else if (context.DEC() != null)
            {
                newResult.Variables.First(v => v.Name == variableName).Value--;
            }

            else
            {
                throw new Exception("Unknown operation.");

            }

            return newResult;
        }

        public override ProgramData VisitPrev_inccrement_or_decrement_no_semicolon([NotNull] MiniLanguageParser.Prev_inccrement_or_decrement_no_semicolonContext context)
        {
            var variableName = context.VARIABLE_NAME().GetText(); // Extrage numele variabilei

            if(!_result.IsVariable(variableName))
            {
                throw new Exception("Variable not found.");
            }

            if (context.INC() != null)
            {
                _result.Variables.First(v => v.Name == variableName).Value++;
            }
            else if (context.DEC() != null)
            {
                _result.Variables.First(v => v.Name == variableName).Value--;
            }
            else
            {
                throw new Exception("Unknown operation.");
            }

            return _result;
        }

        public override ProgramData VisitExpression([NotNull] MiniLanguageParser.ExpressionContext context)
        {
            return VisitLogical_expression(context.logical_expression());
        }

        public override ProgramData VisitLogical_expression([NotNull] MiniLanguageParser.Logical_expressionContext context)
        {
            if (context.relational_or_equality_expression().Length == 1)
            {
                return VisitRelational_or_equality_expression(context.relational_or_equality_expression()[0]);
            }
            else
            {
                ProgramData left = VisitRelational_or_equality_expression(context.relational_or_equality_expression()[0]);
                dynamic? first = left.ExpresionValue;
                ProgramData right = VisitRelational_or_equality_expression(context.relational_or_equality_expression()[1]);
                dynamic? second = right.ExpresionValue;

                if(first == null || second == null)
                {
                    throw new Exception("Expression values cannot be null.");
                }

                if (first is string || second is string)
                {
                    throw new Exception("A string can't be used in a logical operation.");
                }
                if (context.AND() != null)
                {
                    _result.ExpresionValue = first && second;
                    return _result;
                }
                else if (context.OR() != null)
                {
                    _result.ExpresionValue = first || second;
                    return _result;
                }
                else
                {
                    throw new Exception("Unknown operation.");
                }
            }
        }

        public override ProgramData VisitRelational_or_equality_expression([NotNull] MiniLanguageParser.Relational_or_equality_expressionContext context)
        {
            if (context.additive_or_subtractive_expression().Length == 1)
            {
                return VisitAdditive_or_subtractive_expression(context.additive_or_subtractive_expression()[0]);
            }
            else
            {
                ProgramData left = VisitAdditive_or_subtractive_expression(context.additive_or_subtractive_expression()[0]);
                dynamic? first = left.ExpresionValue;
                ProgramData right = VisitAdditive_or_subtractive_expression(context.additive_or_subtractive_expression()[1]);
                dynamic? second = right.ExpresionValue;

                if(first == null || second == null)
                {
                    throw new Exception("Expression values cannot be null.");
                }

                if (context.EQUAL() != null)
                {
                    _result.ExpresionValue = first == second;
                    return _result;
                }
                else if (context.NOT_EQUAL() != null)
                {
                    _result.ExpresionValue = first != second;
                    return _result;
                }
                else if (first is string || second is string)
                {
                    throw new Exception("It's not a native operation.Only == and != are.");
                }
                else if (context.GREATER_THAN() != null)
                {
                    _result.ExpresionValue = first > second;
                    return _result;
                }
                else if (context.GREATER_THAN_OR_EQUAL() != null)
                {
                    _result.ExpresionValue = first >= second;
                    return _result;
                }
                else if (context.LESS_THAN() != null)
                {
                    _result.ExpresionValue = first < second;
                    return _result;
                }
                else if (context.LESS_THAN_OR_EQUAL() != null)
                {
                    _result.ExpresionValue = first <= second;
                    return _result;
                }
                else
                {
                    throw new Exception("Unknown operation.");
                }
            }
        }

        public override ProgramData VisitAdditive_or_subtractive_expression([NotNull] MiniLanguageParser.Additive_or_subtractive_expressionContext context)
        {
            if (context.multiplicative_expression().Length == 1)
            {
                return VisitMultiplicative_expression(context.multiplicative_expression()[0]);
            }

            ProgramData left = VisitMultiplicative_expression(context.multiplicative_expression()[0]);
            dynamic? first2 = left.ExpresionValue;
            ProgramData right = VisitMultiplicative_expression(context.multiplicative_expression()[1]);
            dynamic? second2 = right.ExpresionValue;

            if(first2 == null || second2 == null)
            {
                throw new Exception("Expression values cannot be null.");
            }

            if (context.PLUS() != null)
            {
                if (first2 is string || second2 is string)
                {
                    if (first2.ToString() != null && second2.ExpresionValue.ToString() != null)
                    {
                        var first = first2.ExpresionValue.ToString();
                        var second = second2.ExpresionValue.ToString();
                        if (first is string && second is string)
                        {
                            _result.ExpresionValue = first + second;
                        }
                        else if (first is string)
                        {
                            _result.ExpresionValue = first + second.ToString();
                        }
                        else
                        {
                            _result.ExpresionValue = first.ToString() + second;
                        }
                    }
                    else
                    {
                        throw new Exception("Expression values cannot be null.");
                    }
                }
                else
                {
                    _result.ExpresionValue = first2 + second2;
                }
            }
            else if (context.MINUS() != null)
            {
                if (first2 is string || second2 is string)
                {
                    throw new Exception("A string can't be subtracted.");
                }
                _result.ExpresionValue = first2 - second2;
            }
            else
            {
                throw new Exception("Unknown operation.");
            }

            return _result;
        }

        public override ProgramData VisitMultiplicative_expression([NotNull] MiniLanguageParser.Multiplicative_expressionContext context)
        {
            if (context.unary_expression().Length == 1)
            {
                return VisitUnary_expression(context.unary_expression()[0]);
            }
            else
            {
                ProgramData left = VisitUnary_expression(context.unary_expression()[0]);
                dynamic? first = left.ExpresionValue;
                ProgramData right = VisitUnary_expression(context.unary_expression()[1]);
                dynamic? second = right.ExpresionValue;

                if(first == null || second == null)
                {
                    throw new Exception("Expression values cannot be null.");
                }

                if (first is string || second is string)
                {
                    if (context.ASTERISK() != null || context.SLASH() != null)
                    {
                        throw new Exception("A string can't be multiplied or divided.");
                    }
                }

                if (context.ASTERISK() != null)
                {
                    _result.ExpresionValue = first * second;
                    return _result;
                }
                else if (context.SLASH() != null)
                {
                    _result.ExpresionValue = first / second;
                    return _result;
                }
                else
                {
                    throw new Exception("Unknown operation.");
                }
            }
        }

        public override ProgramData VisitUnary_expression([NotNull] MiniLanguageParser.Unary_expressionContext context)
        {
            ProgramData value = VisitPrimary_expression(context.primary_expression());

            if (value.ExpresionValue is string && context.NOT() == null && context.MINUS() == null)
            {
                return value;
            }
            else if (value.ExpresionValue is string && (context.NOT() != null || context.MINUS() != null))
            {
                throw new Exception("A string can't be negated or change it's sign.");
            }

            if (context.NOT() != null)
            {
                _result.ExpresionValue = !value.ExpresionValue;
                return _result;
            }
            else if (context.MINUS() != null)
            {
                _result.ExpresionValue = -value.ExpresionValue;
                return _result;
            }
            else
            {
                return value;
            }
        }

        public override ProgramData VisitPrimary_expression([NotNull] MiniLanguageParser.Primary_expressionContext context)
        {
            if (context.VARIABLE_NAME() != null)
            {
                if (!_result.IsVariable(context.VARIABLE_NAME().GetText()))
                {
                    throw new Exception("Variable not found.");
                }
                _result.ExpresionValue = _result.GetVariable(context.VARIABLE_NAME().GetText()).Value;
                return _result;
            }
            else if (context.numeral_value() != null)
            {
                if (context.numeral_value().INTEGER_VALUE() != null)
                {
                    _result.ExpresionValue = int.Parse(context.numeral_value().INTEGER_VALUE().GetText());
                    return _result;
                }
                else if (context.numeral_value().FLOAT_VALUE() != null)
                {
                    _result.ExpresionValue = float.Parse(context.numeral_value().FLOAT_VALUE().GetText());
                    return _result;
                }
                else if (context.numeral_value().DOUBLE_VALUE() != null)
                {
                    _result.ExpresionValue = double.Parse(context.numeral_value().DOUBLE_VALUE().GetText());
                    return _result;
                }
                else
                {
                    throw new Exception("Unknown numeral value.");
                }
            }
            else
            {
                _result.ExpresionValue = context.STRING_VALUE().GetText().Trim('"');
                return _result;
            }
        }

        public override ProgramData VisitFor_statement([NotNull] MiniLanguageParser.For_statementContext context)
        {
            var assignmentContext = context.assignment_no_semicolon();
            var assignmentContextDeclaration = context.declaration_and_assignment_no_semicolon();
            var expressionContext = context.expression();
            var assignmentOperation = context.assignment_op_no_semicolon();
            var prevIncrementOrDecrement = context.prev_inccrement_or_decrement_no_semicolon();
            var postIncrementOrDecrement = context.post_inccrement_or_decrement_no_semicolon();

            bool hasLocalVariables = false;

            if (assignmentContext != null)
            {
                VisitAssignment_no_semicolon(assignmentContext);
                hasLocalVariables = true;
            }
            else if (assignmentContextDeclaration != null)
            {
                VisitDeclaration_and_assignment_no_semicolon(assignmentContextDeclaration);
                hasLocalVariables = true;
            }

            ProgramData forClause = VisitExpression(expressionContext);

            if (!(forClause.ExpresionValue is bool))
            {
                throw new Exception("For clause must evaluate to a boolean.");
            }

            while ((bool)forClause.ExpresionValue)
            {
                if (context.block() != null)
                {
                    VisitBlock(context.block());
                }
                else if (context.new_line_block() != null)
                {
                    VisitNew_line_block(context.new_line_block());
                }

                if (assignmentOperation != null)
                {
                    VisitAssignment_op_no_semicolon(assignmentOperation);
                }
                else if (postIncrementOrDecrement != null)
                {
                    VisitPost_inccrement_or_decrement_no_semicolon(postIncrementOrDecrement);
                }
                else if (prevIncrementOrDecrement != null)
                {
                    VisitPrev_inccrement_or_decrement_no_semicolon(prevIncrementOrDecrement);
                }
                forClause = VisitExpression(expressionContext);
            }

            if(hasLocalVariables)
            {
                _result.Variables.RemoveAt(_result.Variables.Count - 1);
            }

            return _result;
        }

        public override ProgramData VisitAssignment_op([NotNull] MiniLanguageParser.Assignment_opContext context)
        {
            if(_result.GetVariable(context.VARIABLE_NAME().GetText()) == null)
            {
                throw new Exception("Variable not found.");
            }

            var variableName = _result.GetVariable(context.VARIABLE_NAME().GetText()).Name;
            if(context.value().VARIABLE_NAME() != null)
                _result.CurrentVariable = context.value().VARIABLE_NAME().GetText();
            else
                _result.CurrentVariable = variableName;

            if (context.ADD_EQUALS() != null)
            {
                _result.Variables.First(v => v.Name == variableName).Value += ParseValue(context.value());
            }
            else if (context.SUB_EQUALS() != null)
            {
                _result.Variables.First(v => v.Name == variableName).Value -= ParseValue(context.value());
            }
            else if (context.MUL_EQUALS() != null)
            {
                _result.Variables.First(v => v.Name == variableName).Value *= ParseValue(context.value());
            }
            else if (context.DIV_EQUALS() != null)
            {
                _result.Variables.First(v => v.Name == variableName).Value /= ParseValue(context.value());
            }

            _result.CurrentVariable = "";
            return _result;
        }

        public override ProgramData VisitStatement([NotNull] MiniLanguageParser.StatementContext context)
        {
            if (context.assignment() != null)
            {
                return VisitAssignment(context.assignment());
            }
            else if (context.assignment_op() != null)
            {
                return VisitAssignment_op(context.assignment_op());
            }
            else if (context.declaration() != null)
            {
                return VisitDeclaration(context.declaration());
            }
            else if (context.declaration_and_assignment() != null)
            {
                return VisitDeclaration_and_assignment(context.declaration_and_assignment());
            }
            else if (context.function_call() != null)
            {
                VisitFunction_call(context.function_call());
            }
            else if (context.if_statement() != null)
            {
                return VisitIf_statement(context.if_statement());
            }
            else if (context.while_statement() != null)
            {
                return VisitWhile_statement(context.while_statement());
            }
            else if (context.for_statement() != null)
            {
                return VisitFor_statement(context.for_statement());
            }
            else if (context.return_statement() != null)
            {
                return VisitReturn_statement(context.return_statement());
            }
            else
            {
                throw new Exception("Unkown statement");
            }

            return _result;
        }

        public override ProgramData VisitReturn_statement([NotNull] MiniLanguageParser.Return_statementContext context)
        {
            //if(_result.CurrentFunction != "")
            //{
            //    return _result;
            //}
            //ProgramData hihihaha = VisitExpression(context.expression());
            //if (_result.GetFunction(_result.CurrentFunction) != null)
            //{
            //    _result.CalculatedFunctionValues[_result.CurrentFunction] = hihihaha.ExpresionValue;
            //} Need to rethink this
            return _result;
        }

        public override ProgramData VisitBlock([NotNull] MiniLanguageParser.BlockContext context)
        {
            var ListOfStatements = context.statement();

            if (ListOfStatements != null)
            {
                foreach (var statement in ListOfStatements)
                {
                    VisitStatement(statement);
                }
            }

            return _result;
        }

        public override ProgramData VisitFunction_call([NotNull] MiniLanguageParser.Function_callContext context)
        {
            if (_result.GetFunction(context.VARIABLE_NAME().GetText()) == null)
            {
                throw new Exception("Function not found.");
            }
            _result.CurrentFunction = context.VARIABLE_NAME().GetText();
            //return VisitArguments(context.arguments());
            return _result;
        }

        public override ProgramData VisitFunction_call_no_semicolon([NotNull] MiniLanguageParser.Function_call_no_semicolonContext context)
        {
            if (_result.GetFunction(context.VARIABLE_NAME().GetText()) == null)
            {
                throw new Exception("Function not found.");
            }
            //_result.CurrentFunction = context.VARIABLE_NAME().GetText();
            return _result;
        }

        public override ProgramData VisitArguments([NotNull] MiniLanguageParser.ArgumentsContext context)
        {
            // Curăță argumentele anterioare din lista de argumente
            if (_result.FunctionCallArgumentsExpressions.Count != 0)
            {
                _result.FunctionCallArgumentsExpressions.Clear();
            }

            // Verifică dacă există variabile
            if (context.VARIABLE_NAME().Length > 0)
            {
                foreach (var variableName in context.VARIABLE_NAME())
                {
                    // Verifică dacă variabila există
                    if (!_result.IsVariable(variableName.GetText()))
                    {
                        throw new Exception($"Variable '{variableName.GetText()}' not found.");
                    }

                    // Adaugă valoarea variabilei în lista de argumente
                    _result.FunctionCallArgumentsExpressions.Add(_result.GetVariable(variableName.GetText()));
                }
            }
            else
            {
                // Dacă nu sunt argumente de tip variabilă
                throw new Exception("Unknown argument type.");
            }

            // Verifică dacă parametrii funcției se potrivesc cu argumentele apelului
            if (_result.CheckFunctionParameters())
            {
                return _result; // Parametrii se potrivesc, întoarce rezultatul
            }
            else
            {
                throw new Exception("Function parameters don't match.");
            }
        }

        public override ProgramData VisitNew_line_block([NotNull] MiniLanguageParser.New_line_blockContext context)
        {
            return VisitBlock(context.block());
        }

        public override ProgramData VisitIf_statement([NotNull] MiniLanguageParser.If_statementContext context)
        {
            
            ProgramData condition = VisitExpression(context.expression());
            if (!(condition.ExpresionValue is bool))
            {
                throw new Exception("Condition in if statement must evaluate to a boolean.");
            }

            if ((bool)condition.ExpresionValue)
            {
                if (context.new_line_block().Length > 0)
                {
                    VisitNew_line_block(context.new_line_block(0));
                }
                else if (context.block().Length > 0)
                {
                    VisitBlock(context.block(0));
                }
            }
            else if (context.ELSE() != null)
            {
                if (context.new_line_block().Length > 1)
                {
                    VisitNew_line_block(context.new_line_block(1));
                }
                else if (context.block().Length > 1)
                {
                    VisitBlock(context.block(1));
                }
            }

            return _result;
        }

        public override ProgramData VisitWhile_statement([NotNull] MiniLanguageParser.While_statementContext context)
        {
            ProgramData condition = VisitExpression(context.expression());
            if (!(condition.ExpresionValue is bool))
            {
                throw new Exception("Condition in while statement must evaluate to a boolean.");
            }

            while (condition.ExpresionValue)
            {
                VisitBlock(context.block());
                condition = VisitExpression(context.expression());
            }

            return _result;
        }

        private ProgramData.ReturnType FunctionParseType(MiniLanguageParser.Return_typeContext context)
        {
            if (context.VOID_TYPE() != null) return ProgramData.ReturnType.Void;
            var type = ParseType(context.type());
            if (type == ProgramData.Type.Int) return ProgramData.ReturnType.Int;
            if (type == ProgramData.Type.Float) return ProgramData.ReturnType.Float;
            if (type == ProgramData.Type.String) return ProgramData.ReturnType.String;
            if (type == ProgramData.Type.Double) return ProgramData.ReturnType.Double;
            throw new Exception("Unknown return type");
        }

        private dynamic ParseValue(MiniLanguageParser.ValueContext valueContext)
        {
            if (valueContext.STRING_VALUE() != null)
            {
                if(_result.GetVariable(_result.CurrentVariable).VariableType != ProgramData.Type.String)
                {
                    throw new Exception("Variable is not a string.");
                }
                return valueContext.STRING_VALUE().GetText().Trim('"');
            }
            else if (valueContext.expression() != null)
            {
                var expressionResult = Visit(valueContext.expression());
                if (expressionResult.ExpresionValue != null)
                {
                    return expressionResult.ExpresionValue;
                }
                else
                {
                    throw new Exception("Unknown expression value.");
                }
            }
            else if (valueContext.VARIABLE_NAME() != null)
            {
                if (!_result.IsVariable(valueContext.VARIABLE_NAME().GetText()))
                {
                    throw new Exception($"Variable '{valueContext.VARIABLE_NAME().GetText()}' not found.");
                }

                ProgramData.Type variableType = _result.GetVariable(valueContext.VARIABLE_NAME().GetText()).VariableType;
                ProgramData.Type currentVariableType = _result.GetVariable(_result.CurrentVariable).VariableType;

                if (variableType == ProgramData.Type.String && currentVariableType != ProgramData.Type.String ||
                    variableType != ProgramData.Type.String && currentVariableType == ProgramData.Type.String)
                {
                    throw new Exception($"Type required {currentVariableType}; Type found {variableType}");
                }

                return _result.GetVariable(valueContext.VARIABLE_NAME().GetText()).Value;
            }
            else if (valueContext.function_call_no_semicolon() != null)
            {
                return VisitFunction_call_no_semicolon(valueContext.function_call_no_semicolon());
            }
            else if (valueContext.numeral_value().FLOAT_VALUE() != null)
            {
                ProgramData.Type currentVariableType = _result.GetVariable(_result.CurrentVariable).VariableType;

                if (currentVariableType == ProgramData.Type.String)
                {
                    throw new Exception($"Type required {currentVariableType}; Type found {ProgramData.Type.String}");
                }

                return float.Parse(valueContext.numeral_value().FLOAT_VALUE().GetText());
            }
            else if (valueContext.numeral_value().INTEGER_VALUE() != null)
            {
                ProgramData.Type currentVariableType = _result.GetVariable(_result.CurrentVariable).VariableType;

                if (currentVariableType == ProgramData.Type.String)
                {
                    throw new Exception($"Type required {currentVariableType}; Type found {ProgramData.Type.String}");
                }

                return int.Parse(valueContext.numeral_value().INTEGER_VALUE().GetText());
            }
            else
            {
                throw new Exception("Valoare necunoscută.");
            }
        }

        public override ProgramData VisitAssignment_no_semicolon([NotNull] MiniLanguageParser.Assignment_no_semicolonContext context)
        {
            var variableName = context.VARIABLE_NAME().GetText(); // Extrage numele variabilei
            if (variableName == null)
            {
                throw new Exception("Variable not found.");
            }
            var value = Visit(context.value()); // Apelează vizitatorul pentru valoare

            _result.GetVariable(variableName).Value = value.ExpresionValue;

            return _result;
        }

        public override ProgramData VisitAssignment([NotNull] MiniLanguageParser.AssignmentContext context)
        {
            var variableName = context.VARIABLE_NAME().GetText(); // Extrage numele variabilei
            if (variableName == null)
            {
                throw new Exception("Variable not found.");
            }
            var value = Visit(context.value()); // Apelează vizitatorul pentru valoare

            _result.GetVariable(variableName).Value = value.ExpresionValue;

            return _result;
        }
    }

    class Program
    {
        private static MiniLanguageLexer SetupLexer(string text)
        {
            AntlrInputStream inputStream = new AntlrInputStream(text);
            MiniLanguageLexer expressionLexer = new MiniLanguageLexer(inputStream);
            return expressionLexer;
        }

        private static MiniLanguageParser SetupParser(MiniLanguageLexer lexer)
        {
            CommonTokenStream commonTokenStream = new CommonTokenStream(lexer);
            MiniLanguageParser expressionParser = new MiniLanguageParser(commonTokenStream);
            return expressionParser;
        }

        private static void PrintLexems(MiniLanguageLexer lexer)
        {
            IList<IToken> tokens = lexer.GetAllTokens();
            lexer.Reset();

            Console.WriteLine("=== Lexems ===");
            Console.WriteLine("{0,-20} {1}", "Type", "Text");
            Console.WriteLine(new string('-', 40));

            foreach (IToken token in tokens)
            {
                Console.WriteLine("{0,-20} {1}",
                    lexer.Vocabulary.GetSymbolicName(token.Type),
                    token.Text);
            }

            Console.WriteLine(new string('=', 40));
        }

        static void Main(string[] args)
        {
            string input = File.ReadAllText("input.txt");

            // Setup lexer and parser
            MiniLanguageLexer lexer = SetupLexer(input);
            PrintLexems(lexer);
            MiniLanguageParser parser = SetupParser(lexer);

            // Parse input and use the visitor
            var tree = parser.program(); // 'program' este regula de top din gramatica
            MiniLanguageVisitor visitor = new MiniLanguageVisitor();
            ProgramData programData = visitor.Visit(tree);
            if(programData == null)
            {
                Console.WriteLine("Error parsing input");
                return;
            }

            Console.WriteLine("=== Variables from main or global ===");
            if (programData.Variables.Count > 0)
            {
                foreach (var variable in programData.Variables)
                {
                    Console.WriteLine($"- Type: {variable.VariableType}");
                    Console.WriteLine($"  Name: {variable.Name}");
                    if(variable.FromFunction != "")
                    {
                        Console.WriteLine($"  From function: {variable.FromFunction}");
                    }
                    else
                    {
                        Console.WriteLine($"  Global Variable");
                    }
                    Console.WriteLine($"  Value: {(variable.Value != null ? variable.Value : "none")}");
                }
            }
            else
            {
                Console.WriteLine("No variables collected.");
            }

            Console.WriteLine("\n=== Functions ===");
            if (programData.Functions.Count > 0)
            {
                foreach (var function in programData.Functions)
                {
                    Console.WriteLine($"\nFunction: {function.Name}");
                    Console.WriteLine($"  Return type: {function.ReturnType}");

                    Console.WriteLine("  Parameters:");
                    if (function.Parameters.Count > 0)
                    {
                        foreach (var parameter in function.Parameters)
                        {
                            Console.WriteLine($"    - Type: {parameter.VariableType}, Name: {parameter.Name}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("    None");
                    }

                    Console.WriteLine("  Local variables:");
                    if (function.LocalVariables.Count > 0)
                    {
                        foreach (var localVariable in function.LocalVariables)
                        {
                            Console.WriteLine($"    - Type: {localVariable.VariableType}, Name: {localVariable.Name}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("    None");
                    }
                }
            }
            else
            {
                Console.WriteLine("No functions collected.");
            }

        }

    }
}
