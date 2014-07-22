using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BtrieveWrapper
{
    public abstract class KeySpec
    {
        protected KeySpec(
            ushort position,
            ushort length,
            KeyFlag flag,
            byte extendedKeyType,
            byte nullValue,
            sbyte keyNumber,
            byte acsNumber) {

            if (position == ushort.MaxValue) {
                throw new ArgumentOutOfRangeException();
            }

            this.Position = position;
            this.Length = length;
            this.Flag = flag;
            this.ExtendedKeyType = extendedKeyType;
            this.NullValue = nullValue;
            this.Number = keyNumber;
            this.AcsNumber = acsNumber;
        }

        public ushort Position { get; private set; }
        public ushort Length { get; private set; }
        public KeyFlag Flag { get; private set; }
        public byte ExtendedKeyType { get; private set; }
        public byte NullValue { get; private set; }
        public sbyte Number { get; private set; }
        public byte AcsNumber { get; private set; }

        public DuplicateKeyOption DuplicateKeyOption {
            get {
                if ((this.Flag & KeyFlag.Dup) != KeyFlag.Dup) {
                    return DuplicateKeyOption.Unique;
                }
                if ((this.Flag & KeyFlag.RepeatDupsKey) == KeyFlag.RepeatDupsKey) {
                    return DuplicateKeyOption.RepeatDuplicate;
                } else {
                    return DuplicateKeyOption.LinkDuplicate;
                }
            }
        }
        public bool IsModifiable { get { return (this.Flag & KeyFlag.Mod) == KeyFlag.Mod; } }
        public NullKeyOption NullKeyOption {
            get {
                if ((this.Flag & KeyFlag.Nul) == KeyFlag.Nul) {
                    return NullKeyOption.IgnoreKeyMatch;
                }
                if ((this.Flag & KeyFlag.ManualKey) == KeyFlag.ManualKey) {
                    return NullKeyOption.IgnoreKeySegmentMatch;
                }
                return NullKeyOption.None;
            }
        }
        public AcsOption AcsOption {
            get {
                var flag=this.Flag & KeyFlag.NamedACS;
                if (flag == KeyFlag.NamedACS) {
                    return AcsOption.NamedAcs;
                }
                if (flag == KeyFlag.NumberedACS) {
                    return AcsOption.NumberedAcs;
                }
                if (flag == KeyFlag.Alt) {
                    return AcsOption.DefaultAcs;
                }
                return AcsOption.None;
            }
        }
        public bool IsSegmentKey { get { return (this.Flag & KeyFlag.Seg) == KeyFlag.Seg; } }
        public bool IsDescending { get { return (this.Flag & KeyFlag.DescKey) == KeyFlag.DescKey; } }
        public bool IsIgnoreCase { get { return (this.Flag & KeyFlag.NocaseKey) == KeyFlag.NocaseKey; } }
        public KeyType KeyType {
            get {
                if ((this.Flag & KeyFlag.ExttypeKey) == KeyFlag.ExttypeKey) {
                    return (KeyType)this.ExtendedKeyType;
                } else if ((this.Flag & KeyFlag.Bin) == KeyFlag.Bin) {
                    return KeyType.LegacyBinary;
                } else {
                    return KeyType.LegacyString;
                }
            }
        }
    }
}
