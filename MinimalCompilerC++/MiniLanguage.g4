grammar MiniLanguage;

//parser rules

declaration: type VARIABLE_NAME EQUALS value SEMICOLON;
type: INTEGER_TYPE | FLOAT_TYPE | STRING_TYPE;
value: INTEGER_VALUE | FLOAT_VALUE | STRING_VALUE;
assignment: (type)? VARIABLE_NAME EQUALS value SEMICOLON;
assignment_operator:
	ADD_EQUALS
	| SUB_EQUALS
	| MUL_EQUALS
	| DIV_EQUALS;

//lexer rules

fragment DIGIT: [0-9];
fragment NON_ZERO_DIGIT: [1-9];
fragment ZERO: '0';
fragment LETTER: [a-zA-Z];

INTEGER_TYPE: 'int';
FLOAT_TYPE: 'float';
STRING_TYPE: 'string';

LPARAN: '(';
RPARAN: ')';
LBRACE: '{';
RBRACE: '}';

EXP: '^';
ASTERISK: '*';
SLASH: '/';
PLUS: '+';
MINUS: '-';

EQUALS: '=';
ADD_EQUALS: '+=';
SUB_EQUALS: '-=';
MUL_EQUALS: '*=';
DIV_EQUALS: '/=';

SEMICOLON: ';';

AND: '&&';
OR: '||';
NOT: '!';

INTEGER_VALUE: ZERO | NON_ZERO_DIGIT DIGIT*;
FLOAT_VALUE: INTEGER_VALUE '.' DIGIT* ('f')?;
STRING_VALUE: '"' .+? '"';
VARIABLE_NAME: LETTER (LETTER | DIGIT)*;

WS: [ \t\r\n]+ -> skip;
COMMENT: '//' ~[\r\n]* -> skip;