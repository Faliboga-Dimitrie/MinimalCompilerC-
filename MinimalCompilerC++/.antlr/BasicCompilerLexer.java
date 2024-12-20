// Generated from c:/Users/Dimitrie_U/1Facultate/Anul2/Semestrul1/LFC/Proiecte/Proiect2/MinimalCompilerC++/MinimalCompilerC++/BasicCompiler.g4 by ANTLR 4.13.1
import org.antlr.v4.runtime.Lexer;
import org.antlr.v4.runtime.CharStream;
import org.antlr.v4.runtime.Token;
import org.antlr.v4.runtime.TokenStream;
import org.antlr.v4.runtime.*;
import org.antlr.v4.runtime.atn.*;
import org.antlr.v4.runtime.dfa.DFA;
import org.antlr.v4.runtime.misc.*;

@SuppressWarnings({"all", "warnings", "unchecked", "unused", "cast", "CheckReturnValue", "this-escape"})
public class BasicCompilerLexer extends Lexer {
	static { RuntimeMetaData.checkVersion("4.13.1", RuntimeMetaData.VERSION); }

	protected static final DFA[] _decisionToDFA;
	protected static final PredictionContextCache _sharedContextCache =
		new PredictionContextCache();
	public static final int
		INTEGER_TYPE=1, FLOAT_TYPE=2, STRING_TYPE=3, LPARAN=4, RPARAN=5, LBRACE=6, 
		RBRACE=7, EXP=8, ASTERISK=9, SLASH=10, PLUS=11, MINUS=12, EQUALS=13, ADD_EQUALS=14, 
		SUB_EQUALS=15, MUL_EQUALS=16, DIV_EQUALS=17, SEMICOLON=18, AND=19, OR=20, 
		NOT=21, INTEGER_VALUE=22, FLOAT_VALUE=23, STRING_VALUE=24, VARIABLE_NAME=25, 
		WS=26;
	public static String[] channelNames = {
		"DEFAULT_TOKEN_CHANNEL", "HIDDEN"
	};

	public static String[] modeNames = {
		"DEFAULT_MODE"
	};

	private static String[] makeRuleNames() {
		return new String[] {
			"DIGIT", "NON_ZERO_DIGIT", "ZERO", "LETTER", "INTEGER_TYPE", "FLOAT_TYPE", 
			"STRING_TYPE", "LPARAN", "RPARAN", "LBRACE", "RBRACE", "EXP", "ASTERISK", 
			"SLASH", "PLUS", "MINUS", "EQUALS", "ADD_EQUALS", "SUB_EQUALS", "MUL_EQUALS", 
			"DIV_EQUALS", "SEMICOLON", "AND", "OR", "NOT", "INTEGER_VALUE", "FLOAT_VALUE", 
			"STRING_VALUE", "VARIABLE_NAME", "WS"
		};
	}
	public static final String[] ruleNames = makeRuleNames();

	private static String[] makeLiteralNames() {
		return new String[] {
			null, "'int'", "'float'", "'string'", "'('", "')'", "'{'", "'}'", "'^'", 
			"'*'", "'/'", "'+'", "'-'", "'='", "'+='", "'-='", "'*='", "'/='", "';'", 
			"'&&'", "'||'", "'!'"
		};
	}
	private static final String[] _LITERAL_NAMES = makeLiteralNames();
	private static String[] makeSymbolicNames() {
		return new String[] {
			null, "INTEGER_TYPE", "FLOAT_TYPE", "STRING_TYPE", "LPARAN", "RPARAN", 
			"LBRACE", "RBRACE", "EXP", "ASTERISK", "SLASH", "PLUS", "MINUS", "EQUALS", 
			"ADD_EQUALS", "SUB_EQUALS", "MUL_EQUALS", "DIV_EQUALS", "SEMICOLON", 
			"AND", "OR", "NOT", "INTEGER_VALUE", "FLOAT_VALUE", "STRING_VALUE", "VARIABLE_NAME", 
			"WS"
		};
	}
	private static final String[] _SYMBOLIC_NAMES = makeSymbolicNames();
	public static final Vocabulary VOCABULARY = new VocabularyImpl(_LITERAL_NAMES, _SYMBOLIC_NAMES);

	/**
	 * @deprecated Use {@link #VOCABULARY} instead.
	 */
	@Deprecated
	public static final String[] tokenNames;
	static {
		tokenNames = new String[_SYMBOLIC_NAMES.length];
		for (int i = 0; i < tokenNames.length; i++) {
			tokenNames[i] = VOCABULARY.getLiteralName(i);
			if (tokenNames[i] == null) {
				tokenNames[i] = VOCABULARY.getSymbolicName(i);
			}

			if (tokenNames[i] == null) {
				tokenNames[i] = "<INVALID>";
			}
		}
	}

	@Override
	@Deprecated
	public String[] getTokenNames() {
		return tokenNames;
	}

	@Override

	public Vocabulary getVocabulary() {
		return VOCABULARY;
	}


	public BasicCompilerLexer(CharStream input) {
		super(input);
		_interp = new LexerATNSimulator(this,_ATN,_decisionToDFA,_sharedContextCache);
	}

	@Override
	public String getGrammarFileName() { return "BasicCompiler.g4"; }

	@Override
	public String[] getRuleNames() { return ruleNames; }

	@Override
	public String getSerializedATN() { return _serializedATN; }

	@Override
	public String[] getChannelNames() { return channelNames; }

	@Override
	public String[] getModeNames() { return modeNames; }

	@Override
	public ATN getATN() { return _ATN; }

	public static final String _serializedATN =
		"\u0004\u0000\u001a\u00ab\u0006\uffff\uffff\u0002\u0000\u0007\u0000\u0002"+
		"\u0001\u0007\u0001\u0002\u0002\u0007\u0002\u0002\u0003\u0007\u0003\u0002"+
		"\u0004\u0007\u0004\u0002\u0005\u0007\u0005\u0002\u0006\u0007\u0006\u0002"+
		"\u0007\u0007\u0007\u0002\b\u0007\b\u0002\t\u0007\t\u0002\n\u0007\n\u0002"+
		"\u000b\u0007\u000b\u0002\f\u0007\f\u0002\r\u0007\r\u0002\u000e\u0007\u000e"+
		"\u0002\u000f\u0007\u000f\u0002\u0010\u0007\u0010\u0002\u0011\u0007\u0011"+
		"\u0002\u0012\u0007\u0012\u0002\u0013\u0007\u0013\u0002\u0014\u0007\u0014"+
		"\u0002\u0015\u0007\u0015\u0002\u0016\u0007\u0016\u0002\u0017\u0007\u0017"+
		"\u0002\u0018\u0007\u0018\u0002\u0019\u0007\u0019\u0002\u001a\u0007\u001a"+
		"\u0002\u001b\u0007\u001b\u0002\u001c\u0007\u001c\u0002\u001d\u0007\u001d"+
		"\u0001\u0000\u0001\u0000\u0001\u0001\u0001\u0001\u0001\u0002\u0001\u0002"+
		"\u0001\u0003\u0001\u0003\u0001\u0004\u0001\u0004\u0001\u0004\u0001\u0004"+
		"\u0001\u0005\u0001\u0005\u0001\u0005\u0001\u0005\u0001\u0005\u0001\u0005"+
		"\u0001\u0006\u0001\u0006\u0001\u0006\u0001\u0006\u0001\u0006\u0001\u0006"+
		"\u0001\u0006\u0001\u0007\u0001\u0007\u0001\b\u0001\b\u0001\t\u0001\t\u0001"+
		"\n\u0001\n\u0001\u000b\u0001\u000b\u0001\f\u0001\f\u0001\r\u0001\r\u0001"+
		"\u000e\u0001\u000e\u0001\u000f\u0001\u000f\u0001\u0010\u0001\u0010\u0001"+
		"\u0011\u0001\u0011\u0001\u0011\u0001\u0012\u0001\u0012\u0001\u0012\u0001"+
		"\u0013\u0001\u0013\u0001\u0013\u0001\u0014\u0001\u0014\u0001\u0014\u0001"+
		"\u0015\u0001\u0015\u0001\u0016\u0001\u0016\u0001\u0016\u0001\u0017\u0001"+
		"\u0017\u0001\u0017\u0001\u0018\u0001\u0018\u0001\u0019\u0001\u0019\u0001"+
		"\u0019\u0005\u0019\u0084\b\u0019\n\u0019\f\u0019\u0087\t\u0019\u0003\u0019"+
		"\u0089\b\u0019\u0001\u001a\u0001\u001a\u0001\u001a\u0005\u001a\u008e\b"+
		"\u001a\n\u001a\f\u001a\u0091\t\u001a\u0001\u001a\u0001\u001a\u0001\u001b"+
		"\u0001\u001b\u0004\u001b\u0097\b\u001b\u000b\u001b\f\u001b\u0098\u0001"+
		"\u001b\u0001\u001b\u0001\u001c\u0001\u001c\u0001\u001c\u0005\u001c\u00a0"+
		"\b\u001c\n\u001c\f\u001c\u00a3\t\u001c\u0001\u001d\u0004\u001d\u00a6\b"+
		"\u001d\u000b\u001d\f\u001d\u00a7\u0001\u001d\u0001\u001d\u0001\u0098\u0000"+
		"\u001e\u0001\u0000\u0003\u0000\u0005\u0000\u0007\u0000\t\u0001\u000b\u0002"+
		"\r\u0003\u000f\u0004\u0011\u0005\u0013\u0006\u0015\u0007\u0017\b\u0019"+
		"\t\u001b\n\u001d\u000b\u001f\f!\r#\u000e%\u000f\'\u0010)\u0011+\u0012"+
		"-\u0013/\u00141\u00153\u00165\u00177\u00189\u0019;\u001a\u0001\u0000\u0004"+
		"\u0001\u000009\u0001\u000019\u0002\u0000AZaz\u0003\u0000\t\n\r\r  \u00ad"+
		"\u0000\t\u0001\u0000\u0000\u0000\u0000\u000b\u0001\u0000\u0000\u0000\u0000"+
		"\r\u0001\u0000\u0000\u0000\u0000\u000f\u0001\u0000\u0000\u0000\u0000\u0011"+
		"\u0001\u0000\u0000\u0000\u0000\u0013\u0001\u0000\u0000\u0000\u0000\u0015"+
		"\u0001\u0000\u0000\u0000\u0000\u0017\u0001\u0000\u0000\u0000\u0000\u0019"+
		"\u0001\u0000\u0000\u0000\u0000\u001b\u0001\u0000\u0000\u0000\u0000\u001d"+
		"\u0001\u0000\u0000\u0000\u0000\u001f\u0001\u0000\u0000\u0000\u0000!\u0001"+
		"\u0000\u0000\u0000\u0000#\u0001\u0000\u0000\u0000\u0000%\u0001\u0000\u0000"+
		"\u0000\u0000\'\u0001\u0000\u0000\u0000\u0000)\u0001\u0000\u0000\u0000"+
		"\u0000+\u0001\u0000\u0000\u0000\u0000-\u0001\u0000\u0000\u0000\u0000/"+
		"\u0001\u0000\u0000\u0000\u00001\u0001\u0000\u0000\u0000\u00003\u0001\u0000"+
		"\u0000\u0000\u00005\u0001\u0000\u0000\u0000\u00007\u0001\u0000\u0000\u0000"+
		"\u00009\u0001\u0000\u0000\u0000\u0000;\u0001\u0000\u0000\u0000\u0001="+
		"\u0001\u0000\u0000\u0000\u0003?\u0001\u0000\u0000\u0000\u0005A\u0001\u0000"+
		"\u0000\u0000\u0007C\u0001\u0000\u0000\u0000\tE\u0001\u0000\u0000\u0000"+
		"\u000bI\u0001\u0000\u0000\u0000\rO\u0001\u0000\u0000\u0000\u000fV\u0001"+
		"\u0000\u0000\u0000\u0011X\u0001\u0000\u0000\u0000\u0013Z\u0001\u0000\u0000"+
		"\u0000\u0015\\\u0001\u0000\u0000\u0000\u0017^\u0001\u0000\u0000\u0000"+
		"\u0019`\u0001\u0000\u0000\u0000\u001bb\u0001\u0000\u0000\u0000\u001dd"+
		"\u0001\u0000\u0000\u0000\u001ff\u0001\u0000\u0000\u0000!h\u0001\u0000"+
		"\u0000\u0000#j\u0001\u0000\u0000\u0000%m\u0001\u0000\u0000\u0000\'p\u0001"+
		"\u0000\u0000\u0000)s\u0001\u0000\u0000\u0000+v\u0001\u0000\u0000\u0000"+
		"-x\u0001\u0000\u0000\u0000/{\u0001\u0000\u0000\u00001~\u0001\u0000\u0000"+
		"\u00003\u0088\u0001\u0000\u0000\u00005\u008a\u0001\u0000\u0000\u00007"+
		"\u0094\u0001\u0000\u0000\u00009\u009c\u0001\u0000\u0000\u0000;\u00a5\u0001"+
		"\u0000\u0000\u0000=>\u0007\u0000\u0000\u0000>\u0002\u0001\u0000\u0000"+
		"\u0000?@\u0007\u0001\u0000\u0000@\u0004\u0001\u0000\u0000\u0000AB\u0005"+
		"0\u0000\u0000B\u0006\u0001\u0000\u0000\u0000CD\u0007\u0002\u0000\u0000"+
		"D\b\u0001\u0000\u0000\u0000EF\u0005i\u0000\u0000FG\u0005n\u0000\u0000"+
		"GH\u0005t\u0000\u0000H\n\u0001\u0000\u0000\u0000IJ\u0005f\u0000\u0000"+
		"JK\u0005l\u0000\u0000KL\u0005o\u0000\u0000LM\u0005a\u0000\u0000MN\u0005"+
		"t\u0000\u0000N\f\u0001\u0000\u0000\u0000OP\u0005s\u0000\u0000PQ\u0005"+
		"t\u0000\u0000QR\u0005r\u0000\u0000RS\u0005i\u0000\u0000ST\u0005n\u0000"+
		"\u0000TU\u0005g\u0000\u0000U\u000e\u0001\u0000\u0000\u0000VW\u0005(\u0000"+
		"\u0000W\u0010\u0001\u0000\u0000\u0000XY\u0005)\u0000\u0000Y\u0012\u0001"+
		"\u0000\u0000\u0000Z[\u0005{\u0000\u0000[\u0014\u0001\u0000\u0000\u0000"+
		"\\]\u0005}\u0000\u0000]\u0016\u0001\u0000\u0000\u0000^_\u0005^\u0000\u0000"+
		"_\u0018\u0001\u0000\u0000\u0000`a\u0005*\u0000\u0000a\u001a\u0001\u0000"+
		"\u0000\u0000bc\u0005/\u0000\u0000c\u001c\u0001\u0000\u0000\u0000de\u0005"+
		"+\u0000\u0000e\u001e\u0001\u0000\u0000\u0000fg\u0005-\u0000\u0000g \u0001"+
		"\u0000\u0000\u0000hi\u0005=\u0000\u0000i\"\u0001\u0000\u0000\u0000jk\u0005"+
		"+\u0000\u0000kl\u0005=\u0000\u0000l$\u0001\u0000\u0000\u0000mn\u0005-"+
		"\u0000\u0000no\u0005=\u0000\u0000o&\u0001\u0000\u0000\u0000pq\u0005*\u0000"+
		"\u0000qr\u0005=\u0000\u0000r(\u0001\u0000\u0000\u0000st\u0005/\u0000\u0000"+
		"tu\u0005=\u0000\u0000u*\u0001\u0000\u0000\u0000vw\u0005;\u0000\u0000w"+
		",\u0001\u0000\u0000\u0000xy\u0005&\u0000\u0000yz\u0005&\u0000\u0000z."+
		"\u0001\u0000\u0000\u0000{|\u0005|\u0000\u0000|}\u0005|\u0000\u0000}0\u0001"+
		"\u0000\u0000\u0000~\u007f\u0005!\u0000\u0000\u007f2\u0001\u0000\u0000"+
		"\u0000\u0080\u0089\u0003\u0005\u0002\u0000\u0081\u0085\u0003\u0003\u0001"+
		"\u0000\u0082\u0084\u0003\u0001\u0000\u0000\u0083\u0082\u0001\u0000\u0000"+
		"\u0000\u0084\u0087\u0001\u0000\u0000\u0000\u0085\u0083\u0001\u0000\u0000"+
		"\u0000\u0085\u0086\u0001\u0000\u0000\u0000\u0086\u0089\u0001\u0000\u0000"+
		"\u0000\u0087\u0085\u0001\u0000\u0000\u0000\u0088\u0080\u0001\u0000\u0000"+
		"\u0000\u0088\u0081\u0001\u0000\u0000\u0000\u00894\u0001\u0000\u0000\u0000"+
		"\u008a\u008b\u00033\u0019\u0000\u008b\u008f\u0005.\u0000\u0000\u008c\u008e"+
		"\u0003\u0001\u0000\u0000\u008d\u008c\u0001\u0000\u0000\u0000\u008e\u0091"+
		"\u0001\u0000\u0000\u0000\u008f\u008d\u0001\u0000\u0000\u0000\u008f\u0090"+
		"\u0001\u0000\u0000\u0000\u0090\u0092\u0001\u0000\u0000\u0000\u0091\u008f"+
		"\u0001\u0000\u0000\u0000\u0092\u0093\u0005f\u0000\u0000\u00936\u0001\u0000"+
		"\u0000\u0000\u0094\u0096\u0005\"\u0000\u0000\u0095\u0097\t\u0000\u0000"+
		"\u0000\u0096\u0095\u0001\u0000\u0000\u0000\u0097\u0098\u0001\u0000\u0000"+
		"\u0000\u0098\u0099\u0001\u0000\u0000\u0000\u0098\u0096\u0001\u0000\u0000"+
		"\u0000\u0099\u009a\u0001\u0000\u0000\u0000\u009a\u009b\u0005\"\u0000\u0000"+
		"\u009b8\u0001\u0000\u0000\u0000\u009c\u00a1\u0003\u0007\u0003\u0000\u009d"+
		"\u00a0\u0003\u0007\u0003\u0000\u009e\u00a0\u0003\u0001\u0000\u0000\u009f"+
		"\u009d\u0001\u0000\u0000\u0000\u009f\u009e\u0001\u0000\u0000\u0000\u00a0"+
		"\u00a3\u0001\u0000\u0000\u0000\u00a1\u009f\u0001\u0000\u0000\u0000\u00a1"+
		"\u00a2\u0001\u0000\u0000\u0000\u00a2:\u0001\u0000\u0000\u0000\u00a3\u00a1"+
		"\u0001\u0000\u0000\u0000\u00a4\u00a6\u0007\u0003\u0000\u0000\u00a5\u00a4"+
		"\u0001\u0000\u0000\u0000\u00a6\u00a7\u0001\u0000\u0000\u0000\u00a7\u00a5"+
		"\u0001\u0000\u0000\u0000\u00a7\u00a8\u0001\u0000\u0000\u0000\u00a8\u00a9"+
		"\u0001\u0000\u0000\u0000\u00a9\u00aa\u0006\u001d\u0000\u0000\u00aa<\u0001"+
		"\u0000\u0000\u0000\b\u0000\u0085\u0088\u008f\u0098\u009f\u00a1\u00a7\u0001"+
		"\u0006\u0000\u0000";
	public static final ATN _ATN =
		new ATNDeserializer().deserialize(_serializedATN.toCharArray());
	static {
		_decisionToDFA = new DFA[_ATN.getNumberOfDecisions()];
		for (int i = 0; i < _ATN.getNumberOfDecisions(); i++) {
			_decisionToDFA[i] = new DFA(_ATN.getDecisionState(i), i);
		}
	}
}