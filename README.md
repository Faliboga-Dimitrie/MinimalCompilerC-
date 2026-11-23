# Mini Language Processor (C# / ANTLR4)

An educational **C# / .NET 8** project that defines a small programming-language grammar with **ANTLR4**, tokenizes and parses a source file, and uses a custom parse-tree visitor to inspect and process language constructs.

The repository is named `MinimalCompilerC-`, and the original solution is named `MinimalCompilerC++`, but the implementation is **C#**, not C++. The project focuses on the **front end of language processing** and a partially implemented evaluator; it **does not generate executable or machine code**.

## What it does

- Defines a small C-like language in [`MiniLanguage.g4`](MinimalCompilerC++/MiniLanguage.g4).
- Uses ANTLR4-generated lexer, parser, and visitor-related classes.
- Reads a source program from `input.txt`.
- Prints the lexer tokens (token types and text).
- Parses the program according to the grammar.
- Traverses the parse tree using the custom `MiniLanguageVisitor` implemented in `Program.cs`.
- Records variables, functions, parameters, and local-variable information in `ProgramData`.
- Performs some value evaluation and validation while visiting declarations, assignments, expressions, and control-flow constructs.
- Prints collected variable and function information to the console.

> **Status:** This is a learning project with incomplete semantics, rather than a production compiler or a fully featured interpreter. In particular, return-value handling is unfinished, and the project does not emit compiled code.

## Technologies

| Component | Technology |
| --- | --- |
| Implementation | C# |
| Runtime and SDK target | .NET 8 (`net8.0`) |
| Lexer and parser | ANTLR4 |
| ANTLR runtime package | `Antlr4.Runtime.Standard` 4.13.1 |
| Development environment | Visual Studio / .NET CLI |
| Input | Text file (`input.txt`) |

## Processing pipeline

```text
input.txt
   |
   v
ANTLR4 lexer (MiniLanguageLexer)
   |
   +--> Console output: tokens / lexemes
   |
   v
ANTLR4 parser (MiniLanguageParser)
   |
   v
Parse tree
   |
   v
Custom MiniLanguageVisitor
   |
   v
ProgramData (variables, values, functions, parameters)
   |
   v
Console summary
```

### The grammar

`MiniLanguage.g4` describes lexical tokens and syntactic rules for a small language with:

- Numeric and string types: `int`, `float`, `double`, and `string`.
- Function declarations, parameters, calls, and a required `main` declaration.
- Variable declarations and assignments.
- Arithmetic, comparison, and basic logical expressions.
- `if` / `else`, `while`, and `for` syntax.
- Increment/decrement and compound assignments.
- `return` syntax (runtime handling is incomplete).

**Recognized syntax is not the same as complete execution support.** Several grammar productions have visitor implementations that are partial, and the behavior is not equivalent to a standard C-like language compiler.

### The visitor and program model

The custom `MiniLanguageVisitor` (inside `Program.cs`) walks parser contexts and collects or processes information. `ProgramData.cs` holds records for variables and functions, including types, values, parameters, and local variables.

The application entry point also appears in `Program.cs`: it initializes the lexer/parser, prints tokens, visits the parse tree, and displays the collected information.

## Getting started

### Requirements

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- Git (optional)
- Visual Studio with .NET desktop development support (optional, if using the .NET CLI)

The repository already includes ANTLR4-generated `.cs` files. You do **not** need to regenerate those files just to build the current version. If you change `MiniLanguage.g4`, regenerate the ANTLR sources separately; generation is not configured as an automatic step in the `.csproj`.

### Build and run with the .NET CLI

From PowerShell:

```powershell
git clone https://github.com/Faliboga-Dimitrie/MinimalCompilerC-.git
cd MinimalCompilerC-
cd 'MinimalCompilerC++'

dotnet restore
dotnet build
dotnet run
```

**Important:** Run the command from inside the `MinimalCompilerC++` project directory. The program reads `input.txt` relative to its **current working directory**. The project file also includes a setting to copy `input.txt` to the build output folder, but launching from an unrelated directory can still produce a file-not-found error.

### Run from Visual Studio

1. Open `MinimalCompilerC++.sln`.
2. Restore NuGet packages if prompted.
3. Build the solution.
4. Ensure `input.txt` is available in the working directory used to launch the project.
5. Run the console application.

## Try the language

Open `MinimalCompilerC++/input.txt` and replace its contents with a small program. This example demonstrates the **grammar's basic declaration syntax**:

```c
void main() {
    int count = 3;
    string message = "hello";
}
```

The input file is parsed when you run the project. The console is designed to show a token listing followed by the variables and functions collected by the visitor. The snippet is illustrative of the grammar; it is not presented as a separately verified runtime test.

If you experiment with expressions, function calls, or control flow, consult [`MiniLanguage.g4`](MinimalCompilerC++/MiniLanguage.g4) for the accepted syntax and expect some incomplete behavior.

## Project structure

```text
MinimalCompilerC-/
├── MinimalCompilerC++.sln
├── README.md
└── MinimalCompilerC++/
    ├── MinimalCompilerC++.csproj   # .NET 8 console project and NuGet dependency
    ├── MiniLanguage.g4             # Language grammar
    ├── Program.cs                  # Custom visitor and application entry point
    ├── ProgramData.cs              # Variable / function data model
    ├── MiniLanguageLexer.cs        # ANTLR4-generated lexer
    ├── MiniLanguageParser.cs       # ANTLR4-generated parser
    ├── MiniLanguageBaseVisitor.cs  # ANTLR4-generated base visitor
    ├── MiniLanguageVisitor.cs      # ANTLR4-generated visitor interface
    └── input.txt                   # Source program to process
```

## Implementation details and scope

- **Lexing:** `SetupLexer()` wraps the input in an `AntlrInputStream` and creates a `MiniLanguageLexer`. `PrintLexems()` displays the tokens.
- **Parsing:** `SetupParser()` passes a `CommonTokenStream` to `MiniLanguageParser`; the entry rule is `program()`.
- **Visiting:** `MiniLanguageVisitor` contains custom methods for grammar contexts, with logic for collecting program entities and handling selected statements.
- **Data model:** `ProgramData` holds variable/function lists and state used by the visitor.
- **Output:** The console displays token information and a summary of variables and functions. No object file, binary, or target-language source file is produced.

## Known limitations

- `VisitReturn_statement()` is currently a placeholder; function-return semantics are not implemented end to end.
- Handling of function calls, evaluation, and other control-flow constructs is partial. The presence of a grammar rule does not guarantee complete runtime behavior.
- Errors may surface as parser diagnostics or exceptions rather than as a unified compiler-diagnostics system.
- The input path is fixed to `input.txt`; there is no command-line flag to select another source file.
- There is no code-generation phase or verified automated test suite in the current repository.

## Development notes

This project combines **manually written code and AI-assisted development**. ANTLR4 was also used to generate the lexer, parser, and visitor scaffolding from the grammar. I keep it as an educational project in parsing, grammar design, C#, and program representation.

## Author

[Dimitrie-Aron Faliboga](https://github.com/Faliboga-Dimitrie)
