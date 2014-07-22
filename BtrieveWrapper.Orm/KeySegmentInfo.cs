using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BtrieveWrapper.Orm
{
    public class KeySegmentInfo
    {
        internal KeySegmentInfo(KeySegmentAttribute attribute, FieldInfo field) {
            this.KeyNumber = attribute.KeyNumber;
            this.Index = attribute.Index;
            this.NullValue = attribute.NullValue;
            this.IsDescending = attribute.IsDescending;
            this.IsIgnoreCase = attribute.IsIgnoreCase;

            this.Length = field.Length;
            this.Field = field;

            this.Position = 0;
            this.Key = null;
            this.IsSegmentKey = true;
        }

        public sbyte KeyNumber { get; private set; }
        public ushort Index { get; private set; }
        public byte NullValue { get; private set; }
        public bool IsDescending { get; private set; }
        public bool IsIgnoreCase { get; private set; }

        public ushort Position { get; internal set; }
        public ushort Length { get; private set; }
        public FieldInfo Field { get; private set; }
        public KeyInfo Key { get; internal set; }
        internal bool IsSegmentKey { get;  set; }

        internal CreateKeySpec GetCreateKeySpec() {
            var keyFlag = KeyFlag.None;
            byte extendedKeyType = 0;
            switch (this.Key.DuplicateKeyOption) {
                case DuplicateKeyOption.LinkDuplicate:
                    keyFlag |= KeyFlag.Dup;
                    break;
                case DuplicateKeyOption.RepeatDuplicate:
                    keyFlag |= KeyFlag.RepeatDupsKey;
                    break;
                case DuplicateKeyOption.Unique:
                    break;
                default:
                    throw new InvalidDefinitionException();
            }
            if (this.Key.IsModifiable) {
                keyFlag |= KeyFlag.Mod;
            }
            switch (this.Field.KeyType) {
                case KeyType.LegacyBinary:
                    keyFlag |= KeyFlag.Bin;
                    break;
                case KeyType.LegacyString:
                    break;
                default:
                    keyFlag |= KeyFlag.ExttypeKey;
                    extendedKeyType = (byte)this.Field.KeyType;
                    break;
            }
            switch (this.Key.NullKeyOption) {
                case NullKeyOption.IgnoreKeyMatch:
                    keyFlag |= KeyFlag.Nul;
                    break;
                case NullKeyOption.IgnoreKeySegmentMatch:
                    keyFlag |= KeyFlag.ManualKey;
                    break;
            }
            if (this.IsSegmentKey) {
                keyFlag |= KeyFlag.Seg;
            }
            if (this.IsDescending) {
                keyFlag |= KeyFlag.DescKey;
            }
            return new CreateKeySpec(
                this.Field.Position,
                this.Field.Length,
                keyFlag,
                extendedKeyType,
                this.NullValue,
                this.Key.KeyNumber,
                0);
        }

    }
}
