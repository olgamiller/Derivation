/*
Derivation
  
Written in 2015 by <Olga Miller> <olga.rgb@googlemail.com>
To the extent possible under law, the author(s) have dedicated all copyright and related and neighboring rights to this software to the public domain worldwide.
This software is distributed without any warranty.
You should have received a copy of the CC0 Public Domain Dedication along with this software. If not, see <http://creativecommons.org/publicdomain/zero/1.0/>.
*/

using Derivation.Nodes;
using Derivation.Properties;

namespace Derivation.Parsing
{
    public class FunctionParser
    {
        private int mPos;
        private string mInput;
        private Parameters mParams;

        /// <exception cref="SyntaxException"/>
        public FunctionTree Parse(string input)
        {
            mPos = 0;
            mInput = input;
            mParams = new Parameters();

            return new FunctionTree(mParams, Parse(false));
        }

        private Node Parse(bool bParentheses)
        {
            FunctionTreeBuilder ec = new FunctionTreeBuilder();

            while (mPos < mInput.Length)
            {
                switch (mInput[mPos])
                {
                    case ' ':
                        break;
                    case '0':
                    case '1':
                    case '2':
                    case '3':
                    case '4':
                    case '5':
                    case '6':
                    case '7':
                    case '8':
                    case '9':
                    case '.':
                        HandleNumber(ec);
                        break;
                    case 'x':
                    case 'X':
                        SetExpression(ec, mParams.X);
                        break;
                    case 'y':
                    case 'Y':
                        SetExpression(ec, mParams.Y);
                        break;
                    case 'z':
                    case 'Z':
                        SetExpression(ec, mParams.Z);
                        break;
                    case 't':
                    case 'T':
                        SetExpression(ec, mParams.T);
                        break;
                    case '+':
                        SetOperator(ec, new OperatorAdd());
                        break;
                    case '-':
                        HandleMinus(ec);
                        break;
                    case '/':
                        SetOperator(ec, new OperatorDivide());
                        break;
                    case '*':
                        SetOperator(ec, new OperatorMultiply());
                        break;
                    case '^':
                        SetOperator(ec, new OperatorPower());
                        break;
                    case '(':
                        mPos++;
                        SetExpression(ec, Parse(true));
                        break;
                    case ')':
                        if (!bParentheses)
                            throw new ParenthesisException(mInput, mPos, Resources.Opening);
                        return GetResultExpression(ec);
                    case 'c':
                    case 'C':
                        if (ConsumeWord("cos"))
                        {
                            ConsumeOpeningParenthesis();
                            SetExpression(ec, Node.Cos(Parse(true)));
                        }
                        else
                            throw new UnknownWordException(mInput, mPos);
                        break;
                    case 'e':
                    case 'E':
                        if (ConsumeWord("exp"))
                        {
                            ConsumeOpeningParenthesis();
                            SetExpression(ec, Node.Exp(Parse(true)));
                        }
                        else
                            SetExpression(ec, Node.E());
                        break;
                    case 'l':
                    case 'L':
                        if (ConsumeWord("ln"))
                        {
                            ConsumeOpeningParenthesis();
                            SetExpression(ec, Node.Ln(Parse(true)));
                        }
                        else
                            throw new UnknownWordException(mInput, mPos);
                        break;
                    case 'p':
                    case 'P':
                        if (ConsumeWord("pi"))
                            SetExpression(ec, Node.PI());
                        else
                            throw new UnknownWordException(mInput, mPos);
                        break;
                    case 's':
                    case 'S':
                        if (ConsumeWord("sin"))
                        {
                            ConsumeOpeningParenthesis();
                            SetExpression(ec, Node.Sin(Parse(true)));
                        }
                        else if (ConsumeWord("sqrt"))
                        {
                            ConsumeOpeningParenthesis();
                            SetExpression(ec, Node.Sqrt(Parse(true)));
                        }
                        else
                            throw new UnknownWordException(mInput, mPos);
                        break;
                    default:
                        throw new UnknownTokenException(mInput, mPos);
                }

                mPos++;
            }

            if (bParentheses)
                throw new ParenthesisException(mInput, mPos, Resources.Closing);

            return GetResultExpression(ec);
        }

        private void HandleNumber(FunctionTreeBuilder ec)
        {
            bool point = mInput[mPos] == '.';
            int begin = mPos++;

            while (mPos < mInput.Length)
            {
                if (mInput[mPos] == '.')
                {
                    if (point)
                        throw new SyntaxException(mInput, mPos);

                    point = true;
                }
                else if (!char.IsDigit(mInput[mPos]))
                {
                    break;
                }

                mPos++;
            }

            mPos--;

            if (mInput[mPos] == '.')
                throw new SyntaxException(mInput, mPos);

            double d = double.Parse(mInput.Substring(begin, mPos - begin + 1));
            SetExpression(ec, Node.Number(d));
        }

        private void SetExpression(FunctionTreeBuilder ec, Node expr)
        {
            if (!ec.SetExpression(expr))
                throw new OperatorException(mInput, mPos);
        }

        private void HandleMinus(FunctionTreeBuilder ec)
        {
            if (ec.LeftIsEmpty())
                SetOperator(ec, new OperatorNegate());
            else
                SetOperator(ec, new OperatorSubtract());
        }

        private void SetOperator(FunctionTreeBuilder ec, IOperator op)
        {
            if (!ec.SetOperator(op))
                throw new EmptyExpressionException(mInput, mPos);
        }

        private bool ConsumeWord(string name)
        {
            if ((mPos + name.Length) <= mInput.Length && mInput.Substring(mPos, name.Length).ToLower() == name)
            {
                mPos += name.Length - 1;
                return true;
            }

            return false;
        }

        private void ConsumeOpeningParenthesis()
        {
            mPos++;

            while (mPos < mInput.Length && mInput[mPos] == ' ')
                mPos++;

            if (mPos >= mInput.Length || mInput[mPos] != '(')
                throw new ParenthesisException(mInput, mPos, Resources.Opening);

            mPos++;
        }

        private Node GetResultExpression(FunctionTreeBuilder ec)
        {
            Node result = ec.GetResult();

            if (result == null)
                throw new EmptyExpressionException(mInput, mPos);

            return result;
        }
    }
}

