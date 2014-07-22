using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace BtrieveWrapper.Orm
{
    public class FieldInfo
    {
        FieldConverterAttribute _converterInfo;

        internal FieldInfo(FieldAttribute attribute, RecordInfo record, IEnumerable<KeySegmentAttribute> keySegmentAttributes, PropertyInfo property) {
            this.Position = attribute.Position;
            this.Length = attribute.Length;
            this.ConverterType = attribute.ConverterType;
            this.Parameter = attribute.Parameter;
            this.NullType = attribute.NullType;
            this.KeyType = attribute.KeyType;
            this.Property = property;

            var keySegments = new List<KeySegmentInfo>(keySegmentAttributes.Count());
            foreach (var keySegmentAttribute in keySegmentAttributes) {
                keySegments.Add(new KeySegmentInfo(keySegmentAttribute, this));
            }
            this.KeySegments = keySegments.ToArray();

            this.Record = record;
            this.IsPrimaryKeySegment = this.KeySegments.Any(a => a.KeyNumber == record.PrimaryKeyNumber);
            this.IsModifiable = true;

            this.Converter = Resource.GetFieldConverter(this.ConverterType);
            _converterInfo = Resource.GetFieldConverterAttribute(this.ConverterType);
            if (this.NullType == Orm.NullType.NullFlag) {
                if (this.ConverterType != typeof(Converters.NullFlagConverter) ||
                    this.KeyType != BtrieveWrapper.KeyType.LegacyString ||
                    !this.KeySegments.All(s => s.IsDescending)) {
                    throw new InvalidDefinitionException();
                }
            }
            if (!_converterInfo.ValidateLength(this.Length)) {
                throw new InvalidDefinitionException();
            }
        }

        public ushort Position { get; private set; }
        public ushort Length { get; private set; }
        public KeyType KeyType { get; private set; }
        public Type ConverterType { get; private set; }
        public object Parameter { get; private set; }
        public NullType NullType { get; private set; }

        public bool IsPrimaryKeySegment { get; private set; }
        public IEnumerable<KeySegmentInfo> KeySegments { get; private set; }
        public RecordInfo Record { get; private set; }
        public bool IsModifiable { get; internal set; }
        public string Name { get; internal set; }
        public string FullName { get; internal set; }
        public bool IsFilterable { get { return _converterInfo.IsFilterable; } }
        public FieldInfo NullFlagField { get; internal set; }
        public IFieldConverter Converter { get; private set; }
        public PropertyInfo Property { get; private set; }

        public object Convert(byte[] source) {
            return this.Convert(source, this.Position, this.Length, this.Parameter);
        }

        public object Convert(byte[] source, ushort position, ushort length, object parameter) {
            switch (this.NullType) {
                case Orm.NullType.None:
                case Orm.NullType.NullFlag:
                    return this.Converter.Convert(source, position, length, parameter);
                case Orm.NullType.Nullable:
                    if (source[position - 1] == 0x00) {
                        return this.Converter.Convert(source, position, length, parameter);
                    } else {
                        return null;
                    }
                default:
                    throw new NotSupportedException();
            }
        }

        public void ConvertBack(object source, byte[] destination) {
            this.ConvertBack(source, destination, this.Position, this.Length, this.Parameter);
        }

        public void ConvertBack(object source, byte[] destination, ushort position, ushort length, object parameter) {
            switch (this.NullType) {
                case Orm.NullType.None:
                    this.Converter.ConvertBack(source, destination, position, length, parameter);
                    break;
                case Orm.NullType.Nullable:
                    if (source == null) {
                        destination[position - 1] = 0x01;
                        for (var i = 0; i < length; i++) {
                            destination[position + i] = 0x00;
                        }
                    } else {
                        destination[position - 1] = 0x00;
                        this.Converter.ConvertBack(source, destination, position, length, parameter);
                    }
                    break;
                case Orm.NullType.NullFlag:
                default:
                    throw new NotSupportedException();
            }
        }

        public void SetMaxValue(byte[] buffer) {
            this.SetMaxValue(buffer, this.Position, this.Length, this.Parameter);
        }

        public void SetMaxValue(byte[] buffer, ushort position, ushort length, object parameter) {
            switch (this.NullType) {
                case Orm.NullType.None:
                    this.Converter.SetMaxValue(buffer, position, length, parameter);
                    break;
                case Orm.NullType.Nullable:
                    buffer[position - 1] = 0x00;
                    this.Converter.SetMaxValue(buffer, position, length, parameter);
                    break;
                case Orm.NullType.NullFlag:
                    break;
                default:
                    throw new NotSupportedException();
            }
        }

        public void SetMinValue(byte[] buffer) {
            this.SetMinValue(buffer, this.Position, this.Length, this.Parameter);
        }

        public void SetMinValue(byte[] buffer, ushort position, ushort length, object parameter) {
            switch (this.NullType) {
                case Orm.NullType.None:
                    this.Converter.SetMinValue(buffer, position, length, parameter);
                    break;
                case Orm.NullType.Nullable:
                    buffer[position - 1] = 0x00;
                    this.Converter.SetMinValue(buffer, position, length, parameter);
                    break;
                case Orm.NullType.NullFlag:
                    break;
                default:
                    throw new NotSupportedException();
            }
        }

        public void SetDefaultValue(byte[] buffer) {
            this.SetDefaultValue(buffer, this.Position, this.Length, this.Parameter);
        }

        public void SetDefaultValue(byte[] buffer, ushort position, ushort length, object parameter) {
            switch (this.NullType) {
                case Orm.NullType.None:
                    this.Converter.SetDefaultValue(buffer, position, length, parameter);
                    break;
                case Orm.NullType.Nullable:
                    buffer[position - 1] = 0x01;
                    break;
                case Orm.NullType.NullFlag:
                    break;
                default:
                    throw new NotSupportedException();
            }
        }
    }
}
