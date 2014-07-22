using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BtrieveWrapper.Orm
{
    public class KeyInfo
    {
        internal KeyInfo(KeyAttribute attribute, IEnumerable<KeySegmentInfo> keySegments, RecordInfo record) {
            this.KeyNumber = attribute.KeyNumber;
            this.DuplicateKeyOption = attribute.DuplicateKeyOption;
            this.IsModifiable = attribute.IsModifiable;
            this.NullKeyOption = attribute.NullKeyOption;
            this.Segments = new KeySegmentCollection(keySegments);
            this.Length = (ushort)this.Segments.Sum(s => s.Length);

            ushort position = 0;
            foreach (var keySegment in this.Segments) {
                keySegment.Position = position;
                keySegment.Key = this;
                position += keySegment.Length;
            }
            this.Record = record;
        }

        public sbyte KeyNumber { get; private set; }
        public DuplicateKeyOption DuplicateKeyOption { get; private set; }
        public bool IsModifiable { get; private set; }
        public NullKeyOption NullKeyOption { get; private set; }

        internal ushort Length { get; private set; }
        internal KeySegmentCollection Segments { get; private set; }
        internal RecordInfo Record { get; private set; }
    }
}
