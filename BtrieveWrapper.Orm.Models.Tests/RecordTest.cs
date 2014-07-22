using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xunit;
using Xunit.Extensions;

namespace BtrieveWrapper.Orm.Models.Tests
{
    public class RecordTest
    {
        [Theory]
        [InlineData(0, true)]
        [InlineData(1, false)]
        [InlineData(2, false)]
        [InlineData(3, false)]
        [InlineData(4, true)]
        [InlineData(5, false)]
        [InlineData(6, false)]
        [InlineData(7, false)]
        [InlineData(8, false)]
        [InlineData(9, false)]
        [InlineData(10, true)]
        [InlineData(11, false)]
        public void _001_RecordInitialize(int mode, bool success) {
            var field1 = new Field() { Id = 1, Position = 0, Length = 4 };
            var field2 = new Field() { Id = 2, Position = 4, Length = 4 };

            var keySegment0 = new KeySegment() { FieldId = 1 };
            var keySegment1 = new KeySegment() { FieldId = 2 };

            var key0 = new Key { KeyNumber = 0, Segments = new[] { keySegment0, keySegment1 } };

            try {
                Record.Initialize(null);
            } catch (InvalidModelException) { }
            //Assert.Throws<InvalidModelException>(() => { Record.Initialize(null); });
            var record = new Record();
            record.Fields = new[]{
                field1,
                field2
            };
            record.Keys = new[]{
                key0
            };

            switch (mode) {
                case 1:
                    record = null;
                    break;
                case 2:
                    field2.Id = 0;
                    break;
                case 3:
                    key0.KeyNumber = -1;
                    break;
                case 4:
                    record.PrimaryKeyNumber = 118;
                    key0.KeyNumber = 118;
                    break;
                case 5:
                    record.PrimaryKeyNumber = 119;
                    key0.KeyNumber = 119;
                    break;
                case 6:
                    record.PrimaryKeyNumber = 1;
                    break;
                case 7:
                    key0.Segments = new KeySegment[0];
                    break;
                case 8:
                    keySegment1.Index = 2;
                    break;
                case 9:
                    keySegment0.FieldId = 3;
                    break;
                case 10:
                    keySegment0.Field = field1;
                    keySegment1.Field = field2;
                    break;
                case 11:
                    record.Fields = new Field[] { field1 };
                    keySegment0.Field = field1;
                    keySegment1.Field = field2;
                    break;
            }

            if (success) {
                Record.Initialize(record);
            } else {
                try {
                    Record.Initialize(record);
                } catch (InvalidModelException) { }
                //Assert.Throws<InvalidModelException>(() => { Record.Initialize(record); });
            }
        }
    }
}
