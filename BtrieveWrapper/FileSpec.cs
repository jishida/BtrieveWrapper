using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BtrieveWrapper
{
    public abstract class FileSpec
    {
        protected FileSpec(
            ushort recordLength,
            ushort pageSize,
            byte keyCount,
            FileFlag flag) {

            this.RecordLength = recordLength;
            this.PageSize = pageSize;
            this.KeyCount = keyCount;
            this.Flag = flag;
        }

        public ushort RecordLength { get; protected set; }
        public ushort PageSize { get; protected set; }
        public byte KeyCount { get; internal set; }
        public FileFlag Flag { get; protected set; }

        public RecordVariableOption VariableOption {
            get {
                if ((this.Flag & FileFlag.VarRecs) == FileFlag.VarRecs) {
                    var result = RecordVariableOption.Variable;
                    if ((this.Flag & FileFlag.BlankTrunc) == FileFlag.BlankTrunc) {
                        result |= RecordVariableOption.BlankTruncation;
                    }
                    if ((this.Flag & FileFlag.VatsSupport) == FileFlag.VatsSupport) {
                        result |= RecordVariableOption.VariabletailAllocationTable;
                    }
                    return result;
                } else {
                    return RecordVariableOption.NotVariable;
                }
            }
        }
        public bool UsesIndexBalancing { get { return (this.Flag & FileFlag.BalancedKeys) == FileFlag.BalancedKeys; } }
        public bool UsesPagePreAllocation { get { return (this.Flag & FileFlag.PreAlloc) == FileFlag.PreAlloc; } }
        public bool IsCompressed { get { return (this.Flag & FileFlag.DataComp) == FileFlag.DataComp; } }
        public bool IsKeyOnly { get { return (this.Flag & FileFlag.KeyOnly) == FileFlag.KeyOnly; } }
        public FreeSpaceThreshold FreeSpaceThreshold {
            get {
                var tenFlag = (this.Flag & FileFlag.Free10) == FileFlag.Free10;
                var twentyFlag = (this.Flag & FileFlag.Free20) == FileFlag.Free20;
                return tenFlag
                    ? twentyFlag
                        ? FreeSpaceThreshold.ThirtyPercent
                        : FreeSpaceThreshold.TenPercent
                    : twentyFlag
                        ? FreeSpaceThreshold.TwentyPercent
                        : FreeSpaceThreshold.FivePercent;
            }
        }
        public bool IsDuplicatedPointerReserved { get { return (this.Flag & FileFlag.DupPtrs) == FileFlag.DupPtrs; } }
        public SystemDataOption SystemDataOption {
            get {
                var flag=this.Flag & FileFlag.NoSystemDataIncluded;
                if (flag == FileFlag.SystemDataIncluded) {
                    return SystemDataOption.Force;
                }
                if (flag == FileFlag.NoSystemDataIncluded) {
                    return SystemDataOption.None;
                }
                return SystemDataOption.FollowEngine;
            }
        }
        public bool IsManualKeyNumber { get { return (this.Flag & FileFlag.SpecifyKeyNums) == FileFlag.SpecifyKeyNums; } }
    }
}
