using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

using BtrieveWrapper.Orm.Converters;

namespace BtrieveWrapper.Orm.Models
{
    public static class Config
    {
        static Config() {
            Config.KeyName = "Key{0}";
            Config.FieldName = "Field{0}";
            Config.RecordName = "Record{0}";
            Config.DefaultModelName = "Model";
            Config.ConverterDictionary = new ConverterSetDictionary();
            Config.ConverterDictionary[KeyType.Autoincrement, 2] = new ConverterSet(typeof(UInt16Converter));
            Config.ConverterDictionary[KeyType.Autoincrement, 4] = new ConverterSet(typeof(UInt32Converter));
            Config.ConverterDictionary[KeyType.Bit, 1, 0x01] = new ConverterSet(typeof(BitBooleanConverter), "BtrieveWrapper.Orm.Converters.Bit.Bit1");
            Config.ConverterDictionary[KeyType.Bit, 1, 0x02] = new ConverterSet(typeof(BitBooleanConverter), "BtrieveWrapper.Orm.Converters.Bit.Bit2");
            Config.ConverterDictionary[KeyType.Bit, 1, 0x04] = new ConverterSet(typeof(BitBooleanConverter), "BtrieveWrapper.Orm.Converters.Bit.Bit3");
            Config.ConverterDictionary[KeyType.Bit, 1, 0x08] = new ConverterSet(typeof(BitBooleanConverter), "BtrieveWrapper.Orm.Converters.Bit.Bit4");
            Config.ConverterDictionary[KeyType.Bit, 1, 0x10] = new ConverterSet(typeof(BitBooleanConverter), "BtrieveWrapper.Orm.Converters.Bit.Bit5");
            Config.ConverterDictionary[KeyType.Bit, 1, 0x20] = new ConverterSet(typeof(BitBooleanConverter), "BtrieveWrapper.Orm.Converters.Bit.Bit6");
            Config.ConverterDictionary[KeyType.Bit, 1, 0x40] = new ConverterSet(typeof(BitBooleanConverter), "BtrieveWrapper.Orm.Converters.Bit.Bit7");
            Config.ConverterDictionary[KeyType.Bit, 1, 0x80] = new ConverterSet(typeof(BitBooleanConverter), "BtrieveWrapper.Orm.Converters.Bit.Bit8");
            Config.ConverterDictionary[KeyType.Currency, 8] = new ConverterSet(typeof(SignedToDecimalConverter), "4");
            Config.ConverterDictionary[KeyType.Currency, 8, 4] = new ConverterSet(typeof(SignedToDecimalConverter), "4");
            Config.ConverterDictionary[KeyType.Date, 4] = new ConverterSet(typeof(DateConverter));
            Config.ConverterDictionary[KeyType.DateTime, 8] = new ConverterSet(typeof(DateTimeConverter));
            Config.ConverterDictionary[KeyType.Float, 4] = new ConverterSet(typeof(FloatConverter));
            Config.ConverterDictionary[KeyType.Float, 8] = new ConverterSet(typeof(DoubleConverter));
            Config.ConverterDictionary[KeyType.Integer, 1] = new ConverterSet(typeof(SByteConverter));
            Config.ConverterDictionary[KeyType.Integer, 2] = new ConverterSet(typeof(Int16Converter));
            Config.ConverterDictionary[KeyType.Integer, 4] = new ConverterSet(typeof(Int32Converter));
            Config.ConverterDictionary[KeyType.Integer, 8] = new ConverterSet(typeof(Int64Converter));
            Config.ConverterDictionary[KeyType.String] = new ConverterSet(typeof(StringConverter), "0x20");
            Config.ConverterDictionary[KeyType.Time, 4] = new ConverterSet(typeof(TimeConverter));
            Config.ConverterDictionary[KeyType.Timestamp, 8] = new ConverterSet(typeof(TimestampConverter));
            Config.ConverterDictionary[KeyType.UnsignedBinary, 1] = new ConverterSet(typeof(ByteConverter));
            Config.ConverterDictionary[KeyType.UnsignedBinary, 2] = new ConverterSet(typeof(UInt16Converter));
            Config.ConverterDictionary[KeyType.UnsignedBinary, 4] = new ConverterSet(typeof(UInt32Converter));
            Config.ConverterDictionary[KeyType.UnsignedBinary, 8] = new ConverterSet(typeof(UInt64Converter));
            Config.DefaultConverterType = typeof(ByteArrayConverter);
            Config.DefaultVariableConverterType = typeof(ByteArrayConverter);
            Config.SetAssemblies(null);

            for (ushort i = 1; i < 15; i++) {
                for (byte j = 0; j < 29; j++) {
                    Config.ConverterDictionary[KeyType.Decimal, i, j] = new ConverterSet(typeof(DecimalConverter), j.ToString());
                }
                Config.ConverterDictionary[KeyType.Decimal, i] = new ConverterSet(typeof(DecimalConverter));
                Config.ConverterDictionary[KeyType.Decimal, i, 2] = new ConverterSet(typeof(DecimalConverter), "2");
            }

            for (ushort i = 1; i < 256; i++) {
                Config.ConverterDictionary[KeyType.ZString, i] = new ConverterSet(typeof(ZStringConverter));
            }
        }

        public static string GetConverterTypeName(KeyType keyType, ushort length, byte dec = 0) {
            Type type;
            if (Config.ConverterDictionary.ContainsKey(keyType, length, dec)) {
                type = Config.ConverterDictionary[keyType, length, dec].ConverterType;
            } else {
                if (Config.ConverterDictionary.ContainsKey(keyType, 0, dec)) {
                    type = Config.ConverterDictionary[keyType, dec].ConverterType;
                } else {
                    type = Config.DefaultConverterType;
                }
            }
            return type.Assembly.GetName().Name + "," + type.FullName;
        }

        public static string GetConverterParameter(KeyType keyType, ushort length, byte dec = 0) {
            if (Config.ConverterDictionary.ContainsKey(keyType, length, dec)) {
                return Config.ConverterDictionary[keyType, length, dec].Parameter;
            } else {
                if (Config.ConverterDictionary.ContainsKey(keyType, 0, dec)) {
                    return Config.ConverterDictionary[keyType, dec].Parameter;
                } else {
                    return null;
                }
            }
        }

        public static void SetAssemblies(IEnumerable<string> paths) {
            var assemblies = new List<Assembly>();
            var self = Assembly.GetEntryAssembly();
            assemblies.Add(self);
            assemblies.AddRange(self.GetReferencedAssemblies().Select(a => Assembly.Load(a)));
            if (paths != null) {
                foreach (var path in paths) {
                    try {
                        assemblies.Add(Assembly.LoadFrom(path));
                    } catch { }
                }
            }
            Config.Assemblies = assemblies.Select(a => a).Distinct();
        }

        public static Type GetType(string typeName) {
            if (typeName == null) {
                throw new ArgumentNullException();
            }
            var args=typeName.Split(',');
            return Config.Assemblies
                .Where(a => a.GetName().Name == args[0].Trim())
                .SelectMany(a => a.GetTypes())
                .Single(t => t.FullName == args[1].Trim());
        }

        public static IEnumerable<Assembly> Assemblies { get; private set; }
        public static string KeyName { get; set; }
        public static string FieldName { get; set; }
        public static string RecordName { get; set; }
        public static string DefaultModelName { get; set; }
        public static ConverterSetDictionary ConverterDictionary { get; private set; }
        public static Type DefaultConverterType { get; set; }
        public static Type DefaultVariableConverterType { get; set; }
    }
}
