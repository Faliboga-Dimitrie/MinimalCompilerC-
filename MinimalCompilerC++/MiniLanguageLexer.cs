//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     ANTLR Version: 4.13.1
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from c:/Users/Dimitrie_U/1Facultate/Anul2/Semestrul1/LFC/Proiecte/Proiect2/MinimalCompilerC++/MinimalCompilerC++/MiniLanguage.g4 by ANTLR 4.13.1

// Unreachable code detected
#pragma warning disable 0162
// The variable '...' is assigned but its value is never used
#pragma warning disable 0219
// Missing XML comment for publicly visible type or member '...'
#pragma warning disable 1591
// Ambiguous reference in cref attribute
#pragma warning disable 419

using System;
using System.IO;
using System.Text;
using Antlr4.Runtime;
using Antlr4.Runtime.Atn;
using Antlr4.Runtime.Misc;
using DFA = Antlr4.Runtime.Dfa.DFA;

[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.13.1")]
[System.CLSCompliant(false)]
public partial class MiniLanguageLexer : Lexer {
	protected static DFA[] decisionToDFA;
	protected static PredictionContextCache sharedContextCache = new PredictionContextCache();
	public const int
		INTEGER_TYPE=1, FLOAT_TYPE=2, STRING_TYPE=3, LPARAN=4, RPARAN=5, LBRACE=6, 
		RBRACE=7, EXP=8, ASTERISK=9, SLASH=10, PLUS=11, MINUS=12, EQUALS=13, ADD_EQUALS=14, 
		SUB_EQUALS=15, MUL_EQUALS=16, DIV_EQUALS=17, SEMICOLON=18, AND=19, OR=20, 
		NOT=21, INTEGER_VALUE=22, FLOAT_VALUE=23, STRING_VALUE=24, VARIABLE_NAME=25, 
		WS=26, COMMENT=27;
	public static string[] channelNames = {
		"DEFAULT_TOKEN_CHANNEL", "HIDDEN"
	};

	public static string[] modeNames = {
		"DEFAULT_MODE"
	};

	public static readonly string[] ruleNames = {
		"DIGIT", "NON_ZERO_DIGIT", "ZERO", "LETTER", "INTEGER_TYPE", "FLOAT_TYPE", 
		"STRING_TYPE", "LPARAN", "RPARAN", "LBRACE", "RBRACE", "EXP", "ASTERISK", 
		"SLASH", "PLUS", "MINUS", "EQUALS", "ADD_EQUALS", "SUB_EQUALS", "MUL_EQUALS", 
		"DIV_EQUALS", "SEMICOLON", "AND", "OR", "NOT", "INTEGER_VALUE", "FLOAT_VALUE", 
		"STRING_VALUE", "VARIABLE_NAME", "WS", "COMMENT"
	};


	public MiniLanguageLexer(ICharStream input)
	: this(input, Console.Out, Console.Error) { }

	public MiniLanguageLexer(ICharStream input, TextWriter output, TextWriter errorOutput)
	: base(input, output, errorOutput)
	{
		Interpreter = new LexerATNSimulator(this, _ATN, decisionToDFA, sharedContextCache);
	}

	private static readonly string[] _LiteralNames = {
		null, "'int'", "'float'", "'string'", "'('", "')'", "'{'", "'}'", "'^'", 
		"'*'", "'/'", "'+'", "'-'", "'='", "'+='", "'-='", "'*='", "'/='", "';'", 
		"'&&'", "'||'", "'!'"
	};
	private static readonly string[] _SymbolicNames = {
		null, "INTEGER_TYPE", "FLOAT_TYPE", "STRING_TYPE", "LPARAN", "RPARAN", 
		"LBRACE", "RBRACE", "EXP", "ASTERISK", "SLASH", "PLUS", "MINUS", "EQUALS", 
		"ADD_EQUALS", "SUB_EQUALS", "MUL_EQUALS", "DIV_EQUALS", "SEMICOLON", "AND", 
		"OR", "NOT", "INTEGER_VALUE", "FLOAT_VALUE", "STRING_VALUE", "VARIABLE_NAME", 
		"WS", "COMMENT"
	};
	public static readonly IVocabulary DefaultVocabulary = new Vocabulary(_LiteralNames, _SymbolicNames);

	[NotNull]
	public override IVocabulary Vocabulary
	{
		get
		{
			return DefaultVocabulary;
		}
	}

	public override string GrammarFileName { get { return "MiniLanguage.g4"; } }

	public override string[] RuleNames { get { return ruleNames; } }

	public override string[] ChannelNames { get { return channelNames; } }

	public override string[] ModeNames { get { return modeNames; } }

	public override int[] SerializedAtn { get { return _serializedATN; } }

	static MiniLanguageLexer() {
		decisionToDFA = new DFA[_ATN.NumberOfDecisions];
		for (int i = 0; i < _ATN.NumberOfDecisions; i++) {
			decisionToDFA[i] = new DFA(_ATN.GetDecisionState(i), i);
		}
	}
	private static int[] _serializedATN = {
		4,0,27,185,6,-1,2,0,7,0,2,1,7,1,2,2,7,2,2,3,7,3,2,4,7,4,2,5,7,5,2,6,7,
		6,2,7,7,7,2,8,7,8,2,9,7,9,2,10,7,10,2,11,7,11,2,12,7,12,2,13,7,13,2,14,
		7,14,2,15,7,15,2,16,7,16,2,17,7,17,2,18,7,18,2,19,7,19,2,20,7,20,2,21,
		7,21,2,22,7,22,2,23,7,23,2,24,7,24,2,25,7,25,2,26,7,26,2,27,7,27,2,28,
		7,28,2,29,7,29,2,30,7,30,1,0,1,0,1,1,1,1,1,2,1,2,1,3,1,3,1,4,1,4,1,4,1,
		4,1,5,1,5,1,5,1,5,1,5,1,5,1,6,1,6,1,6,1,6,1,6,1,6,1,6,1,7,1,7,1,8,1,8,
		1,9,1,9,1,10,1,10,1,11,1,11,1,12,1,12,1,13,1,13,1,14,1,14,1,15,1,15,1,
		16,1,16,1,17,1,17,1,17,1,18,1,18,1,18,1,19,1,19,1,19,1,20,1,20,1,20,1,
		21,1,21,1,22,1,22,1,22,1,23,1,23,1,23,1,24,1,24,1,25,1,25,1,25,5,25,134,
		8,25,10,25,12,25,137,9,25,3,25,139,8,25,1,26,1,26,1,26,5,26,144,8,26,10,
		26,12,26,147,9,26,1,26,3,26,150,8,26,1,27,1,27,4,27,154,8,27,11,27,12,
		27,155,1,27,1,27,1,28,1,28,1,28,5,28,163,8,28,10,28,12,28,166,9,28,1,29,
		4,29,169,8,29,11,29,12,29,170,1,29,1,29,1,30,1,30,1,30,1,30,5,30,179,8,
		30,10,30,12,30,182,9,30,1,30,1,30,1,155,0,31,1,0,3,0,5,0,7,0,9,1,11,2,
		13,3,15,4,17,5,19,6,21,7,23,8,25,9,27,10,29,11,31,12,33,13,35,14,37,15,
		39,16,41,17,43,18,45,19,47,20,49,21,51,22,53,23,55,24,57,25,59,26,61,27,
		1,0,5,1,0,48,57,1,0,49,57,2,0,65,90,97,122,3,0,9,10,13,13,32,32,2,0,10,
		10,13,13,189,0,9,1,0,0,0,0,11,1,0,0,0,0,13,1,0,0,0,0,15,1,0,0,0,0,17,1,
		0,0,0,0,19,1,0,0,0,0,21,1,0,0,0,0,23,1,0,0,0,0,25,1,0,0,0,0,27,1,0,0,0,
		0,29,1,0,0,0,0,31,1,0,0,0,0,33,1,0,0,0,0,35,1,0,0,0,0,37,1,0,0,0,0,39,
		1,0,0,0,0,41,1,0,0,0,0,43,1,0,0,0,0,45,1,0,0,0,0,47,1,0,0,0,0,49,1,0,0,
		0,0,51,1,0,0,0,0,53,1,0,0,0,0,55,1,0,0,0,0,57,1,0,0,0,0,59,1,0,0,0,0,61,
		1,0,0,0,1,63,1,0,0,0,3,65,1,0,0,0,5,67,1,0,0,0,7,69,1,0,0,0,9,71,1,0,0,
		0,11,75,1,0,0,0,13,81,1,0,0,0,15,88,1,0,0,0,17,90,1,0,0,0,19,92,1,0,0,
		0,21,94,1,0,0,0,23,96,1,0,0,0,25,98,1,0,0,0,27,100,1,0,0,0,29,102,1,0,
		0,0,31,104,1,0,0,0,33,106,1,0,0,0,35,108,1,0,0,0,37,111,1,0,0,0,39,114,
		1,0,0,0,41,117,1,0,0,0,43,120,1,0,0,0,45,122,1,0,0,0,47,125,1,0,0,0,49,
		128,1,0,0,0,51,138,1,0,0,0,53,140,1,0,0,0,55,151,1,0,0,0,57,159,1,0,0,
		0,59,168,1,0,0,0,61,174,1,0,0,0,63,64,7,0,0,0,64,2,1,0,0,0,65,66,7,1,0,
		0,66,4,1,0,0,0,67,68,5,48,0,0,68,6,1,0,0,0,69,70,7,2,0,0,70,8,1,0,0,0,
		71,72,5,105,0,0,72,73,5,110,0,0,73,74,5,116,0,0,74,10,1,0,0,0,75,76,5,
		102,0,0,76,77,5,108,0,0,77,78,5,111,0,0,78,79,5,97,0,0,79,80,5,116,0,0,
		80,12,1,0,0,0,81,82,5,115,0,0,82,83,5,116,0,0,83,84,5,114,0,0,84,85,5,
		105,0,0,85,86,5,110,0,0,86,87,5,103,0,0,87,14,1,0,0,0,88,89,5,40,0,0,89,
		16,1,0,0,0,90,91,5,41,0,0,91,18,1,0,0,0,92,93,5,123,0,0,93,20,1,0,0,0,
		94,95,5,125,0,0,95,22,1,0,0,0,96,97,5,94,0,0,97,24,1,0,0,0,98,99,5,42,
		0,0,99,26,1,0,0,0,100,101,5,47,0,0,101,28,1,0,0,0,102,103,5,43,0,0,103,
		30,1,0,0,0,104,105,5,45,0,0,105,32,1,0,0,0,106,107,5,61,0,0,107,34,1,0,
		0,0,108,109,5,43,0,0,109,110,5,61,0,0,110,36,1,0,0,0,111,112,5,45,0,0,
		112,113,5,61,0,0,113,38,1,0,0,0,114,115,5,42,0,0,115,116,5,61,0,0,116,
		40,1,0,0,0,117,118,5,47,0,0,118,119,5,61,0,0,119,42,1,0,0,0,120,121,5,
		59,0,0,121,44,1,0,0,0,122,123,5,38,0,0,123,124,5,38,0,0,124,46,1,0,0,0,
		125,126,5,124,0,0,126,127,5,124,0,0,127,48,1,0,0,0,128,129,5,33,0,0,129,
		50,1,0,0,0,130,139,3,5,2,0,131,135,3,3,1,0,132,134,3,1,0,0,133,132,1,0,
		0,0,134,137,1,0,0,0,135,133,1,0,0,0,135,136,1,0,0,0,136,139,1,0,0,0,137,
		135,1,0,0,0,138,130,1,0,0,0,138,131,1,0,0,0,139,52,1,0,0,0,140,141,3,51,
		25,0,141,145,5,46,0,0,142,144,3,1,0,0,143,142,1,0,0,0,144,147,1,0,0,0,
		145,143,1,0,0,0,145,146,1,0,0,0,146,149,1,0,0,0,147,145,1,0,0,0,148,150,
		5,102,0,0,149,148,1,0,0,0,149,150,1,0,0,0,150,54,1,0,0,0,151,153,5,34,
		0,0,152,154,9,0,0,0,153,152,1,0,0,0,154,155,1,0,0,0,155,156,1,0,0,0,155,
		153,1,0,0,0,156,157,1,0,0,0,157,158,5,34,0,0,158,56,1,0,0,0,159,164,3,
		7,3,0,160,163,3,7,3,0,161,163,3,1,0,0,162,160,1,0,0,0,162,161,1,0,0,0,
		163,166,1,0,0,0,164,162,1,0,0,0,164,165,1,0,0,0,165,58,1,0,0,0,166,164,
		1,0,0,0,167,169,7,3,0,0,168,167,1,0,0,0,169,170,1,0,0,0,170,168,1,0,0,
		0,170,171,1,0,0,0,171,172,1,0,0,0,172,173,6,29,0,0,173,60,1,0,0,0,174,
		175,5,47,0,0,175,176,5,47,0,0,176,180,1,0,0,0,177,179,8,4,0,0,178,177,
		1,0,0,0,179,182,1,0,0,0,180,178,1,0,0,0,180,181,1,0,0,0,181,183,1,0,0,
		0,182,180,1,0,0,0,183,184,6,30,0,0,184,62,1,0,0,0,10,0,135,138,145,149,
		155,162,164,170,180,1,6,0,0
	};

	public static readonly ATN _ATN =
		new ATNDeserializer().Deserialize(_serializedATN);


}
