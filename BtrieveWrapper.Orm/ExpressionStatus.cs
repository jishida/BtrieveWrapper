using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace BtrieveWrapper.Orm
{
    class ExpressionStatus :IDisposable
    {
        static HashSet<int> _hashSet = new HashSet<int>();
        static int _lastId = 0;

        public ExpressionStatus(Expression expression, ParameterExpression argument, bool isNot) {
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
                    if (binaryExpression.Left.GetField(argument) == null && binaryExpression.Right.GetField(argument) != null) {
                        binaryExpression = binaryExpression.FlipBinary();
                    }
                    this.Left = binaryExpression.Left.GetField(argument);
                    this.Right = binaryExpression.Right.GetField(argument);
                    if (this.Left != null) {
                        
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
                default:
                    if (isNot) {
                        this.Expression = Expression.Not(expression);
                    } else {
                        this.Expression = expression;
                    }
                    break;
            }

            lock (_hashSet) {
                unchecked {
                    for (this.Id = _lastId + 1; _hashSet.Contains(this.Id); this.Id++) { }
                    _hashSet.Add(this.Id);
                    _lastId = this.Id;
                }
            }
        }

        public int Id { get; private set; }
        public Expression Expression { get; private set; }
        public FilterComparison ComparisonType { get; private set; }
        public FilterType FilterType { get; private set; }
        public FieldInfo Left { get; private set; }
        public FieldInfo Right { get; private set; }
        public object Value { get; private set; }
        public IEnumerable<FieldInfo> Fields {
            get {
                if (this.Left != null) {
                    yield return this.Left;
                    if (this.Right != null) {
                        yield return this.Right;
                    }
                }
            }
        }


        public override int GetHashCode() {
            return this.Id;
        }

        public override bool Equals(object obj) {
            var instance = obj as ExpressionStatus;
            if (instance == null) {
                return false;
            }
            return this.Id == instance.Id;
        }

        public void Dispose() {
            lock (_hashSet) {
                _hashSet.Remove(this.Id);
            }
        }
    }
}
