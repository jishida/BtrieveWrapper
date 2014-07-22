using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace BtrieveWrapper.Orm
{
    class ExpressionParser
    {
        ParameterExpression _argument;

        public ExpressionParser(Expression body, ParameterExpression argument, bool isIgnoreCase) {
            _argument = argument;
            this.Expressions=new List<Expression>();
            FilterAnd filterAnd = new FilterAnd();
            var map = body.ToFilterExpressionMap();
            for (var i = 0; i < map.Length; i++) {
                var filters = map[i].Select(f => new ExpressionFilter(f, argument)).ToArray();
                if (filters.Any(f => f.ComparisonType == FilterComparison.NotComparable) ||
                    filters.Any(ef => ef.Fields.Any(f => !f.IsFilterable))) {
                    this.Expressions.Add(map[i].ToOrExpression());
                } else {
                    var isNotComparable = false;
                    var filterOr = new FilterOr();
                    foreach (var filter in filters) {
                        switch (filter.ComparisonType) {
                            case FilterComparison.ToConstant:
                                if (filter.Left.NullType == NullType.Nullable) {
                                    var isNull = filter.Value == null;
                                    switch (filter.FilterType) {
                                        case FilterType.Equal:
                                            if (isNull) {
                                                filterOr.Add(filter.Left.NullFlagField, FilterType.Equal, true);
                                            } else {
                                                if (map[i].Length == 1) {
                                                    filterAnd.Add(new FilterOr().Add(filter.Left.NullFlagField, FilterType.Equal, false));
                                                    filterOr.Add(filter.Left, FilterType.Equal, filter.Value, isIgnoreCase: isIgnoreCase);
                                                } else if (filterOr.FilterAnd == null) {
                                                    filterOr.FilterAnd = new FilterAnd()
                                                        .Add(new FilterOr().Add(filter.Left.NullFlagField, FilterType.Equal, false))
                                                        .Add(new FilterOr().Add(filter.Left, FilterType.Equal, filter.Value, isIgnoreCase: isIgnoreCase));
                                                } else {
                                                    isNotComparable = true;
                                                }
                                            }
                                            break;
                                        case FilterType.NotEqual:
                                            if (isNull) {
                                                filterOr.Add(filter.Left.NullFlagField, FilterType.Equal, false);
                                            } else {
                                                filterOr
                                                    .Add(filter.Left.NullFlagField, FilterType.Equal, false)
                                                    .Add(filter.Left, FilterType.NotEqual, filter.Value, isIgnoreCase: isIgnoreCase);
                                            }
                                            break;
                                        case FilterType.GreaterThan:
                                        case FilterType.GreaterThanOrEqual:
                                        case FilterType.LessThan:
                                        case FilterType.LessThanOrEqual:
                                            if (map[i].Length == 1) {
                                                filterAnd.Add(new FilterOr().Add(filter.Left.NullFlagField, FilterType.Equal, true));
                                                filterOr.Add(filter.Left, filter.FilterType, filter.Value, isIgnoreCase: isIgnoreCase);
                                            } else if (filterOr.FilterAnd == null) {
                                                filterOr.FilterAnd = new FilterAnd()
                                                    .Add(new FilterOr().Add(filter.Left.NullFlagField, FilterType.Equal, true))
                                                    .Add(new FilterOr().Add(filter.Left, filter.FilterType, filter.Value, isIgnoreCase: isIgnoreCase));
                                            } else {
                                                isNotComparable = true;
                                            }
                                            break;
                                        default:
                                            throw new NotSupportedException();
                                    }
                                } else {
                                    filterOr.Add(filter.Left, filter.FilterType, filter.Value, isIgnoreCase: isIgnoreCase);
                                }
                                break;
                            case FilterComparison.ToField:
                                var leftNullable = filter.Left.NullType == NullType.Nullable;
                                var rightNullable = filter.Right.NullType == NullType.Nullable;
                                if (leftNullable) {
                                    switch (filter.FilterType) {
                                        case FilterType.Equal:
                                            if (map[i].Length == 1) {
                                                filterAnd.Add(new FilterOr().Add(filter.Left.NullFlagField, FilterType.Equal,
                                                    rightNullable ? (object)filter.Right.NullFlagField : (object)true));
                                                filterOr.Add(filter.Left, FilterType.Equal, filter.Right, isIgnoreCase: isIgnoreCase);
                                            } else if (filterOr.FilterAnd == null) {
                                                filterOr.FilterAnd = new FilterAnd()
                                                    .Add(new FilterOr().Add(filter.Left.NullFlagField, FilterType.Equal,
                                                        rightNullable ? (object)filter.Right.NullFlagField : (object)true))
                                                    .Add(new FilterOr().Add(filter.Left, FilterType.Equal, filter.Right, isIgnoreCase: isIgnoreCase));
                                            } else {
                                                isNotComparable = true;
                                            }
                                            break;
                                        case FilterType.NotEqual:
                                            if (rightNullable) {
                                                filterOr
                                                    .Add(filter.Left.NullFlagField, FilterType.NotEqual, filter.Right.NullFlagField)
                                                    .Add(filter.Left, FilterType.NotEqual, filter.Right, isIgnoreCase: isIgnoreCase);
                                            } else {
                                                filterOr
                                                    .Add(filter.Left.NullFlagField, FilterType.Equal, false)
                                                    .Add(filter.Left, FilterType.NotEqual, filter.Right, isIgnoreCase: isIgnoreCase);
                                            }
                                            break;
                                        case FilterType.GreaterThan:
                                        case FilterType.GreaterThanOrEqual:
                                        case FilterType.LessThan:
                                        case FilterType.LessThanOrEqual:
                                            if (map[i].Length == 1) {
                                                filterAnd.Add(new FilterOr().Add(filter.Left.NullFlagField, FilterType.Equal, true));
                                                if (rightNullable) {
                                                    filterAnd.Add(new FilterOr().Add(filter.Right.NullFlagField, FilterType.Equal, true));
                                                }
                                                filterOr.Add(filter.Left, filter.FilterType, filter.Right, isIgnoreCase: isIgnoreCase);
                                            } else if (filterOr.FilterAnd == null) {
                                                filterOr.FilterAnd = new FilterAnd()
                                                    .Add(new FilterOr().Add(filter.Left.NullFlagField, FilterType.Equal, true));
                                                if (rightNullable) {
                                                    filterOr.FilterAnd
                                                        .Add(new FilterOr().Add(filter.Right.NullFlagField, FilterType.Equal, true));
                                                }
                                                filterOr.FilterAnd
                                                    .Add(new FilterOr().Add(filter.Left, filter.FilterType, filter.Value, isIgnoreCase: isIgnoreCase));
                                            } else {
                                                isNotComparable = true;
                                            }
                                            break;
                                        default:
                                            throw new NotSupportedException();
                                    }
                                } else {
                                    filterOr.Add(filter.Left, filter.FilterType, filter.Right, isIgnoreCase: isIgnoreCase);
                                }
                                break;
                            default:
                                throw new NotSupportedException();
                        }
                        if (isNotComparable) {
                            break;
                        }
                    }
                    if (isNotComparable) {
                        this.Expressions.Add(map[i].ToOrExpression());
                    } else {
                        if (filterOr.Count != 0 || filterOr.FilterAnd != null) {
                            filterAnd.Add(filterOr);
                        }
                    }
                }

            }
            this.Filter = filterAnd;
        }

        public FilterAnd Filter { get; private set; }
        public List<Expression> Expressions { get; private set; }

        public Func<TRecord, bool> GetAdditionalFilter<TRecord>() where TRecord : Record<TRecord> {
            Expression expression = null;
            foreach (var expr in this.Expressions) {
                if (expression == null) {
                    expression = expr;
                } else {
                    expression = Expression.AndAlso(expression, expr);
                }
            }
            return expression == null ? null
                : Expression.Lambda<Func<TRecord, bool>>(expression, _argument).Compile();
        }
    }
}
