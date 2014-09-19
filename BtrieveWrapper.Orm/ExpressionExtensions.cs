using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace BtrieveWrapper.Orm
{
    static class ExpressionExtensions
    {

        public static FieldInfo GetField(this Expression expression, ParameterExpression argument) {
            var memberExpression = expression as MemberExpression;
            if (memberExpression != null &&
                memberExpression.Expression.Equals(argument)) {
                    return Resource.GetFieldInfo(memberExpression.Member);
            } else {
                return null;
            }
        }

        public static bool HasArgument(this Expression expression, ParameterExpression argument) {
            if (expression is ParameterExpression) {
                return expression.Equals(argument);
            } else if (expression is MemberExpression) {
                var memberExpression = (MemberExpression)expression;
                if (memberExpression.Expression == null) {
                    return false;
                } else {
                    return memberExpression.Expression.HasArgument(argument);
                }
            } else if (expression is MethodCallExpression) {
                var methodCallExpression = (MethodCallExpression)expression;
                return
                    (methodCallExpression.Object != null && methodCallExpression.Object.HasArgument(argument)) ||
                    methodCallExpression.Arguments.Any(a => a.HasArgument(argument));
            } else if (expression is NewExpression) {
                return ((NewExpression)expression).Arguments.Any(a => a.HasArgument(argument));
            } else if (expression is BinaryExpression) {
                var binaryExpression = (BinaryExpression)expression;
                return binaryExpression.Left.HasArgument(argument) || binaryExpression.Right.HasArgument(argument);
            } else if (expression is ConditionalExpression) {
                var conditionalExpression = (ConditionalExpression)expression;
                return
                    conditionalExpression.Test.HasArgument(argument) ||
                    conditionalExpression.IfTrue.HasArgument(argument) ||
                    conditionalExpression.IfFalse.HasArgument(argument);
#if NET_4_0
            } else if (expression is ConstantExpression || expression is DefaultExpression) {
                return false;
            } else if (expression is IndexExpression) {
                var indexExpression = (IndexExpression)expression;
                return 
                    (indexExpression.Object != null && indexExpression.Object.HasArgument(argument)) || 
                    indexExpression.Arguments.Any(a => a.HasArgument(argument));
#else
            } else if (expression is ConstantExpression) {
                return false;
#endif
            } else if (expression is NewArrayExpression) {
                var newArrayExpression = (NewArrayExpression)expression;
                return newArrayExpression.Expressions.Any(a => a.HasArgument(argument));
            } else if (expression is TypeBinaryExpression) {
                var typeBinaryExpression = (TypeBinaryExpression)expression;
                return typeBinaryExpression.Expression.HasArgument(argument);
            } else if (expression is UnaryExpression) {
                var unaryExpression = (UnaryExpression)expression;
                return unaryExpression.Operand.HasArgument(argument);
            } else {
                throw new NotSupportedException();
            }
        }

        public static object ToValue(this Expression expression) {
            return Expression.Lambda<Func<object>>(Expression.Convert(expression, typeof(object))).Compile().Invoke();
        }

        public static Expression[][] ToFilterExpressionMap(this Expression expression) {
            var isNot = false;
            for (; ; ) {
                switch (expression.NodeType) {
                    case ExpressionType.Not:
                        isNot = !isNot;
                        expression = ((UnaryExpression)expression).Operand;
                        break;
                    case ExpressionType.AndAlso:
                    case ExpressionType.OrElse:
                    case ExpressionType.And:
                    case ExpressionType.Or:
                        var binaryExpression = ((BinaryExpression)expression);
                        if (isNot) {
                            if (binaryExpression.NodeType == ExpressionType.AndAlso ||
                                binaryExpression.NodeType == ExpressionType.And) {
                                binaryExpression = Expression.OrElse(Expression.Not(binaryExpression.Left), Expression.Not(binaryExpression.Right));
                            } else {
                                binaryExpression = Expression.AndAlso(Expression.Not(binaryExpression.Left), Expression.Not(binaryExpression.Right));
                            }
                        }
                        var left = binaryExpression.Left.ToFilterExpressionMap();
                        var right = binaryExpression.Right.ToFilterExpressionMap();
                        if (binaryExpression.NodeType == ExpressionType.AndAlso ||
                            binaryExpression.NodeType == ExpressionType.And) {
                            var result = new Expression[left.Length + right.Length][];
                            for (var i = 0; i < left.Length; i++) {
                                result[i] = new Expression[left[i].Length];
                                for (var j = 0; j < left[i].Length; j++) {
                                    result[i][j] = left[i][j];
                                }
                            }
                            for (var i = 0; i < right.Length; i++) {
                                result[left.Length + i] = new Expression[right[i].Length];
                                for (var j = 0; j < right[i].Length; j++) {
                                    result[left.Length + i][j] = right[i][j];
                                }
                            }
                            return result;
                        } else {
                            if (left.Length == 1 && right.Length == 1) {
                                var result = new Expression[1][];
                                result[0] = new Expression[left[0].Length + right[0].Length];
                                for (var i = 0; i < left[0].Length; i++) {
                                    result[0][i] = left[0][i];
                                }
                                for (var i = 0; i < right[0].Length; i++) {
                                    result[0][left[0].Length + i] = right[0][i];
                                }
                                return result;
                            } else {
                                return new Expression[][] { new[] { Expression.OrElse(left.ToAndExpression(), right.ToAndExpression()) } };
                            }
                        }
                    case ExpressionType.GreaterThan:
                    case ExpressionType.GreaterThanOrEqual:
                    case ExpressionType.LessThan:
                    case ExpressionType.LessThanOrEqual:
                    case ExpressionType.Equal:
                    case ExpressionType.NotEqual:
                    case ExpressionType.MemberAccess:
                    case ExpressionType.Call:
                    case ExpressionType.Constant:
                    case ExpressionType.Conditional:
                    case ExpressionType.Coalesce:
                    case ExpressionType.TypeIs:
                        return new Expression[][] { new[] { isNot ? Expression.Not(expression) : expression } };
                    default:
                        throw new NotSupportedException();
                }
            }
        }

        public static Expression ToAndExpression(this IEnumerable<IEnumerable<Expression>> andExpressions) {
            Expression result = null;
            foreach (var orExpressions in andExpressions) {
                var orExpression = orExpressions.ToOrExpression();
                if (orExpression != null) {
                    result = result == null
                        ? orExpression
                        : Expression.AndAlso(result, orExpression);
                }
            }
            return result;
        }

        public static Expression ToOrExpression(this IEnumerable<Expression> orExpressions) {
            Expression result = null;
            foreach (var expression in orExpressions) {
                result = result == null
                    ? expression
                    : Expression.OrElse(result, expression);
            }
            return result;
        }

        public static BinaryExpression ToReversedExpression(this BinaryExpression binaryExpression) {
            return Expression.MakeBinary(
                GetReversedType(binaryExpression.NodeType),
                binaryExpression.Left,
                binaryExpression.Right);
        }

        public static MethodCallExpression ToReversedExpression(this MethodCallExpression callExpression) {
            return Expression.Call(
                callExpression.Method.ToReversedStringExtensionMethod(),
                callExpression.Arguments[0],
                callExpression.Arguments[1]);
        }

        static ExpressionType GetReversedType(ExpressionType nodeType) {
            switch (nodeType) {
                case ExpressionType.Equal:
                    return ExpressionType.NotEqual;
                case ExpressionType.NotEqual:
                    return ExpressionType.Equal;
                case ExpressionType.GreaterThan:
                    return ExpressionType.LessThanOrEqual;
                case ExpressionType.GreaterThanOrEqual:
                    return ExpressionType.LessThan;
                case ExpressionType.LessThan:
                    return ExpressionType.GreaterThanOrEqual;
                case ExpressionType.LessThanOrEqual:
                    return ExpressionType.GreaterThan;
                default:
                    throw new ArgumentException();
            }
        }


        public static BinaryExpression FlipBinary(this BinaryExpression binaryExpression) {
            return Expression.MakeBinary(GetFlippedType(binaryExpression.NodeType), binaryExpression.Right, binaryExpression.Left);
        }

        public static MethodCallExpression FlipStringBinary(this MethodCallExpression callExpression) {
            return Expression.Call(callExpression.Method.ToFlippedStringExtensionMethod(),
                callExpression.Arguments[1], callExpression.Arguments[0]);
        }

        static ExpressionType GetFlippedType(ExpressionType nodeType) {
            switch (nodeType) {
                case ExpressionType.Equal:
                case ExpressionType.NotEqual:
                    return nodeType;
                case ExpressionType.GreaterThan:
                    return ExpressionType.LessThan;
                case ExpressionType.GreaterThanOrEqual:
                    return ExpressionType.LessThanOrEqual;
                case ExpressionType.LessThan:
                    return ExpressionType.GreaterThan;
                case ExpressionType.LessThanOrEqual:
                    return ExpressionType.GreaterThanOrEqual;
                default:
                    throw new ArgumentException();
            }
        }

        static BinaryExpression GetFixedEqualExpression(Expression expression1, Expression expression2, ParameterExpression argument, bool isEqual) {
            try {
                if (expression1.GetField(argument)!=null) {
                    var value = expression2.ToValue();
                    if (!isEqual && value.GetType() == typeof(bool)) {
                        isEqual = true;
                        value = !(bool)value;
                    }
                    if (isEqual) {
                        return Expression.Equal(
                            expression1,
                            Expression.Constant(value));
                    } else {
                        return Expression.NotEqual(
                            expression1,
                            Expression.Constant(value));
                    }
                }
            } catch { }
            return null;
        }

        public static BinaryExpression ToFixedRelationalExpression(this BinaryExpression expression, ParameterExpression argument, bool isNot) {
            return GetFixedRelationalExpression(
                    expression.Left,
                    expression.Right,
                    argument,
                    isNot
                        ? GetReversedType(expression.NodeType)
                        : expression.NodeType)
                ?? GetFixedRelationalExpression(
                    expression.Right,
                    expression.Left,
                    argument,
                    isNot
                        ? expression.NodeType
                        : GetReversedType(expression.NodeType));
        }

        static BinaryExpression GetFixedRelationalExpression(Expression expression1, Expression expression2, ParameterExpression argument, ExpressionType nodeType) {
            try {
                if (expression1.GetField(argument) != null) {
                    switch (nodeType) {
                        case ExpressionType.GreaterThan:
                            return Expression.GreaterThan(expression1, expression2);
                        case ExpressionType.GreaterThanOrEqual:
                            return Expression.GreaterThanOrEqual(expression1, expression2);
                        case ExpressionType.LessThan:
                            return Expression.LessThan(expression1, expression2);
                        case ExpressionType.LessThanOrEqual:
                            return Expression.LessThanOrEqual(expression1, expression2);
                    }
                }
            } catch { }
            return null;
        }


        public static FieldInfo GetFieldInfo(this BinaryExpression fixedExpression, ParameterExpression argument) {
            FieldInfo result = null;
            var memberAccessExpression = fixedExpression.Left as MemberExpression;
            if (memberAccessExpression != null &&
                memberAccessExpression.Expression.Equals(argument)) {
                result = Resource.GetFieldInfo(memberAccessExpression.Member);
            }
            return result;
        }


    }
}
