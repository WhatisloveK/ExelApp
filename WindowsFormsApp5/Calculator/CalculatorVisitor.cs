using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime.Misc;

namespace WindowsFormsApp5
{
    class CalculatorVisitor : CalculatorGrammarBaseVisitor<double>
    {

        MyHashTable myTable;


        public CalculatorVisitor(ref MyHashTable myTable)
        {
            this.myTable = myTable;
        }

        //public override double Visit
        public override double VisitCompileUnit(CalculatorGrammarParser.CompileUnitContext context)
        {
            return Visit(context.expression());
        }

     

        public override double VisitNumberExpr(CalculatorGrammarParser.NumberExprContext context)
        {
            var result = double.Parse(context.GetText());
            Debug.WriteLine(result);

            return result;
        }

        //IdentifierExpr
        public override double VisitIdentifierExpr(CalculatorGrammarParser.IdentifierExprContext context)
        {
            var result = context.GetText();
            if(myTable.formulas.Contains(result))
            {
                return Calculator.Evaluate(myTable.getFormula(result),ref myTable);
            }
            return 0;
           
        }

        public override double VisitParenthesizedExpr(CalculatorGrammarParser.ParenthesizedExprContext context)
        {
            return Visit(context.expression());
        }

        public override double VisitExponentialExpr(CalculatorGrammarParser.ExponentialExprContext context)
        {
            var left = WalkLeft(context);
            var right = WalkRight(context);

            Debug.WriteLine("{0} ^ {1}", left, right);
            return System.Math.Pow(left, right);
        }

        public override double VisitAdditiveExpr(CalculatorGrammarParser.AdditiveExprContext context)
        {
            var left = WalkLeft(context);
            var right = WalkRight(context);

            if (context.operatorToken.Type == CalculatorGrammarLexer.ADD)
            {
                Debug.WriteLine("{0} + {1}", left, right);
                return left + right;
            }
            else //GrammarLexer.SUBTRACT
            {
                Debug.WriteLine("{0} - {1}", left, right);
                return left - right;
            }
        }

        public override double VisitBoolExpr(CalculatorGrammarParser.BoolExprContext context)
        {
            var left = WalkLeft(context);
            var right = WalkRight(context);
            string op;
            double res;

            if (context.operatorToken.Type == CalculatorGrammarLexer.LEFTTRIANGLE)
            {

                if (left < right)
                {
                    op = "<";
                    res = 1;
                }
                else
                {
                    op = ">";
                    res = 0;
                }
            }
            else if (context.operatorToken.Type == CalculatorGrammarLexer.RIGHTTRIANGLE)
            {

                if (left > right)
                {
                    op = ">";
                    res = 1;
                }
                else
                {
                    op = "<";
                    res = 0;
                }
            }
            else if (context.operatorToken.Type == CalculatorGrammarLexer.EQUAL)//GrammarLexer.EQUAL
            {
                if (left == right)
                {
                    op = "=";
                    res = 1;
                }
                else
                {
                    op = "!=";
                    res = 0;
                }
            }
            else if (context.operatorToken.Type == CalculatorGrammarLexer.RTRIANGLEEQUAL)
            {

                if (left >= right)
                {
                    op = ">=";
                    res = 1;
                }
                else
                {
                    op = "<=";
                    res = 0;
                }
            }
            else if (context.operatorToken.Type == CalculatorGrammarLexer.LTRIANGLEEQUAL)
            {
                if (left <= right)
                {
                    op = "<=";
                    res = 1;
                }
                else
                {
                    op = ">=";
                    res = 0;
                }
            }
            else //CalculatorGrammarLexer.NOTEQUAL
            {
                if (left != right)
                {
                    op = "!=";
                    res = 1;
                }
                else
                {
                    op = "=";
                    res = 0;
                }
            }
            Debug.WriteLine("{0} " + op + "{1}", left, right);
            return res;

        }

        public override double VisitOptimumfunctions(CalculatorGrammarParser.OptimumfunctionsContext context)
        {
            var left = WalkLeft(context);
            var right = WalkRight(context);
            if (context.operatorToken.Type == CalculatorGrammarLexer.MAX)
            {
                if (left >= right)
                {
                    Debug.WriteLine("Max " + left);
                    return left;
                }
                else
                {
                    Debug.WriteLine("Max " + right);
                    return right;
                }
            }
            else //GrammarLexer.MIN
            {
                if (left <= right)
                {
                    Debug.WriteLine("Min " + left);
                    return left;
                }
                else
                {
                    Debug.WriteLine("Min " + right);
                    return right;
                }
            }

        }

        public override double VisitMultiplicativeExpr(CalculatorGrammarParser.MultiplicativeExprContext context)
        {
            var left = WalkLeft(context);
            var right = WalkRight(context);

            if (context.operatorToken.Type == CalculatorGrammarLexer.MULTIPLY)
            {
                Debug.WriteLine("{0} * {1}", left, right);
                return left * right;
            }
            else //GrammarLexer.DIVIDE
            {
                Debug.WriteLine("{0} / {1}", left, right);
                return left / right;
            }
        }

        private double WalkLeft(CalculatorGrammarParser.ExpressionContext context)
        {
            return Visit(context.GetRuleContext<CalculatorGrammarParser.ExpressionContext>(0));
        }

        private double WalkRight(CalculatorGrammarParser.ExpressionContext context)
        {
            return Visit(context.GetRuleContext<CalculatorGrammarParser.ExpressionContext>(1));
        }

    }
}
