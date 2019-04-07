grammar CalculatorGrammar;

/*
 * Parser Rules
 */

compileUnit : expression EOF;

expression :
	LPAREN expression RPAREN #ParenthesizedExpr
	| expression EXPONENT expression #ExponentialExpr
	| operatorToken=(MAX | MIN) LPAREN expression ',' expression RPAREN #Optimumfunctions
    | expression operatorToken=(MULTIPLY | DIVIDE) expression #MultiplicativeExpr
 	| expression operatorToken=(ADD | SUBTRACT) expression #AdditiveExpr
	| expression operatorToken=(RIGHTTRIANGLE | LEFTTRIANGLE|EQUAL|RTRIANGLEEQUAL|LTRIANGLEEQUAL|NOTEQUAL) expression #BoolExpr
	//| expression operatorToken
	| NUMBER #NumberExpr
	| IDENTIFIER #IdentifierExpr
	; 

/*
 * Lexer Rules
 */

NUMBER : INT ('.' INT)?; 
IDENTIFIER : [A-Z]+'.'[0-9][0-9]*;

INT : ('0'..'9')+;
MAX : 'max';
MIN : 'min';
EQUAL : '=';
RTRIANGLEEQUAL : '>=';
LTRIANGLEEQUAL : '<=';
NOTEQUAL : '<>';
RIGHTTRIANGLE : '>';
LEFTTRIANGLE : '<';
EXPONENT : '^';
MULTIPLY : '*';
DIVIDE : '/';
SUBTRACT : '-'; 
ADD : '+';
LPAREN : '(';
RPAREN : ')';

WS : [ \t\r\n] -> channel(HIDDEN);

/*
(...)+ Повторення підправила 1 чи більше разів
(...)? підправило, може бути відсутнє
{...} семантичні дії
[...] параметри правила
| оператор альтернативи
.. оператор діапазона
~ заперечення
. довільний символ
= присвоювання
: мітка початку
; мітка закінчення правила
*/