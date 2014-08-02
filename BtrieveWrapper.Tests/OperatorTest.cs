using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xunit;
using Xunit.Extensions;

namespace BtrieveWrapper.Tests
{
    public class OperatorTest : IDisposable
    {
        Operator _operator;
        Temporary _temp;

        public OperatorTest() {
            _temp = new Temporary("OperatorTest");
            _operator = new Operator();
            _operator.Reset();
        }

        public void Dispose() {
            _operator.Reset();
        }

        [Fact]
        public void _001_Create_Open_Stat_Close() {
            var path = _temp.GetPath("001.mkd");
            var createData = CreateDataFactory.CreateDefault();
            _operator.Create(path, createData, true);
            try {
                _operator.Create(path, createData, false);
            } catch (OperationException e) {
                Assert.Equal(59, e.StatusCode);
            }
            _operator.Create(path, createData, true);
            var positionBlock = _operator.Open(path);
            var statData = _operator.Stat(positionBlock);
            _operator.Close(positionBlock);
            Assert.Equal(createData.FileSpec.Flag & (FileFlag)0xedff, statData.FileSpec.Flag & (FileFlag)0xedff);
            Assert.Equal(createData.FileSpec.RecordLength, statData.FileSpec.RecordLength);
            Assert.Equal(createData.FileSpec.PageSize, statData.FileSpec.PageSize);
            var createKeySpecs = createData.KeySpecs.GetEnumerator();
            var statKeySpecs = statData.KeySpecs.GetEnumerator();
            while (createKeySpecs.MoveNext()) {
                statKeySpecs.MoveNext();
                Assert.Equal(createKeySpecs.Current.Position, statKeySpecs.Current.Position);
                Assert.Equal(createKeySpecs.Current.Length, statKeySpecs.Current.Length);
                Assert.Equal(createKeySpecs.Current.KeyType, statKeySpecs.Current.KeyType);
                Assert.Equal(createKeySpecs.Current.Flag, statKeySpecs.Current.Flag);
            }
        }

        [Fact]
        public void _002_SetOwner_ClearOwner() {
            var path = _temp.GetPath("002.mkd");
            var createData = CreateDataFactory.CreateDefault();
            _operator.Create(path, createData, true);

            var positionBlock = _operator.Open(path);
            _operator.SetOwner(positionBlock, "Ab123456", OwnerNameOption.NoEncryption);
            _operator.Close(positionBlock);

            try {
                _operator.Open(path);
            } catch (OperationException e) {
                Assert.Equal(51, e.StatusCode);
            }

            positionBlock = _operator.Open(path, "Ab123456");
            _operator.ClearOwner(positionBlock);
            _operator.Close(positionBlock);

            positionBlock = _operator.Open(path);
            _operator.Close(positionBlock);
        }

        [Fact]
        public void _003_Insert_Update_Delete_Step() {
            var random = new Random(0);
            var path = _temp.GetPath("003.mkd");
            var createData = CreateDataFactory.CreateDefault();
            _operator.Create(path, createData, true);

            var firstRecord = new byte[100];
            var secondRecord = new byte[100];
            var updateFirstRecord = new byte[100];
            var updateSecondRecord = new byte[100];
            var readFirstRecord = new byte[100];
            var readSecondRecord = new byte[100];
            var temporaryBuffer = new byte[100];

            random.NextBytes(firstRecord);
            random.NextBytes(secondRecord);
            random.NextBytes(updateFirstRecord);
            random.NextBytes(updateSecondRecord);
            firstRecord[0] = 0;
            secondRecord[0] = 1;
            Array.Copy(firstRecord, updateFirstRecord, 4);
            Array.Copy(secondRecord, updateSecondRecord, 4);


            var positionBlock = _operator.Open(path);
            _operator.Insert(positionBlock, firstRecord);
            _operator.Insert(positionBlock, secondRecord);
            var statData = _operator.Stat(positionBlock);
            Assert.Equal((uint)2, statData.FileSpec.RecordCount);
            _operator.StepFirst(positionBlock, readFirstRecord);
            _operator.StepNext(positionBlock, readSecondRecord);
            Assert.Equal(firstRecord, readFirstRecord);
            Assert.Equal(secondRecord, readSecondRecord);
            try {
                _operator.StepNext(positionBlock, temporaryBuffer);
            } catch (OperationException e) {
                Assert.Equal(9, e.StatusCode);
            }
            _operator.StepLast(positionBlock, readSecondRecord);
            _operator.StepPrevious(positionBlock, readFirstRecord);
            Assert.Equal(firstRecord, readFirstRecord);
            Assert.Equal(secondRecord, readSecondRecord);
            try {
                _operator.StepPrevious(positionBlock, temporaryBuffer);
            } catch (OperationException e) {
                Assert.Equal(9, e.StatusCode);
            }
            _operator.StepFirst(positionBlock, temporaryBuffer);
            _operator.Update(positionBlock, updateFirstRecord);
            _operator.StepNext(positionBlock, temporaryBuffer);
            _operator.Update(positionBlock, updateSecondRecord);
            _operator.StepFirst(positionBlock, readFirstRecord);
            _operator.StepNext(positionBlock, readSecondRecord);
            Assert.Equal(updateFirstRecord, readFirstRecord);
            Assert.Equal(updateSecondRecord, readSecondRecord);
            _operator.StepFirst(positionBlock, temporaryBuffer);
            _operator.Delete(positionBlock);
            _operator.StepFirst(positionBlock, temporaryBuffer);
            _operator.Delete(positionBlock);
            try {
                _operator.StepFirst(positionBlock, temporaryBuffer);
            } catch (OperationException e) {
                Assert.Equal(9, e.StatusCode);
            }
            _operator.Close(positionBlock);


        }
    }
}
