grammar MiniLanguage;

//parser rules

program: (
		declaration
		| declaration_and_assignment
		| function_declaration
		| COMMENT
	)* main_declaration COMMENT* EOF;

main_declaration:
	return_type MAIN_FUNCTION LPARAN RPARAN (
		COMMENT? new_line_block
		| block
	);

function_declaration:
	return_type VARIABLE_NAME LPARAN parameters? RPARAN (
		COMMENT? new_line_block
		| block
	);
parameters: type VARIABLE_NAME (COMMA type VARIABLE_NAME)*;
new_line_block: NEW_LINE block;
block: LBRACE (COMMENT | statement)* RBRACE;

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
	IF LPARAN logical_expression RPARAN (
		COMMENT? new_line_block
		| block
	) (ELSE (COMMENT? new_line_block | block))?;

while_statement:
	WHILE LPARAN expression RPARAN (
		COMMENT? new_line_block
		| block
	);

for_statement:
	FOR LPARAN for_clause RPARAN (
		COMMENT? new_line_block
		| block
	);
for_clause: (
		assignment_no_semicolon
		| declaration_and_assignment_no_semicolon
	)? SEMICOLON expression SEMICOLON (
		assignment_op_no_semicolon
		| (
			prev_inccrement_or_decrement_no_semicolon
			| post_inccrement_or_decrement_no_semicolon
		)
	)?;

return_statement: RETURN expression SEMICOLON COMMENT?;

function_call_no_semicolon:
	VARIABLE_NAME LPARAN arguments? RPARAN;
function_call: VARIABLE_NAME LPARAN arguments? RPARAN SEMICOLON;
arguments: VARIABLE_NAME (COMMA VARIABLE_NAME)*;

expression: logical_expression;

logical_expression:
	relational_or_equality_expression
	| relational_or_equality_expression (AND | OR) relational_or_equality_expression;

relational_or_equality_expression:
	additive_or_subtractive_expression
	| additive_or_subtractive_expression (
		EQUAL
		| NOT_EQUAL
		| LESS_THAN
		| LESS_THAN_OR_EQUAL
		| GREATER_THAN
		| GREATER_THAN_OR_EQUAL
	) additive_or_subtractive_expression;

additive_or_subtractive_expression:
	multiplicative_expression
	| multiplicative_expression (PLUS | MINUS) multiplicative_expression;

multiplicative_expression:
	unary_expression
	| unary_expression (ASTERISK | SLASH) unary_expression;

unary_expression: (NOT | MINUS)? primary_expression;

primary_expression:
	VARIABLE_NAME
	| numeral_value
	| STRING_VALUE;

declaration_and_assignment_no_semicolon:
	type VARIABLE_NAME EQUALS value;
declaration_and_assignment:
	type VARIABLE_NAME EQUALS value SEMICOLON COMMENT?;
declaration: type VARIABLE_NAME SEMICOLON COMMENT?;
type: INTEGER_TYPE | FLOAT_TYPE | STRING_TYPE | DOUBLE_TYPE;

return_type: type | VOID_TYPE;

value:
	numeral_value
	| VARIABLE_NAME
	| function_call_no_semicolon
	| STRING_VALUE
	| expression;
numeral_value: INTEGER_VALUE | FLOAT_VALUE | DOUBLE_VALUE;

assignment_no_semicolon: VARIABLE_NAME EQUALS value;
assignment: VARIABLE_NAME EQUALS value SEMICOLON COMMENT?;
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
NEW_LINE: '\n';

INTEGER_TYPE: 'int';
FLOAT_TYPE: 'float';
DOUBLE_TYPE: 'double';
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
DOUBLE_VALUE: INTEGER_VALUE '.' DIGIT* ('d')?;
STRING_VALUE: '"' .+? '"';
VARIABLE_NAME: LETTER (LETTER | DIGIT)*;

WS: [ \t\r\n]+ -> skip;
COMMENT: '//' ~[\r\n]* -> skip;