using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BtrieveWrapper.Orm
{
    public class ChunkStream<TRecord> : Stream where TRecord : Record<TRecord>
    {
        RecordOperator<TRecord> _recordOperator;
        FieldInfo _field;
        VariableRange _range;

        public ChunkStream(RecordOperator<TRecord> recordOperator, FieldInfo field, TRecord record) {
            if (recordOperator == null || field == null || record == null) {
                throw new ArgumentNullException();
            }
            if (recordOperator.RecordInfo != field.Record ||
                field.ConverterType != typeof(Converters.VariableRangeConverter) ||
                record.GetType() != _recordOperator.RecordInfo.Type ||
                (field.NullType != NullType.Nullable || (field.NullType == NullType.Nullable && !(bool)field.NullFlagField.Convert(record.DataBuffer)))
                ) {
                throw new ArgumentException();
            }
            _recordOperator = recordOperator;
            _field = field;
            var range = field.Convert(record.DataBuffer) as VariableRange?;
            if (range.HasValue) {
                _range = range.Value;
            } else {
                throw new ArgumentException();
            }
            if (_range.Length < 0) {
                throw new ArgumentException();
            }

        }

        public override bool CanRead {
            get { return true; }
        }
        public override bool CanWrite {
            get { return false; }
        }
        public override bool CanSeek {
            get { return true; }
        }
        public override long Position { get; set; }

        public override int Read(byte[] buffer, int offset, int count) {
            throw new NotImplementedException();
        }

        public override int ReadByte() {
            throw new NotImplementedException();
        }
        

        public override long Seek(long offset, SeekOrigin origin) {
            throw new NotImplementedException();
        }

        public override void SetLength(long value) {
            throw new NotImplementedException();
        }

        public override void Write(byte[] buffer, int offset, int count) {
            throw new NotImplementedException();
        }

        public override long Length {
            get { throw new NotImplementedException(); }
        }

        public override void Flush() {
            throw new NotImplementedException();
        }
    }
}
