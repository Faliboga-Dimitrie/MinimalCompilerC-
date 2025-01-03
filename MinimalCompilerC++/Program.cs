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
        }
        public List<Variable> Variables { get; set; } = new List<Variable>();

        public class Function
        {
            public ReturnType ReturnType { get; set; }
            public string Name { get; set; } = string.Empty;
            public List<Variable> Parameters { get; set; } = new List<Variable>();
        }

        public List<Function> Functions { get; set; } = new List<Function>();
    }

    public class MiniLanguageVisitor : MiniLanguageBaseVisitor<ProgramData>
    {
        private ProgramData _result = new ProgramData();
        private ProgramData _residualPostIncrement = new ProgramData();

        private ProgramData GetVariable(string variableName)
        {
            // Căutăm variabila în lista de variabile
            var variable = _result.Variables.FirstOrDefault(v => v.Name == variableName);

            if (variable == null)
            {
                // Dacă nu găsim variabila, aruncăm o excepție sau returnăm un obiect gol
                throw new Exception($"Variable '{variableName}' not found.");
            }

            // Creăm un obiect ProgramData și îi adăugăm variabila găsită
            var programData = new ProgramData();
            programData.Variables.Add(variable); // Adăugăm variabila la ProgramData

            return programData; // Returnăm obiectul ProgramData
        }

        private ProgramData PerformUnaryOperation(ProgramData expr, string operation)
        {
            var programData = new ProgramData();

            if (operation == "NOT")
            {
                if (expr.Variables[0].Value is bool value)
                {
                    programData.Variables.Add(new ProgramData.Variable
                    {
                        VariableType = ProgramData.Type.Int,
                        Value = value ? 0 : 1
                    });
                }
                else
                {
                    throw new Exception("NOT operation can be applied only to boolean values.");
                }
            }
            else if (operation == "NEGATE")
            {
                if (expr.Variables[0].Value is int intValue)
                {
                    programData.Variables.Add(new ProgramData.Variable
                    {
                        VariableType = ProgramData.Type.Int,
                        Value = -intValue
                    });
                }
                else if (expr.Variables[0].Value is float floatValue)
                {
                    programData.Variables.Add(new ProgramData.Variable
                    {
                        VariableType = ProgramData.Type.Float,
                        Value = -floatValue
                    });
                }
                else if (expr.Variables[0].Value is double doubleValue)
                {
                    programData.Variables.Add(new ProgramData.Variable
                    {
                        VariableType = ProgramData.Type.Double,
                        Value = -doubleValue
                    });
                }
                else
                {
                    throw new Exception("NEGATE operation can be applied only to numeric values.");
                }
            }

            return programData;
        }

        private ProgramData PerformLogicalOperation(ProgramData left, ProgramData right, string operation)
        {
            ProgramData programData = new ProgramData();

            if (operation == "AND")
            {
                if (left.Variables[0].Value is bool leftValue && right.Variables[0].Value is bool rightValue)
                {
                    programData.Variables.Add(new ProgramData.Variable
                    {
                        VariableType = ProgramData.Type.Int,
                        Value = leftValue && rightValue ? 1 : 0
                    });
                }
                else
                {
                    throw new Exception("AND operation can be applied only to boolean values.");
                }
            }
            else if (operation == "OR")
            {
                if (left.Variables[0].Value is bool leftValue && right.Variables[0].Value is bool rightValue)
                {
                    programData.Variables.Add(new ProgramData.Variable
                    {
                        VariableType = ProgramData.Type.Int,
                        Value = leftValue || rightValue ? 1 : 0
                    });
                }
                else
                {
                    throw new Exception("OR operation can be applied only to boolean values.");
                }
            }

            return programData;
        }

        private ProgramData PerformArithmeticOperation(ProgramData left, ProgramData right, string operation)
        {
            ProgramData programData = new ProgramData();

            // Functie generica pentru operatii aritmetice
            Func<object, object, double> applyOperation = (object l, object r) =>
            {
                switch (operation)
                {
                    case "ADD": return Convert.ToDouble(l) + Convert.ToDouble(r);
                    case "SUB": return Convert.ToDouble(l) - Convert.ToDouble(r);
                    case "MUL": return Convert.ToDouble(l) * Convert.ToDouble(r);
                    case "DIV": return Convert.ToDouble(l) / Convert.ToDouble(r);
                    case "MOD": return Convert.ToDouble(l) % Convert.ToDouble(r);
                    default: throw new Exception("Unknown operation.");
                }
            };

            // Verificam tipurile si aplicam operatia
            if (left.Variables[0].Value is int leftInt && right.Variables[0].Value is int rightInt)
            {
                programData.Variables.Add(new ProgramData.Variable
                {
                    VariableType = ProgramData.Type.Int,
                    Value = (int)applyOperation(leftInt, rightInt)
                });
            }
            else if (left.Variables[0].Value is float leftFloat && right.Variables[0].Value is float rightFloat)
            {
                programData.Variables.Add(new ProgramData.Variable
                {
                    VariableType = ProgramData.Type.Float,
                    Value = (float)applyOperation(leftFloat, rightFloat)
                });
            }
            else if (left.Variables[0].Value is double leftDouble && right.Variables[0].Value is double rightDouble)
            {
                programData.Variables.Add(new ProgramData.Variable
                {
                    VariableType = ProgramData.Type.Double,
                    Value = applyOperation(leftDouble, rightDouble)
                });
            }
            else
            {
                throw new Exception("Arithmetic operations can be applied only to numeric values.");
            }

            return programData;
        }

        private ProgramData PerformRelationalOperation(ProgramData left, ProgramData right, string operation)
        {
            ProgramData programData = new ProgramData();

            // Functie generica pentru comparatii
            Func<object, object, int> compareValues = (object l, object r) =>
            {
                bool result = false;
                switch (operation)
                {
                    case "EQUAL": result = l.Equals(r); break;
                    case "NOT_EQUAL": result = !l.Equals(r); break;
                    case "LESS_THAN": result = Comparer<object>.Default.Compare(l, r) < 0; break;
                    case "LESS_THAN_OR_EQUAL": result = Comparer<object>.Default.Compare(l, r) <= 0; break;
                    case "GREATER_THAN": result = Comparer<object>.Default.Compare(l, r) > 0; break;
                    case "GREATER_THAN_OR_EQUAL": result = Comparer<object>.Default.Compare(l, r) >= 0; break;
                    default: throw new Exception("Unknown operation.");
                }
                return result ? 1 : 0;
            };

            // Verificam tipul de date si aplicam comparatia
            if (left.Variables[0].Value is int leftInt && right.Variables[0].Value is int rightInt)
            {
                programData.Variables.Add(new ProgramData.Variable
                {
                    VariableType = ProgramData.Type.Int,
                    Value = compareValues(leftInt, rightInt)
                });
            }
            else if (left.Variables[0].Value is float leftFloat && right.Variables[0].Value is float rightFloat)
            {
                programData.Variables.Add(new ProgramData.Variable
                {
                    VariableType = ProgramData.Type.Int,
                    Value = compareValues(leftFloat, rightFloat)
                });
            }
            else if (left.Variables[0].Value is double leftDouble && right.Variables[0].Value is double rightDouble)
            {
                programData.Variables.Add(new ProgramData.Variable
                {
                    VariableType = ProgramData.Type.Int,
                    Value = compareValues(leftDouble, rightDouble)
                });
            }
            else
            {
                throw new Exception("Relational operations can be applied only to numeric values.");
            }

            return programData;
        }

        public override ProgramData VisitProgram([Antlr4.Runtime.Misc.NotNull] MiniLanguageParser.ProgramContext context)
        {
            // Vizitează fiecare declarație sau altă secțiune din program
            foreach (var statement in context.children)
            {
                Visit(statement);  // Vizitează fiecare copil
            }
            return _result;
        }

        public override ProgramData VisitDeclaration_and_assignment([Antlr4.Runtime.Misc.NotNull] MiniLanguageParser.Declaration_and_assignmentContext context)
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

            if(context.ADD_EQUALS() != null)
            {
                _result.Variables.First(v => v.Name == variableName).Value += _result.Variables.First(v => v.Name == variableName).Value;
            }
            else if(context.SUB_EQUALS() != null)
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

        public override ProgramData VisitExpression([NotNull] MiniLanguageParser.ExpressionContext context)
        {
            // Începem cu logical_expression
            return VisitLogical_expression(context.logical_expression());
        }

        public override ProgramData VisitLogical_expression([NotNull] MiniLanguageParser.Logical_expressionContext context)
        {
            // Primul termen
            var left = VisitRelational_or_equality_expression(context.relational_or_equality_expression(0));

            // Verificăm pentru operatorii logici AND sau OR
            for (int i = 1; i < context.relational_or_equality_expression().Length; i++)
            {
                var operatorToken = context.GetChild(2 * i - 1); // operatorul AND / OR
                var right = VisitRelational_or_equality_expression(context.relational_or_equality_expression(i));

                // Procesăm operatorul logic
                if (operatorToken.GetText() == "&&")
                {
                    // Operațiunea AND
                    left = PerformLogicalOperation(left, right, "AND");
                }
                else if (operatorToken.GetText() == "||")
                {
                    // Operațiunea OR
                    left = PerformLogicalOperation(left, right, "OR");
                }
            }

            return left;
        }

        public override ProgramData VisitRelational_or_equality_expression([NotNull] MiniLanguageParser.Relational_or_equality_expressionContext context)
        {
            // Primul termen
            var left = VisitAdditive_or_subtractive_expression(context.additive_or_subtractive_expression(0));

            // Verificăm operatorii de comparație
            for (int i = 1; i < context.additive_or_subtractive_expression().Length; i++)
            {
                var operatorToken = context.GetChild(2 * i - 1); // operatorul de comparație
                var right = VisitAdditive_or_subtractive_expression(context.additive_or_subtractive_expression(i));

                // Procesăm operatorul de comparație
                if (operatorToken.GetText() == "==")
                {
                    left = PerformRelationalOperation(left, right, "EQUAL");
                }
                else if (operatorToken.GetText() == "!=")
                {
                    left = PerformRelationalOperation(left, right, "NOT_EQUAL");
                }
                else if (operatorToken.GetText() == "<")
                {
                    left = PerformRelationalOperation(left, right, "LESS_THAN");
                }
                else if (operatorToken.GetText() == "<=")
                {
                    left = PerformRelationalOperation(left, right, "LESS_THAN_OR_EQUAL");
                }
                else if (operatorToken.GetText() == ">")
                {
                    left = PerformRelationalOperation(left, right, "GREATER_THAN");
                }
                else if (operatorToken.GetText() == ">=")
                {
                    left = PerformRelationalOperation(left, right, "GREATER_THAN_OR_EQUAL");
                }
            }

            return left;
        }

        public override ProgramData VisitAdditive_or_subtractive_expression([NotNull] MiniLanguageParser.Additive_or_subtractive_expressionContext context)
        {
            // Primul termen
            var left = VisitMultiplicative_expression(context.multiplicative_expression(0));

            // Verificăm operatorii de adunare sau scădere
            for (int i = 1; i < context.multiplicative_expression().Length; i++)
            {
                var operatorToken = context.GetChild(2 * i - 1); // operatorul + sau -
                var right = VisitMultiplicative_expression(context.multiplicative_expression(i));

                // Procesăm operatorul de adunare sau scădere
                if (operatorToken.GetText() == "+")
                {
                    left = PerformArithmeticOperation(left, right, "ADD");
                }
                else if (operatorToken.GetText() == "-")
                {
                    left = PerformArithmeticOperation(left, right, "SUB");
                }
            }

            return left;
        }

        public override ProgramData VisitMultiplicative_expression([NotNull] MiniLanguageParser.Multiplicative_expressionContext context)
        {
            // Primul termen
            var left = VisitUnary_expression(context.unary_expression(0));

            // Verificăm operatorii * sau /
            for (int i = 1; i < context.unary_expression().Length; i++)
            {
                var operatorToken = context.GetChild(2 * i - 1); // operatorul * sau /
                var right = VisitUnary_expression(context.unary_expression(i));

                // Procesăm operatorul de înmulțire sau împărțire
                if (operatorToken.GetText() == "*")
                {
                    left = PerformArithmeticOperation(left, right, "MUL");
                }
                else if (operatorToken.GetText() == "/")
                {
                    left = PerformArithmeticOperation(left, right, "DIV");
                }
            }

            return left;
        }

        public override ProgramData VisitUnary_expression([NotNull] MiniLanguageParser.Unary_expressionContext context)
        {
            if (context.NOT() != null)
            {
                var expr = VisitPrimary_expression(context.primary_expression());
                return PerformUnaryOperation(expr, "NOT");
            }
            else if (context.MINUS() != null)
            {
                var expr = VisitPrimary_expression(context.primary_expression());
                return PerformUnaryOperation(expr, "NEGATE");
            }

            return VisitPrimary_expression(context.primary_expression());
        }

        public override ProgramData VisitPrimary_expression([Antlr4.Runtime.Misc.NotNull] MiniLanguageParser.Primary_expressionContext context)
        {
            if (context.VARIABLE_NAME() != null)
            {
                var variableName = context.VARIABLE_NAME().GetText();
                return GetVariable(variableName);
            }
            else if (context.numeral_value() != null)
            {
                return VisitNumeral_value(context.numeral_value());
            }
            else if (context.STRING_VALUE() != null)
            {
                var value = context.STRING_VALUE().GetText();
                ProgramData programData = new ProgramData();
                programData.Variables.Add(new ProgramData.Variable
                {
                    VariableType = ProgramData.Type.String,
                    Value = value
                });
                return programData;
            }

            throw new Exception("Unexpected primary expression");
        }

        public override ProgramData VisitFor_statement([NotNull] MiniLanguageParser.For_statementContext context)
        {
            var forClauseContext = context.for_clause();
            var forBodyContext = context.block();
            var ForBodyContextNewLine = context.new_line_block();

            if(forClauseContext != null)
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

            if(assignmentContext != null)
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
            else if (postIncrementOrDecrement != null)
            {
                _residualPostIncrement = VisitPost_inccrement_or_decrement_no_semicolon(postIncrementOrDecrement);
            }

            return _result;
        }

        public override ProgramData VisitStatement([NotNull] MiniLanguageParser.StatementContext context)
        {
            if (context.assignment() != null)
            {
                VisitAssignment(context.assignment());
            }
            else if (context.assignment_op() != null)
            {
                VisitAssignment_op(context.assignment_op());
            }
            else if(context.declaration() != null)
            {
                VisitDeclaration(context.declaration());
            }
            else if (context.declaration_and_assignment() != null)
            {
                VisitDeclaration_and_assignment(context.declaration_and_assignment());
            }
            else if(context.function_call() != null)
            {
                VisitFunction_call(context.function_call());
            }
            else if (context.if_statement() != null)
            {
                VisitIf_statement(context.if_statement());
            }
            else if (context.while_statement() != null)
            {
                VisitWhile_statement(context.while_statement());
            }
            else if (context.for_statement() != null)
            {
                VisitFor_statement(context.for_statement());
            }
            return _result;
        }

        public override ProgramData VisitBlock([NotNull] MiniLanguageParser.BlockContext context)
        {
            var ListOfStatements = context.statement();

            if(ListOfStatements != null)
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
            return _result;
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

        public override ProgramData VisitFunction_declaration([Antlr4.Runtime.Misc.NotNull] MiniLanguageParser.Function_declarationContext context)
        {
            var functionName = context.VARIABLE_NAME().GetText();
            var returnType = FunctionParseType(context.return_type());

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
                }
            }

            return _result;
        }

        public override ProgramData VisitMain_declaration([Antlr4.Runtime.Misc.NotNull] MiniLanguageParser.Main_declarationContext context)
        {
            var returnType = FunctionParseType(context.return_type());
            var functionName = context.MAIN_FUNCTION().GetText();

            // Adaugă funcția la lista de funcții
            _result.Functions.Add(new ProgramData.Function
            {
                Name = functionName,
                ReturnType = returnType
            });

            return _result;
        }

        private ProgramData.Type ParseType(MiniLanguageParser.TypeContext context)
        {
            if (context.INTEGER_TYPE() != null) return ProgramData.Type.Int;
            if (context.FLOAT_TYPE() != null) return ProgramData.Type.Float;
            if (context.STRING_TYPE() != null) return ProgramData.Type.String;
            if(context.DOUBLE_TYPE() != null) return ProgramData.Type.Double;

            throw new Exception("Unknown variable type");
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
            else if (valueContext.expression() != null)
            {
                // Ar trebui să implementezi un vizitator pentru expresii
                var expressionResult = Visit(valueContext.expression());
                return expressionResult; // Este posibil să trebuiască să returnezi un obiect adecvat
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
            //PrintLexems(lexer);
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
            }
        }

    }
}
