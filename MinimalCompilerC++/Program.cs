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

        public Dictionary<string, dynamic> CalculatedFunctionValues { get; set; } = new Dictionary<string, dynamic>();

        public class Function
        {
            public ReturnType ReturnType { get; set; }
            public string Name { get; set; } = string.Empty;
            public List<Variable> Parameters { get; set; } = new List<Variable>();

            public List<Variable> LocalVariables { get; set; } = new List<Variable> { };
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
            return Functions.First(f => f.Name == name);
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
            var variableType = ParseType(context.type());

            _result.Variables.Add(new ProgramData.Variable
            {
                Name = variableName,
                VariableType = variableType
            });

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

            _result.CurrentVariable = "";
            return _result;
        }

        public override ProgramData VisitFunction_declaration([Antlr4.Runtime.Misc.NotNull] MiniLanguageParser.Function_declarationContext context)
        {
            int contor = 0;
            var functionName = context.VARIABLE_NAME().GetText();
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

                    _result.Variables.Add(new ProgramData.Variable
                    {
                        Name = paramName,
                        VariableType = paramType
                    });
                    contor++;
                }
            }

            _result.CalculatedFunctionValues.Add(functionName, null);
            _result.CurrentFunction = functionName;

            if (blockContext != null)
            {
                VisitBlock(blockContext);
            }
            else if (blockNewLineContext != null)
                VisitNew_line_block(blockNewLineContext);

            _result.CurrentFunction = "";
            while (contor > 0)
            {
                _result.Variables.RemoveAt(_result.Variables.Count - 1);
                contor--;
            }

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

            // 3. Preia valoarea atribuită variabilei
            var valueContext = context.value();
            var value = ParseValue(valueContext);

            // 4. Adaugă variabila și valoarea în rezultatul final
            _result.Variables.Add(new ProgramData.Variable
            {
                Name = variableName,
                VariableType = variableType,
                Value = value
            });

            return _result;
        }

        public override ProgramData VisitAssignment_op_no_semicolon([NotNull] MiniLanguageParser.Assignment_op_no_semicolonContext context)
        {
            var variableName = context.VARIABLE_NAME().GetText(); // Extrage numele variabilei

            if (context.ADD_EQUALS() != null)
            {
                _result.Variables.First(v => v.Name == variableName).Value += _result.Variables.First(v => v.Name == variableName).Value;
            }
            else if (context.SUB_EQUALS() != null)
            {
                _result.Variables.First(v => v.Name == variableName).Value -= _result.Variables.First(v => v.Name == variableName).Value;
            }
            else if (context.MUL_EQUALS() != null)
            {
                _result.Variables.First(v => v.Name == variableName).Value *= _result.Variables.First(v => v.Name == variableName).Value;
            }
            else if (context.DIV_EQUALS() != null)
            {
                _result.Variables.First(v => v.Name == variableName).Value /= _result.Variables.First(v => v.Name == variableName).Value;
            }

            return _result;
        }

        public override ProgramData VisitPost_inccrement_or_decrement_no_semicolon([NotNull] MiniLanguageParser.Post_inccrement_or_decrement_no_semicolonContext context)
        {
            var variableName = context.VARIABLE_NAME().GetText(); // Extrage numele variabilei

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

                if (left.ExpresionValue is string || right.ExpresionValue is string)
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
                else if (left.ExpresionValue is string || right.ExpresionValue is string)
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

            if (context.PLUS() != null)
            {
                if (left.ExpresionValue is string || right.ExpresionValue is string)
                {
                    if (left.ExpresionValue.ToString() != null && right.ExpresionValue.ToString() != null)
                    {
                        var first = left.ExpresionValue.ToString();
                        var second = right.ExpresionValue.ToString();
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
                if (left.ExpresionValue is string || right.ExpresionValue is string)
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

                if (left.ExpresionValue is string || right.ExpresionValue is string)
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
            var forClauseContext = context.for_clause();
            var forBodyContext = context.block();
            var ForBodyContextNewLine = context.new_line_block();

            if (forClauseContext != null)
            {
                VisitFor_clause(forClauseContext);
            }

            if (forBodyContext != null)
            {
                VisitBlock(forBodyContext);
            }

            if (ForBodyContextNewLine != null)
            {
                VisitNew_line_block(ForBodyContextNewLine);
            }

            return _result;
        }

        public override ProgramData VisitFor_clause([NotNull] MiniLanguageParser.For_clauseContext context)
        {
            var assignmentContext = context.assignment_no_semicolon();
            var assignmentContextDeclaration = context.declaration_and_assignment_no_semicolon();
            var expressionContext = context.expression();
            var assignmentOperation = context.assignment_op_no_semicolon();
            var prevIncrementOrDecrement = context.prev_inccrement_or_decrement_no_semicolon();
            var postIncrementOrDecrement = context.post_inccrement_or_decrement_no_semicolon();

            if (assignmentContext != null)
            {
                VisitAssignment_no_semicolon(assignmentContext);
            }
            else if (assignmentContextDeclaration != null)
            {
                VisitDeclaration_and_assignment_no_semicolon(assignmentContextDeclaration);
            }
            else if (expressionContext != null)
            {
                VisitExpression(expressionContext);
            }
            else if (assignmentOperation != null)
            {
                VisitAssignment_op_no_semicolon(assignmentOperation);
            }
            else if (prevIncrementOrDecrement != null)
            {
                VisitPrev_inccrement_or_decrement_no_semicolon(prevIncrementOrDecrement);
            }

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
            return VisitArguments(context.arguments());
        }

        public override ProgramData VisitFunction_call_no_semicolon([NotNull] MiniLanguageParser.Function_call_no_semicolonContext context)
        {
            if (_result.GetFunction(context.VARIABLE_NAME().GetText()) == null)
            {
                throw new Exception("Function not found.");
            }
            _result.CurrentFunction = context.VARIABLE_NAME().GetText();
            return VisitArguments(context.arguments());
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
                if (_result.CurrentFunction != "" && _result.CurrentVariable != "")
                {
                    _result.GetVariable(_result.CurrentVariable).Value = _result.CalculatedFunctionValues[_result.CurrentFunction];
                }
                _result.CurrentFunction = "";
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
            return base.VisitIf_statement(context);
        }

        public override ProgramData VisitWhile_statement([NotNull] MiniLanguageParser.While_statementContext context)
        {
            return base.VisitWhile_statement(context);
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
            else if (valueContext.function_call_no_semicolon() != null)
            {
                return VisitFunction_call_no_semicolon(valueContext.function_call_no_semicolon());
            }
            else if (valueContext.numeral_value().FLOAT_VALUE() != null)
            {
                return float.Parse(valueContext.numeral_value().FLOAT_VALUE().GetText());
            }
            else if (valueContext.numeral_value().INTEGER_VALUE() != null)
            {
                return int.Parse(valueContext.numeral_value().INTEGER_VALUE().GetText());
            }
            else if (valueContext.VARIABLE_NAME() != null)
            {
                return valueContext.VARIABLE_NAME().GetText();
            }
            else
            {
                throw new Exception("Valoare necunoscută.");
            }
        }

        public override ProgramData VisitAssignment_no_semicolon([NotNull] MiniLanguageParser.Assignment_no_semicolonContext context)
        {
            var variableName = context.VARIABLE_NAME().GetText(); // Extrage numele variabilei
            var value = Visit(context.value()); // Apelează vizitatorul pentru valoare

            Console.WriteLine($"Assignment: {variableName} = {value}"); // Debugging: afișează valoarea atribuită

            // Crează obiectul de variabilă și adaugă-l
            _result.Variables.Add(new ProgramData.Variable
            {
                Name = variableName,
                Value = value
            });

            return _result;
        }

        public override ProgramData VisitAssignment([NotNull] MiniLanguageParser.AssignmentContext context)
        {
            var variableName = context.VARIABLE_NAME().GetText(); // Extrage numele variabilei
            _result.CurrentVariable = variableName;
            var value = ParseValue(context.value()); // Apelează vizitatorul pentru valoare

            Console.WriteLine($"Assignment: {variableName} = {value}"); // Debugging: afișează valoarea atribuită

            // Crează obiectul de variabilă și adaugă-l
            _result.Variables.Add(new ProgramData.Variable
            {
                Name = variableName,
                Value = value
            });

            _result.CurrentVariable = "";

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

            Console.WriteLine("Lexems:");
            foreach (IToken token in tokens)
            {
                Console.WriteLine($"Type: " +
                    $"{lexer.Vocabulary.GetSymbolicName(token.Type)} -> " +
                    $"{token.Text}");
            }
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

            // Afiseaza variabilele colectate
            Console.WriteLine("Variables:");
            foreach (var variable in programData.Variables)
            {
                Console.WriteLine($"Type: {variable.VariableType}, Name: {variable.Name}, Value: {(variable.Value != null ? variable.Value : "none")}");
            }

            // Afiseaza functiile colectate
            Console.WriteLine("Functions:");
            foreach (var function in programData.Functions)
            {
                Console.WriteLine($"Return type: {function.ReturnType}, Name: {function.Name}");
                Console.WriteLine("Parameters:");
                foreach (var parameter in function.Parameters)
                {
                    Console.WriteLine($"Type: {parameter.VariableType}, Name: {parameter.Name}");
                }
                //Console.WriteLine("Local variable:");
                //foreach(var parameter in function)
            }
        }

    }
}
