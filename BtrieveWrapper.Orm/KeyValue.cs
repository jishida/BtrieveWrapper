using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BtrieveWrapper.Orm
{
    public class KeyValue
    {
        public KeyValue(KeyInfo key, object[] segmentValues, bool isMinimumComplement = true) {
            if (key == null || segmentValues == null) {
                throw new ArgumentNullException();
            }
            if (segmentValues.Length == 0 || segmentValues.Length > key.Segments.Count()) {
                throw new ArgumentException();
            }
            this.Key = key;
            this.KeyBuffer = new byte[this.Key.Length];
            this.SetValues(segmentValues, 0, isMinimumComplement);
        }

        internal KeyValue(KeyInfo key, byte[] keyBuffer) {
            if (key == null || keyBuffer == null) {
                throw new ArgumentNullException();
            }
            if (key.Length < keyBuffer.Length) {
                throw new ArgumentException();
            }
            this.Key = key;
            this.KeyBuffer = keyBuffer;
            this.ComplementCount = 0;
        }

        public object this[ushort index] { get { return this.SegmentValues.ElementAt(index); } }
        public object this[KeySegmentInfo keySegment] {
            get {
                return this.Key.Segments.Any(s => s == keySegment)
                    ? keySegment.Field.Convert(this.KeyBuffer, keySegment.Position, keySegment.Length, keySegment.Field.Parameter)
                    : null;
            }
            internal set {
                keySegment.Field.ConvertBack(value, this.KeyBuffer, keySegment.Position, keySegment.Length, keySegment.Field.Parameter);
            }
        }
        public IEnumerable<object> SegmentValues {
            get {
                return this.Key.Segments
                    .Select(s => s.Field.Convert(this.KeyBuffer, s.Position, s.Length, s.Field.Parameter));
            }
        }

        public KeyInfo Key { get; private set; }
        public int ComplementCount { get; internal set; }

        internal byte[] KeyBuffer { get; private set; }

        internal void SetValues(object[] segmentValues, int startIndex = 0, bool isMinimumComplement = true) {
            if (segmentValues == null) {
                throw new ArgumentNullException();
            }
            var segments = this.Key.Segments.ToArray();
            if (segmentValues.Length + startIndex > segments.Length) {
                throw new ArgumentException();
            }
            this.ComplementCount = segments.Length - segmentValues.Length;

            for (var i = startIndex; i < segments.Length; i++) {
                if (i - startIndex < segmentValues.Length) {
                    if (segmentValues[i - startIndex].GetType() == typeof(KeyValueSegmentState)) {
                        switch (((KeyValueSegmentState)segmentValues[i - startIndex])) {
                            case KeyValueSegmentState.Maximum:
                                if (segments[i].IsDescending) {
                                    segments[i].Field
                                        .SetMinValue(this.KeyBuffer, segments[i].Position, segments[i].Length, segments[i].Field.Parameter);
                                } else {
                                    segments[i].Field
                                        .SetMaxValue(this.KeyBuffer, segments[i].Position, segments[i].Length, segments[i].Field.Parameter);
                                }
                                break;
                            case KeyValueSegmentState.Minmum:
                                if (segments[i].IsDescending) {
                                    segments[i].Field
                                        .SetMaxValue(this.KeyBuffer, segments[i].Position, segments[i].Length, segments[i].Field.Parameter);
                                } else {
                                    segments[i].Field
                                        .SetMinValue(this.KeyBuffer, segments[i].Position, segments[i].Length, segments[i].Field.Parameter);
                                }
                                break;
                            case KeyValueSegmentState.Default:
                                segments[i].Field
                                    .SetDefaultValue(this.KeyBuffer, segments[i].Position, segments[i].Length, segments[i].Field.Parameter);
                                break;
                            default:
                                throw new NotSupportedException();
                        }
                    } else {
                        segments[i].Field
                            .ConvertBack(segmentValues[i - startIndex], this.KeyBuffer, segments[i].Position, segments[i].Length, segments[i].Field.Parameter);
                    }
                } else {
                    if ((isMinimumComplement && !segments[i].IsDescending) ||
                        (!isMinimumComplement && segments[i].IsDescending)) {
                        segments[i].Field.SetMinValue(this.KeyBuffer, segments[i].Position, segments[i].Length, segments[i].Field.Parameter);
                    } else {
                        segments[i].Field.SetMaxValue(this.KeyBuffer, segments[i].Position, segments[i].Length, segments[i].Field.Parameter);
                    }
                }
            }
        }

        internal void CopyTo(KeyValue keyValue) {
            Array.Copy(this.KeyBuffer, keyValue.KeyBuffer, this.KeyBuffer.Length);
            keyValue.ComplementCount = this.ComplementCount;
        }

        internal KeyValue DeepCopy() {
            var keyBuffer = new byte[this.KeyBuffer.Length];
            Array.Copy(this.KeyBuffer, keyBuffer, this.KeyBuffer.Length);
            var result = new KeyValue(this.Key, keyBuffer);
            result.ComplementCount = this.ComplementCount;
            return result;
        }

        public static KeyValue Create(KeyInfo key, params object[] segmentValues) {
            if (key.Segments.Count != segmentValues.Length) {
                throw new ArgumentException();
            }
            return new KeyValue(key, segmentValues);
        }

        public static KeyValue MinimumComplement(KeyInfo key, params object[] segmentValues) {
            return new KeyValue(key, segmentValues);
        }

        public static KeyValue MaximumComplement(KeyInfo key, params object[] segmentValues) {
            return new KeyValue(key, segmentValues, false);
        }
    }
}
