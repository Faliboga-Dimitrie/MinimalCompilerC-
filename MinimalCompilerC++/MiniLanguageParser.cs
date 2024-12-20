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
using System.Diagnostics;
using System.Collections.Generic;
using Antlr4.Runtime;
using Antlr4.Runtime.Atn;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using DFA = Antlr4.Runtime.Dfa.DFA;

[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.13.1")]
[System.CLSCompliant(false)]
public partial class MiniLanguageParser : Parser {
	protected static DFA[] decisionToDFA;
	protected static PredictionContextCache sharedContextCache = new PredictionContextCache();
	public const int
		INTEGER_TYPE=1, FLOAT_TYPE=2, STRING_TYPE=3, LPARAN=4, RPARAN=5, LBRACE=6, 
		RBRACE=7, EXP=8, ASTERISK=9, SLASH=10, PLUS=11, MINUS=12, EQUALS=13, ADD_EQUALS=14, 
		SUB_EQUALS=15, MUL_EQUALS=16, DIV_EQUALS=17, SEMICOLON=18, AND=19, OR=20, 
		NOT=21, INTEGER_VALUE=22, FLOAT_VALUE=23, STRING_VALUE=24, VARIABLE_NAME=25, 
		WS=26, COMMENT=27;
	public const int
		RULE_declaration = 0, RULE_type = 1, RULE_value = 2, RULE_assignment = 3, 
		RULE_assignment_operator = 4;
	public static readonly string[] ruleNames = {
		"declaration", "type", "value", "assignment", "assignment_operator"
	};

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

	public override int[] SerializedAtn { get { return _serializedATN; } }

	static MiniLanguageParser() {
		decisionToDFA = new DFA[_ATN.NumberOfDecisions];
		for (int i = 0; i < _ATN.NumberOfDecisions; i++) {
			decisionToDFA[i] = new DFA(_ATN.GetDecisionState(i), i);
		}
	}

		public MiniLanguageParser(ITokenStream input) : this(input, Console.Out, Console.Error) { }

		public MiniLanguageParser(ITokenStream input, TextWriter output, TextWriter errorOutput)
		: base(input, output, errorOutput)
	{
		Interpreter = new ParserATNSimulator(this, _ATN, decisionToDFA, sharedContextCache);
	}

	public partial class DeclarationContext : ParserRuleContext {
		[System.Diagnostics.DebuggerNonUserCode] public TypeContext type() {
			return GetRuleContext<TypeContext>(0);
		}
		[System.Diagnostics.DebuggerNonUserCode] public ITerminalNode VARIABLE_NAME() { return GetToken(MiniLanguageParser.VARIABLE_NAME, 0); }
		[System.Diagnostics.DebuggerNonUserCode] public ITerminalNode EQUALS() { return GetToken(MiniLanguageParser.EQUALS, 0); }
		[System.Diagnostics.DebuggerNonUserCode] public ValueContext value() {
			return GetRuleContext<ValueContext>(0);
		}
		[System.Diagnostics.DebuggerNonUserCode] public ITerminalNode SEMICOLON() { return GetToken(MiniLanguageParser.SEMICOLON, 0); }
		public DeclarationContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_declaration; } }
		[System.Diagnostics.DebuggerNonUserCode]
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			IMiniLanguageVisitor<TResult> typedVisitor = visitor as IMiniLanguageVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitDeclaration(this);
			else return visitor.VisitChildren(this);
		}
	}

	[RuleVersion(0)]
	public DeclarationContext declaration() {
		DeclarationContext _localctx = new DeclarationContext(Context, State);
		EnterRule(_localctx, 0, RULE_declaration);
		try {
			EnterOuterAlt(_localctx, 1);
			{
			State = 10;
			type();
			State = 11;
			Match(VARIABLE_NAME);
			State = 12;
			Match(EQUALS);
			State = 13;
			value();
			State = 14;
			Match(SEMICOLON);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class TypeContext : ParserRuleContext {
		[System.Diagnostics.DebuggerNonUserCode] public ITerminalNode INTEGER_TYPE() { return GetToken(MiniLanguageParser.INTEGER_TYPE, 0); }
		[System.Diagnostics.DebuggerNonUserCode] public ITerminalNode FLOAT_TYPE() { return GetToken(MiniLanguageParser.FLOAT_TYPE, 0); }
		[System.Diagnostics.DebuggerNonUserCode] public ITerminalNode STRING_TYPE() { return GetToken(MiniLanguageParser.STRING_TYPE, 0); }
		public TypeContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_type; } }
		[System.Diagnostics.DebuggerNonUserCode]
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			IMiniLanguageVisitor<TResult> typedVisitor = visitor as IMiniLanguageVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitType(this);
			else return visitor.VisitChildren(this);
		}
	}

	[RuleVersion(0)]
	public TypeContext type() {
		TypeContext _localctx = new TypeContext(Context, State);
		EnterRule(_localctx, 2, RULE_type);
		int _la;
		try {
			EnterOuterAlt(_localctx, 1);
			{
			State = 16;
			_la = TokenStream.LA(1);
			if ( !((((_la) & ~0x3f) == 0 && ((1L << _la) & 14L) != 0)) ) {
			ErrorHandler.RecoverInline(this);
			}
			else {
				ErrorHandler.ReportMatch(this);
			    Consume();
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class ValueContext : ParserRuleContext {
		[System.Diagnostics.DebuggerNonUserCode] public ITerminalNode INTEGER_VALUE() { return GetToken(MiniLanguageParser.INTEGER_VALUE, 0); }
		[System.Diagnostics.DebuggerNonUserCode] public ITerminalNode FLOAT_VALUE() { return GetToken(MiniLanguageParser.FLOAT_VALUE, 0); }
		[System.Diagnostics.DebuggerNonUserCode] public ITerminalNode STRING_VALUE() { return GetToken(MiniLanguageParser.STRING_VALUE, 0); }
		public ValueContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_value; } }
		[System.Diagnostics.DebuggerNonUserCode]
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			IMiniLanguageVisitor<TResult> typedVisitor = visitor as IMiniLanguageVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitValue(this);
			else return visitor.VisitChildren(this);
		}
	}

	[RuleVersion(0)]
	public ValueContext value() {
		ValueContext _localctx = new ValueContext(Context, State);
		EnterRule(_localctx, 4, RULE_value);
		int _la;
		try {
			EnterOuterAlt(_localctx, 1);
			{
			State = 18;
			_la = TokenStream.LA(1);
			if ( !((((_la) & ~0x3f) == 0 && ((1L << _la) & 29360128L) != 0)) ) {
			ErrorHandler.RecoverInline(this);
			}
			else {
				ErrorHandler.ReportMatch(this);
			    Consume();
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class AssignmentContext : ParserRuleContext {
		[System.Diagnostics.DebuggerNonUserCode] public ITerminalNode VARIABLE_NAME() { return GetToken(MiniLanguageParser.VARIABLE_NAME, 0); }
		[System.Diagnostics.DebuggerNonUserCode] public ITerminalNode EQUALS() { return GetToken(MiniLanguageParser.EQUALS, 0); }
		[System.Diagnostics.DebuggerNonUserCode] public ValueContext value() {
			return GetRuleContext<ValueContext>(0);
		}
		[System.Diagnostics.DebuggerNonUserCode] public ITerminalNode SEMICOLON() { return GetToken(MiniLanguageParser.SEMICOLON, 0); }
		[System.Diagnostics.DebuggerNonUserCode] public TypeContext type() {
			return GetRuleContext<TypeContext>(0);
		}
		public AssignmentContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_assignment; } }
		[System.Diagnostics.DebuggerNonUserCode]
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			IMiniLanguageVisitor<TResult> typedVisitor = visitor as IMiniLanguageVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitAssignment(this);
			else return visitor.VisitChildren(this);
		}
	}

	[RuleVersion(0)]
	public AssignmentContext assignment() {
		AssignmentContext _localctx = new AssignmentContext(Context, State);
		EnterRule(_localctx, 6, RULE_assignment);
		int _la;
		try {
			EnterOuterAlt(_localctx, 1);
			{
			State = 21;
			ErrorHandler.Sync(this);
			_la = TokenStream.LA(1);
			if ((((_la) & ~0x3f) == 0 && ((1L << _la) & 14L) != 0)) {
				{
				State = 20;
				type();
				}
			}

			State = 23;
			Match(VARIABLE_NAME);
			State = 24;
			Match(EQUALS);
			State = 25;
			value();
			State = 26;
			Match(SEMICOLON);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class Assignment_operatorContext : ParserRuleContext {
		[System.Diagnostics.DebuggerNonUserCode] public ITerminalNode ADD_EQUALS() { return GetToken(MiniLanguageParser.ADD_EQUALS, 0); }
		[System.Diagnostics.DebuggerNonUserCode] public ITerminalNode SUB_EQUALS() { return GetToken(MiniLanguageParser.SUB_EQUALS, 0); }
		[System.Diagnostics.DebuggerNonUserCode] public ITerminalNode MUL_EQUALS() { return GetToken(MiniLanguageParser.MUL_EQUALS, 0); }
		[System.Diagnostics.DebuggerNonUserCode] public ITerminalNode DIV_EQUALS() { return GetToken(MiniLanguageParser.DIV_EQUALS, 0); }
		public Assignment_operatorContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_assignment_operator; } }
		[System.Diagnostics.DebuggerNonUserCode]
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			IMiniLanguageVisitor<TResult> typedVisitor = visitor as IMiniLanguageVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitAssignment_operator(this);
			else return visitor.VisitChildren(this);
		}
	}

	[RuleVersion(0)]
	public Assignment_operatorContext assignment_operator() {
		Assignment_operatorContext _localctx = new Assignment_operatorContext(Context, State);
		EnterRule(_localctx, 8, RULE_assignment_operator);
		int _la;
		try {
			EnterOuterAlt(_localctx, 1);
			{
			State = 28;
			_la = TokenStream.LA(1);
			if ( !((((_la) & ~0x3f) == 0 && ((1L << _la) & 245760L) != 0)) ) {
			ErrorHandler.RecoverInline(this);
			}
			else {
				ErrorHandler.ReportMatch(this);
			    Consume();
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	private static int[] _serializedATN = {
		4,1,27,31,2,0,7,0,2,1,7,1,2,2,7,2,2,3,7,3,2,4,7,4,1,0,1,0,1,0,1,0,1,0,
		1,0,1,1,1,1,1,2,1,2,1,3,3,3,22,8,3,1,3,1,3,1,3,1,3,1,3,1,4,1,4,1,4,0,0,
		5,0,2,4,6,8,0,3,1,0,1,3,1,0,22,24,1,0,14,17,26,0,10,1,0,0,0,2,16,1,0,0,
		0,4,18,1,0,0,0,6,21,1,0,0,0,8,28,1,0,0,0,10,11,3,2,1,0,11,12,5,25,0,0,
		12,13,5,13,0,0,13,14,3,4,2,0,14,15,5,18,0,0,15,1,1,0,0,0,16,17,7,0,0,0,
		17,3,1,0,0,0,18,19,7,1,0,0,19,5,1,0,0,0,20,22,3,2,1,0,21,20,1,0,0,0,21,
		22,1,0,0,0,22,23,1,0,0,0,23,24,5,25,0,0,24,25,5,13,0,0,25,26,3,4,2,0,26,
		27,5,18,0,0,27,7,1,0,0,0,28,29,7,2,0,0,29,9,1,0,0,0,1,21
	};

	public static readonly ATN _ATN =
		new ATNDeserializer().Deserialize(_serializedATN);


}
