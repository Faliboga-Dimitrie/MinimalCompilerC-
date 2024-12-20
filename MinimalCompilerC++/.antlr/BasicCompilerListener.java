// Generated from c:/Users/Dimitrie_U/1Facultate/Anul2/Semestrul1/LFC/Proiecte/Proiect2/MinimalCompilerC++/MinimalCompilerC++/BasicCompiler.g4 by ANTLR 4.13.1
import org.antlr.v4.runtime.tree.ParseTreeListener;

/**
 * This interface defines a complete listener for a parse tree produced by
 * {@link BasicCompilerParser}.
 */
public interface BasicCompilerListener extends ParseTreeListener {
	/**
	 * Enter a parse tree produced by {@link BasicCompilerParser#declaration}.
	 * @param ctx the parse tree
	 */
	void enterDeclaration(BasicCompilerParser.DeclarationContext ctx);
	/**
	 * Exit a parse tree produced by {@link BasicCompilerParser#declaration}.
	 * @param ctx the parse tree
	 */
	void exitDeclaration(BasicCompilerParser.DeclarationContext ctx);
	/**
	 * Enter a parse tree produced by {@link BasicCompilerParser#type}.
	 * @param ctx the parse tree
	 */
	void enterType(BasicCompilerParser.TypeContext ctx);
	/**
	 * Exit a parse tree produced by {@link BasicCompilerParser#type}.
	 * @param ctx the parse tree
	 */
	void exitType(BasicCompilerParser.TypeContext ctx);
	/**
	 * Enter a parse tree produced by {@link BasicCompilerParser#value}.
	 * @param ctx the parse tree
	 */
	void enterValue(BasicCompilerParser.ValueContext ctx);
	/**
	 * Exit a parse tree produced by {@link BasicCompilerParser#value}.
	 * @param ctx the parse tree
	 */
	void exitValue(BasicCompilerParser.ValueContext ctx);
}