using System;
using System.IO;
using System.Collections.Generic;

using Antlr4.Runtime;

namespace MinimalCompiler
{
    public class ProgramData
    {
        public class Variable
        {
            public enum Type
            {
                Int,
                Float,
                String
            }
            public Type VariableType { get; set; }
            public dynamic? Value { get; set; }
        }
        public List<Variable> Variables { get; set; } = new List<Variable>();
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

            MiniLanguageLexer lexer = SetupLexer(input);
            PrintLexems(lexer);
            MiniLanguageParser parser =  SetupParser(lexer);
        }
    }
}
