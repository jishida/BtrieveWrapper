using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BtrieveWrapper
{
    public class CreateData
    {
        public CreateData(CreateFileSpec fileSpec, IEnumerable<CreateKeySpec> keySpecs) {
            if (fileSpec == null || keySpecs == null) {
                throw new ArgumentNullException();
            }
            if (keySpecs.Any(s => s == null)) {
                throw new ArgumentException();
            }
            var keyCount = (byte)keySpecs.Select(s=>s.Number).Distinct().Count();
            var keySpecList = new List<CreateKeySpec>();
            foreach (var keySpec in keySpecs) {
                keySpecList.Add(keySpec);
                if (!keySpec.IsSegmentKey) {
                    var uniqueValues = keySpecList
                        .Select(s => new {
                            DuplicateKeyOption = s.DuplicateKeyOption,
                            IsMod = s.IsModifiable,
                            NullKeyOption = s.NullKeyOption,
                            Number = s.Number
                        })
                        .Distinct();
                    if (uniqueValues.Count() != 1) {
                        throw new ArgumentException();
                    }
                    var keyMap = new bool[keySpecList.Select(s => s.Position + s.Length).Max<int>()];
                    foreach (var keyRange in keySpecList.Select(s => new { Position = s.Position, Length = s.Length })) {
                        for (var i = 0; i < keyRange.Length; i++) {
                            if (keyMap[keyRange.Position + i]) {
                                throw new ArgumentException();
                            } else {
                                keyMap[keyRange.Position + i] = true;
                            }
                        }
                    }
                    keySpecList.Clear();
                }
            }
            if (keySpecList.Count!=0) {
                throw new ArgumentException();
            }
            this.FileSpec = new CreateFileSpec(
                fileSpec.RecordLength,
                fileSpec.PageSize,
                fileSpec.Flag,
                fileSpec.DuplicatedPointerCount,
                fileSpec.Allocation);
            this.FileSpec.KeyCount = keyCount;
            var keySpecArray = new CreateKeySpec[keySpecs.Count()];
            var count = 0;
            foreach (var keySpec in keySpecs) {
                keySpecArray[count] = new CreateKeySpec(
                    keySpec.Position,
                    keySpec.Length,
                    keySpec.Flag,
                    keySpec.ExtendedKeyType,
                    keySpec.NullValue,
                    keySpec.Number,
                    keySpec.AcsNumber);
                count++;
            }
            this.KeySpecs = keySpecArray;
        }

        public CreateFileSpec FileSpec { get; private set; }
        public IEnumerable<CreateKeySpec> KeySpecs { get; private set; }

        public byte[] DataBuffer {
            get {
                var keyCount = KeySpecs.Count();
                var result = new byte[16 * (keyCount + 1)];
                Array.Copy(this.FileSpec.Binary, 0, result, 0, 16);
                var count = 0;
                foreach (var keySpec in this.KeySpecs) {
                    Array.Copy(keySpec.Binary, 0, result, 16 * (count + 1), 16);
                    count++;
                }
                return result;
            }
        }
    }
}
