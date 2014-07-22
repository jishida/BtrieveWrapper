using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace BtrieveWrapper.Orm
{
    class ExpressionFilter
    {
        public ExpressionFilter(Expression expression, ParameterExpression argument) {
            var isNot = false;
            while (expression.NodeType == ExpressionType.Not) {
                isNot = !isNot;
                expression = ((UnaryExpression)expression).Operand;
            }

            this.ComparisonType = FilterComparison.NotComparable;
            if (expression.GetField(argument) != null) {
                expression = Expression.MakeBinary(ExpressionType.Equal, expression, Expression.Constant(isNot ? false : true));
            }
            switch (expression.NodeType) {
                case ExpressionType.Equal:
                case ExpressionType.NotEqual:
                case ExpressionType.GreaterThan:
                case ExpressionType.LessThan:
                case ExpressionType.GreaterThanOrEqual:
                case ExpressionType.LessThanOrEqual:
                    var binaryExpression = (BinaryExpression)expression;
                    if (isNot) {
                        binaryExpression = binaryExpression.ToReversedExpression();
                    }
                    var left = binaryExpression.Left.GetField(argument);
                    var right = binaryExpression.Right.GetField(argument);
                    if (left == null && right != null) {
                        binaryExpression = binaryExpression.FlipBinary();
                    }
                    if (left != null && right != null && left.NullType != NullType.Nullable && right.NullType == NullType.Nullable) {
                        binaryExpression = binaryExpression.FlipBinary();
                    }
                    this.Left = binaryExpression.Left.GetField(argument);
                    this.Right = binaryExpression.Right.GetField(argument);
                    if (this.Left != null) {
                        if(this.Left.NullType == NullType.NullFlag){
                            throw new ArgumentException();
                        }
                        if (this.Right == null) {
                            if (!binaryExpression.Right.HasArgument(argument)) {
                                this.ComparisonType = FilterComparison.ToConstant;
                                this.Value = binaryExpression.Right.ToValue();
                                binaryExpression = Expression.MakeBinary(
                                    binaryExpression.NodeType,
                                    binaryExpression.Left,
                                    Expression.Constant(this.Value));
                            }
                        } else {
                            if (this.Left.Length != this.Right.Length ||
                                this.Left.KeyType != this.Right.KeyType) {
                                throw new ArgumentException();
                            }
                            this.ComparisonType = FilterComparison.ToField;
                            this.Value = this.Right;
                        }
                    }
                    this.Expression = binaryExpression;
                    switch (this.Expression.NodeType) {
                        case ExpressionType.Equal:
                            this.FilterType = Orm.FilterType.Equal;
                            break;
                        case ExpressionType.NotEqual:
                            this.FilterType = Orm.FilterType.NotEqual;
                            break;
                        case ExpressionType.GreaterThan:
                            this.FilterType = Orm.FilterType.GreaterThan;
                            break;
                        case ExpressionType.GreaterThanOrEqual:
                            this.FilterType = Orm.FilterType.GreaterThanOrEqual;
                            break;
                        case ExpressionType.LessThan:
                            this.FilterType = Orm.FilterType.LessThan;
                            break;
                        case ExpressionType.LessThanOrEqual:
                            this.FilterType = Orm.FilterType.LessThanOrEqual;
                            break;
                        default:
                            throw new NotSupportedException();
                    }
                    break;
                case ExpressionType.Call:
                    var callExpression = (MethodCallExpression)expression;
                    if (callExpression.Method.IsComparable()) {
                        if (isNot) {
                            callExpression = callExpression.ToReversedExpression();
                        }
                        var arg0 = callExpression.Arguments[0].GetField(argument);
                        var arg1 = callExpression.Arguments[1].GetField(argument);
                        if (arg0 == null && arg1 != null) {
                            callExpression = callExpression.FlipStringBinary();
                        }
                        if (arg0 != null && arg1 != null && arg0.NullType != NullType.Nullable && arg1.NullType == NullType.Nullable) {
                            callExpression = callExpression.FlipStringBinary();
                        }
                        this.Left = callExpression.Arguments[0].GetField(argument);
                        this.Right = callExpression.Arguments[1].GetField(argument);
                        if (this.Left != null) {
                            if (this.Left.NullType == NullType.NullFlag) {
                                throw new ArgumentException();
                            }
                            if (this.Right == null) {
                                if (!callExpression.Arguments[1].HasArgument(argument)) {
                                    this.ComparisonType = FilterComparison.ToConstant;
                                    this.Value = callExpression.Arguments[1].ToValue();
                                    callExpression = Expression.Call(
                                        callExpression.Method,
                                        callExpression.Arguments[0],
                                        Expression.Constant(this.Value));
                                }
                            } else {
                                if (this.Left.Length != this.Right.Length ||
                                    this.Left.KeyType != this.Right.KeyType) {
                                    throw new ArgumentException();
                                }
                                this.ComparisonType = FilterComparison.ToField;
                                this.Value = this.Right;
                            }
                        }
                        this.Expression = callExpression;
                        switch (callExpression.Method.ToStringExtensionMethodType()) {
                            case ExpressionType.GreaterThan:
                                this.FilterType = Orm.FilterType.GreaterThan;
                                break;
                            case ExpressionType.GreaterThanOrEqual:
                                this.FilterType = Orm.FilterType.GreaterThanOrEqual;
                                break;
                            case ExpressionType.LessThan:
                                this.FilterType = Orm.FilterType.LessThan;
                                break;
                            case ExpressionType.LessThanOrEqual:
                                this.FilterType = Orm.FilterType.LessThanOrEqual;
                                break;
                            default:
                                throw new NotSupportedException();
                        }
                    } else {
                        if (isNot) {
                            this.Expression = Expression.Not(expression);
                        } else {
                            this.Expression = expression;
                        }
                    }
                    break;
                default:
                    if (isNot) {
                        this.Expression = Expression.Not(expression);
                    } else {
                        this.Expression = expression;
                    }
                    break;
            }
        }

        public Expression Expression { get; private set; }
        public FilterComparison ComparisonType { get; private set; }
        public FilterType FilterType { get; private set; }
        public FieldInfo Left { get; private set; }
        public FieldInfo Right { get; private set; }
        public object Value { get; private set; }

        public IEnumerable<FieldInfo> Fields {
            get {
                switch (this.ComparisonType) {
                    case FilterComparison.ToConstant:
                        yield return this.Left;
                        break;
                    case FilterComparison.ToField:
                        yield return this.Left;
                        yield return this.Right;
                        break;
                }
            }
        }
    }
}
