﻿using Njsast.Output;
using Njsast.Reader;

namespace Njsast.Ast
{
    /// A `with` statement
    public class AstWith : AstStatementWithBody
    {
        /// [AstNode] the `with` expression
        public AstNode Expression;

        public AstWith(Parser parser, Position startPos, Position endPos, AstStatement body, AstNode expression) : base(
            parser, startPos, endPos, body)
        {
            Expression = expression;
        }

        public override void Visit(TreeWalker w)
        {
            w.Walk(Expression);
            base.Visit(w);
        }

        public override void CodeGen(OutputContext output)
        {
            output.Print("with");
            output.Space();
            output.Print("(");
            Expression.Print(output);
            output.Print(")");
            output.Space();
            output.PrintBody(Body);
        }
    }
}