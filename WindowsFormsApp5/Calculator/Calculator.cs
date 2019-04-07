using System;
using Antlr4.Runtime;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp5
{
    public static class Calculator
    {
        public static double Evaluate(string expression,ref MyHashTable myTable)
        {
            var lexer = new CalculatorGrammarLexer(new AntlrInputStream(expression));
            lexer.RemoveErrorListeners();
            lexer.AddErrorListener(new ThrowExceptionErrorListener());

            var tokens = new CommonTokenStream(lexer);
            var parser = new CalculatorGrammarParser(tokens);

            var tree = parser.compileUnit();

            var visitor = new CalculatorVisitor(ref myTable);

            return visitor.Visit(tree);
        }
    }
}
