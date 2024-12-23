grammar MiniLanguage;

//parser rules

program: function_declaration* main_declaration EOF;

main_declaration:
	VOID_TYPE
	| INTEGER_TYPE MAIN_FUNCTION LPARAN RPARAN block;

function_declaration:
	type VARIABLE_NAME LPARAN parameters? RPARAN block;
parameters: type VARIABLE_NAME (COMMA type VARIABLE_NAME)*;
block: LBRACE statement* RBRACE;

statement:
	assignment
	| assignment_op
	| declaration
	| declaration_and_assignment
	| function_call
	| if_statement
	| while_statement
	| for_statement
	| return_statement;

if_statement:
	IF LPARAN logical_expression RPARAN block (ELSE block)?;
while_statement: WHILE LPARAN expression RPARAN block;
for_statement: FOR LPARAN for_clause RPARAN block;
for_clause: (
		assignment_no_semicolon
		| declaration_and_assignment_no_semicolon
	)? SEMICOLON expression SEMICOLON (
		assignment_op_no_semicolon
		| (
			prev_inccrement_or_decrement_no_semicolon
			| post_inccrement_or_decrement_no_semicolon
		)
	)? SEMICOLON;
return_statement: RETURN expression SEMICOLON;

function_call: VARIABLE_NAME LPARAN arguments? RPARAN SEMICOLON;
arguments: expression (COMMA expression)*;

expression: logical_expression;

logical_expression:
	relational_or_equality_expression (
		(AND | OR) relational_or_equality_expression
	)*;

relational_or_equality_expression:
	additive_or_subtractive_expression (
		EQUAL
		| NOT_EQUAL
		| LESS_THAN
		| LESS_THAN_OR_EQUAL
		| GREATER_THAN
		| GREATER_THAN_OR_EQUAL additive_or_subtractive_expression
	) additive_or_subtractive_expression;

additive_or_subtractive_expression:
	multiplicative_expression (
		(PLUS | MINUS) multiplicative_expression
	)*;

multiplicative_expression:
	unary_expression ((ASTERISK | SLASH) unary_expression)*;

unary_expression: (NOT | MINUS)? primary_expression
	| (STRING_VALUE | LPARAN STRING_VALUE RPARAN);

primary_expression:
	VARIABLE_NAME
	| numeral_value
	| LPARAN primary_expression RPARAN;

declaration_and_assignment_no_semicolon:
	type VARIABLE_NAME EQUALS value;
declaration_and_assignment:
	type VARIABLE_NAME EQUALS value SEMICOLON;
declaration: type VARIABLE_NAME SEMICOLON;
type: INTEGER_TYPE | FLOAT_TYPE | STRING_TYPE;

value: numeral_value | VARIABLE_NAME;
numeral_value: INTEGER_VALUE | FLOAT_VALUE;

assignment_no_semicolon: VARIABLE_NAME EQUALS value;
assignment: VARIABLE_NAME EQUALS value SEMICOLON;
assignment_op_no_semicolon:
	VARIABLE_NAME (
		ADD_EQUALS
		| SUB_EQUALS
		| MUL_EQUALS
		| DIV_EQUALS
	) value;
assignment_op:
	VARIABLE_NAME (
		ADD_EQUALS
		| SUB_EQUALS
		| MUL_EQUALS
		| DIV_EQUALS
	) value SEMICOLON;

post_inccrement_or_decrement_no_semicolon:
	VARIABLE_NAME (INC | DEC);
post_inccrement_or_decrement:
	VARIABLE_NAME (INC | DEC) SEMICOLON;
prev_inccrement_or_decrement_no_semicolon: (INC | DEC) VARIABLE_NAME;
prev_inccrement_or_decrement: (INC | DEC) VARIABLE_NAME SEMICOLON;

//lexer rules

fragment DIGIT: [0-9];
fragment NON_ZERO_DIGIT: [1-9];
fragment LETTER: [a-zA-Z];

MAIN_FUNCTION: 'main';

INTEGER_TYPE: 'int';
FLOAT_TYPE: 'float';
STRING_TYPE: 'string';
VOID_TYPE: 'void';

IF: 'if';
ELSE: 'else';
WHILE: 'while';
FOR: 'for';
RETURN: 'return';

ASTERISK: '*';
SLASH: '/';
PLUS: '+';
MINUS: '-';

LESS_THAN: '<';
LESS_THAN_OR_EQUAL: '<=';
GREATER_THAN: '>';
GREATER_THAN_OR_EQUAL: '>=';
EQUAL: '==';
NOT_EQUAL: '!=';

AND: '&&';
OR: '||';
NOT: '!';

EQUALS: '=';
ADD_EQUALS: '+=';
SUB_EQUALS: '-=';
MUL_EQUALS: '*=';
DIV_EQUALS: '/=';
INC: '++';
DEC: '--';

LPARAN: '(';
RPARAN: ')';
LBRACE: '{';
RBRACE: '}';
SEMICOLON: ';';
COMMA: ',';

INTEGER_VALUE: DIGIT | NON_ZERO_DIGIT DIGIT*;
FLOAT_VALUE: INTEGER_VALUE '.' DIGIT* ('f')?;
STRING_VALUE: '"' .+? '"';
VARIABLE_NAME: LETTER (LETTER | DIGIT)*;

WS: [ \t\r\n]+ -> skip;
COMMENT: '//' ~[\r\n]* -> skip;