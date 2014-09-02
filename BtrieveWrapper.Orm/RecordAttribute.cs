using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using BtrieveWrapper.Orm.Converters;

namespace BtrieveWrapper.Orm
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class RecordAttribute : Attribute
    {
        public RecordAttribute(ushort fixedLength, string host, string dbName, string table)
            : this(fixedLength) {
            this.UriHost = host;
            this.UriDbName = dbName;
            this.UriTable = table;
        }

        public RecordAttribute(ushort fixedLength, string relativePath, string relativeDirectory)
            : this(fixedLength) {
            this.PathType = PathType.Relative;
            this.RelativePath = relativePath;
            this.RelativeDirectory = relativeDirectory;
        }

        public RecordAttribute(ushort fixedLength, string absolutePath)
            : this(fixedLength) {
            this.PathType = PathType.Absolute;
            this.AbsolutePath = absolutePath;
        }

        public RecordAttribute(ushort fixedLength) {
            this.FixedLength = fixedLength;
            this.PageSize = 4096;
            this.DuplicatedPointerCount = 0;
            this.Allocation = 0;
            this.VariableOption = RecordVariableOption.NotVariable;
            this.UsesIndexBalancing = false;
            this.IsCompressed = false;
            this.FreeSpaceThreshold = FreeSpaceThreshold.FivePercent;
            this.SystemDataOption = SystemDataOption.FollowEngine;

            this.PrimaryKeyNumber = 0;
            this.DefaultByte = default(byte);
            this.DllPath = null;
            this.DependencyPaths = null;

            this.PathType = PathType.Uri;

            this.OwnerNameOption = BtrieveWrapper.OwnerNameOption.NoEncryption;
            this.OpenMode = OpenMode.Normal;

            this.VariableFieldCapacity = 0;
            this.RejectCount = 0;
        }

        public ushort FixedLength { get; private set; }
        public ushort PageSize { get; set; }
        public byte DuplicatedPointerCount { get; set; }
        public ushort Allocation { get; set; }
        public RecordVariableOption VariableOption { get; set; }
        public bool UsesIndexBalancing { get; set; }
        public bool IsCompressed { get; set; }
        public FreeSpaceThreshold FreeSpaceThreshold { get; set; }
        public SystemDataOption SystemDataOption { get; set; }

        public sbyte PrimaryKeyNumber { get; set; }
        public byte DefaultByte { get; set; }
        public string DllPath { get; set; }
        public IEnumerable<string> DependencyPaths { get; set; }

        public PathType PathType { get; set; }
        public string UriHost { get; set; }
        public string UriUser { get; set; }
        public string UriDbName { get; set; }
        public string UriTable { get; set; }
        public string UriDbFile { get; set; }
        public string UriFile { get; set; }
        public string UriPassword { get; set; }
        public bool? UriPrompt { get; set; }
        public string AbsolutePath { get; set; }
        public string RelativeDirectory { get; set; }
        public string RelativePath { get; set; }

        public string OwnerName { get; set; }
        public OwnerNameOption OwnerNameOption { get; set; }
        public OpenMode OpenMode { get; set; }

        public ushort VariableFieldCapacity { get; set; }
        public ushort RejectCount { get; set; }
    }
}
