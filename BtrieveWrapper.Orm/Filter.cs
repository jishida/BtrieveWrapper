using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace BtrieveWrapper.Orm
{
    public class Filter
    {
        ushort _length = 0;

        public Filter(FieldInfo field, FilterType type, object value, AcsIdentifier acsIdentifier = null, bool useDefaultAcs = false, bool isIgnoreCase = false) {
            if (field == null) {
                throw new ArgumentNullException();
            }
            if (!field.IsFilterable || (value is FieldInfo && (!((FieldInfo)value).IsFilterable || field.KeyType != ((FieldInfo)value).KeyType))) {
                throw new ArgumentException();
            }
            this.Field = field;
            this.Type = type;
            this.Value = value;
            this.AcsIdentifier = acsIdentifier;
            this.UseDefaultAcs = useDefaultAcs;
            this.IsIgnoreCase = isIgnoreCase;
            if (this.ComparedField != null && this.Field.Record != this.ComparedField.Record) {
                throw new ArgumentException();
            }
        }

        public FieldInfo Field { get; private set; }
        public FilterType Type { get; private set; }
        public object Value { get; private set; }
        public AcsIdentifier AcsIdentifier { get; private set; }
        public bool UseDefaultAcs { get; private set; }
        public bool IsIgnoreCase { get; private set; }

        public FieldInfo ComparedField { get { return this.Value as FieldInfo; } }


        byte FilterFlag {
            get {
                var result = (byte)this.Type;
                if (this.AcsIdentifier != null) {
                    result += 8;
                }
                if (this.UseDefaultAcs) {
                    result += 32;
                }
                if (this.ComparedField != null) {
                    result += 64;
                }
                if (this.IsIgnoreCase) {
                    result += 128;
                }
                return result;
            }
        }

        public IEnumerable<FieldInfo> Fields {
            get {
                yield return this.Field;
                if (this.ComparedField != null) {
                    yield return this.ComparedField;
                }
            }
        }

        internal Expression ToExpression( ParameterExpression argument) {
            ExpressionType nodeType;
            switch(this.Type){
                case FilterType.Equal:
                    nodeType= ExpressionType.Equal;
                    break;
                case FilterType.NotEqual:
                    nodeType= ExpressionType.NotEqual;
                    break;
                case FilterType.GreaterThan:
                    nodeType= ExpressionType.GreaterThan;
                    break;
                case FilterType.GreaterThanOrEqual:
                    nodeType= ExpressionType.GreaterThanOrEqual;
                    break;
                case FilterType.LessThan:
                    nodeType= ExpressionType.LessThan;
                    break;
                case FilterType.LessThanOrEqual:
                    nodeType= ExpressionType.LessThanOrEqual;
                    break;
                default:
                    throw new NotSupportedException();
            }
            Expression left=Expression.MakeMemberAccess(argument,this.Field.Property);
            Expression right;
            if (this.Value is FieldInfo) {
                right = Expression.MakeMemberAccess(argument, ((FieldInfo)this.Value).Property);
            } else {
                right = Expression.Constant(this.Value);
            }
            return Expression.MakeBinary(nodeType, left, right);
        }

        internal ushort SetDataBuffer(byte[] dataBuffer, ushort position, byte logicalFlag) {
            var result = (ushort)(position + this.Length);
            dataBuffer[position] = (byte)this.Field.KeyType;
            position++;
            Array.Copy(BitConverter.GetBytes(this.Field.Length), 0, dataBuffer, position, 2);
            position += 2;
            Array.Copy(BitConverter.GetBytes(this.Field.Position), 0, dataBuffer, position, 2);
            position += 2;
            dataBuffer[position] = this.FilterFlag;
            position++;
            dataBuffer[position] = logicalFlag;
            position++;
            if (this.ComparedField == null) {
                this.Field.Converter.ConvertBack(this.Value, dataBuffer, position, this.Field.Length, this.Field.Parameter);
                position += this.Field.Length;
            } else {
                Array.Copy(BitConverter.GetBytes(this.ComparedField.Length), 0, dataBuffer, position, 2);
                position += 2;
            }
            if (this.AcsIdentifier != null) {
                this.AcsIdentifier.SetDataBuffer(dataBuffer, position);
            }
            return result;
        }

        internal ushort Length {
            get {
                if (_length == 0) {
                    _length = 7;
                    if (this.ComparedField == null) {
                        _length += this.Field.Length;
                    } else {
                        _length += 2;
                    }
                    if (this.AcsIdentifier != null) {
                        switch (this.AcsIdentifier.Type) {
                            case AcsType.UserDefined:
                                _length += 9;
                                break;
                            case AcsType.LocaleSpecific:
                                _length += 5;
                                break;
                            case AcsType.InternationalSortingRule:
                                _length += 17;
                                break;
                            default:
                                throw new NotSupportedException();
                        }
                    }
                }
                return _length;
            }
        }
    }
}
