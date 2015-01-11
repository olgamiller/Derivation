/*
Derivation

Written in 2015 by <Olga Miller> <olga.rgb@googlemail.com>
To the extent possible under law, the author(s) have dedicated all copyright and related and neighboring rights to this software to the public domain worldwide.
This software is distributed without any warranty.
You should have received a copy of the CC0 Public Domain Dedication along with this software. If not, see <http://creativecommons.org/publicdomain/zero/1.0/>.
*/

using Derivation.Nodes;

namespace Derivation.Parsing
{
    internal class FunctionTreeBuilder
    {
        #region Member Variables

        // expr1 op1 expr2 op2 expr3 op3  expr
        // example: 1 + 2 * 3 ^  4

        private IOperator mOperator1;
        private IOperator mOperator2;
        private IOperator mOperator3;

        private Node mExpression1;
        private Node mExpression2;
        private Node mExpression3;

        #endregion

        internal bool SetOperator(IOperator op)
        {
            bool noError = true;

            if (mOperator1 == null)
                noError = SetOperator1(op);
            else if (mOperator2 == null)
                noError = SetOperator2(op);
            else
                noError = SetOperator3(op);

            return noError;
        }

        internal bool LeftIsEmpty()
        {
            return mExpression1 == null;
        }

        internal bool SetExpression(Node expr)
        {
            bool noError = true;

            if (mExpression1 == null)
                SetExpression1(expr);
            else if (mExpression2 == null)
                noError = SetExpression2(expr);
            else
                noError = SetExpression3(expr);

            return noError;
        }

        internal Node GetResult()
        {
            if (mOperator2 != null)
            {
                if (mExpression3 == null)
                    return null;

                ApplyOperator2(mExpression3);
            }

            if (mOperator1 != null)
            {
                if (mExpression2 == null)
                    return null;

                ApplyOperator1(mExpression2);
            }

            return mExpression1;
        }

        #region Set Operators

        private bool SetOperator1(IOperator op)
        {
            if (mExpression1 == null && !(op is IUnaryOperator))
                return false;

            mOperator1 = op;

            return true;
        }

        private bool SetOperator2(IOperator op)
        {
            if (mExpression2 == null)
                return false;

            if (op.Prio <= mOperator1.Prio)
            {
                ApplyOperator1(mExpression2);
                mOperator1 = op;
            }
            else
            {
                mOperator2 = op;
            }

            return true;
        }

        private bool SetOperator3(IOperator op)
        {
            if (mExpression3 == null)
                return false;

            if (op.Prio <= mOperator2.Prio)
            {
                ApplyOperator2(mExpression3);

                if (op.Prio <= mOperator1.Prio)
                {
                    ApplyOperator1(mExpression2);
                    mOperator1 = op;
                }
                else
                {
                    mOperator2 = op;
                }
            }
            else
            {
                mOperator3 = op;
            }

            return true;
        }

        #endregion

        #region Set Expressions

        private void SetExpression1(Node expr)
        {
            if (mOperator1 != null && mOperator1 is IUnaryOperator)
                mExpression2 = expr;

            mExpression1 = expr;
        }

        private bool SetExpression2(Node expr)
        {
            if (mOperator1 == null)
                return false;

            if (mOperator1.Prio < 3)
                mExpression2 = expr;
            else
                ApplyOperator1(expr);

            return true;
        }

        private bool SetExpression3(Node expr)
        {
            if (mOperator2 == null)
                return false;

            if (mExpression3 == null)
            {
                if (mOperator2.Prio == 2)
                    mExpression3 = expr;
                else
                    ApplyOperator2(expr);
            }
            else
            {
                ApplyOperator3(expr);
            }

            return true;
        }

        #endregion

        #region Apply Operators

        private void ApplyOperator1(Node expr)
        {
            mExpression1 = mOperator1.Apply(mExpression1, expr);
            mOperator1 = null;
            mExpression2 = null;
        }

        private void ApplyOperator2(Node expr)
        {
            mExpression2 = mOperator2.Apply(mExpression2, expr);
            mOperator2 = null;
            mExpression3 = null;
        }

        private void ApplyOperator3(Node expr)
        {
            mExpression3 = mOperator3.Apply(mExpression3, expr);
            mOperator3 = null;
        }

        #endregion
    }
}
