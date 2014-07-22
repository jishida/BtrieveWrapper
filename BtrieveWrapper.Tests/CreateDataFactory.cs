using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BtrieveWrapper.Tests
{
    class CreateDataFactory
    {
        public static CreateData CreateDefault() {
            return new CreateData(
                new CreateFileSpec(
                    100,
                    4096,
                    FileFlag.None,
                    0,
                    0),
                new CreateKeySpec[]{
                    new CreateKeySpec(0,4,KeyFlag.ExttypeKey,0,0,0,0)
                });
        }
        public static CreateData CreateModifiable() {
            return new CreateData(
                new CreateFileSpec(
                    100,
                    4096,
                    FileFlag.None,
                    0,
                    0),
                new CreateKeySpec[]{
                    new CreateKeySpec(0,4,KeyFlag.ExttypeKey|KeyFlag.Mod,0,0,0,0)
                });
        }
        public static CreateData CreateDuplicatable() {
            return new CreateData(
                new CreateFileSpec(
                    100,
                    4096,
                    FileFlag.None,
                    0,
                    0),
                new CreateKeySpec[]{
                    new CreateKeySpec(0,4,KeyFlag.ExttypeKey|KeyFlag.Dup,0,0,0,0)
                });
        }
        public static CreateData CreateDescKey() {
            return new CreateData(
                new CreateFileSpec(
                    100,
                    4096,
                    FileFlag.None,
                    0,
                    0),
                new CreateKeySpec[]{
                    new CreateKeySpec(0,4,KeyFlag.ExttypeKey|KeyFlag.DescKey,0,0,0,0)
                });
        }
        public static CreateData CreateSegmentKey() {
            return new CreateData(
                new CreateFileSpec(
                    100,
                    4096,
                    FileFlag.None,
                    0,
                    0),
                new CreateKeySpec[]{
                    new CreateKeySpec(0,4,KeyFlag.ExttypeKey|KeyFlag.Seg,0,0,0,0),
                    new CreateKeySpec(4,8,KeyFlag.ExttypeKey,0,0,0,0)
                });
        }
        public static CreateData CreateVariable() {
            return new CreateData(
                new CreateFileSpec(
                    100,
                    4096,
                    FileFlag.VarRecs,
                    0,
                    0),
                new CreateKeySpec[]{
                    new CreateKeySpec(0,4,KeyFlag.ExttypeKey,0,0,0,0)
                });
        }
        public static CreateData CreateCompression() {
            return new CreateData(
                new CreateFileSpec(
                    100,
                    4096,
                    FileFlag.DataComp,
                    0,
                    0),
                new CreateKeySpec[]{
                    new CreateKeySpec(0,4,KeyFlag.ExttypeKey,0,0,0,0)
                });
        }
    }
}
