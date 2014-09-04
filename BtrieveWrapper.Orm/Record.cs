using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace BtrieveWrapper.Orm
{
    public abstract class Record<TRecord> where TRecord : Record<TRecord>
    {
        RecordInfo _info;

        protected Record() {
            _info = Resource.GetRecordInfo(typeof(TRecord));
            this.DataBuffer = new byte[_info.DataBufferCapacity];
            if (_info.DefaultByte != default(byte)) {
                for (var i = 0; i < this.DataBuffer.Length; i++) {
                    this.DataBuffer[i] = _info.DefaultByte;
                }
            }
            foreach (var fieldInfo in _info.Fields) {
                fieldInfo.SetDefaultValue(
                    this.DataBuffer,
                    fieldInfo.Position,
                    fieldInfo.Length,
                    fieldInfo.Parameter);
            }
            this.RecordState = RecordState.Detached;
            this.RollbackedState = RecordState.Detached;
            this.IsManagedMember = false;
            this.IsRollbackedMember = false;
        }

        protected Record(byte[] dataBuffer) {
            if (dataBuffer == null) {
                throw new ArgumentNullException();
            }
            _info = Resource.GetRecordInfo(typeof(TRecord));
            if (dataBuffer.Length != _info.DataBufferCapacity) {
                throw new ArgumentException();
            }
            this.DataBuffer = dataBuffer;
            this.RecordState = RecordState.Detached;
            this.RollbackedState = RecordState.Detached;
            this.IsManagedMember = false;
            this.IsRollbackedMember = false;
        }

        public RecordState RecordState { get; private set; }
        internal bool IsManagedMember { get; set; }
        internal RecordState RollbackedState { get; set; }
        internal bool IsRollbackedMember { get; set; }
        public bool HasPhysicalPosition { get;internal set; }
        public uint PhysicalPosition { get;internal set; }

        internal byte[] DataBuffer { get; private set; }
        
        internal void Initialize(byte[] dataBuffer, bool byReference) {
            if (byReference) {
                this.DataBuffer = dataBuffer;
            } else {
                Array.Copy(dataBuffer, this.DataBuffer, this.DataBuffer.Length);
            }
            this.RecordState = RecordState.Detached;
            this.RollbackedState = RecordState.Detached;
            this.IsManagedMember = false;
            this.IsRollbackedMember = false;
        }

        public KeyValue GetKeyValue(KeyInfo key) {
            if (key == null) {
                throw new ArgumentNullException();
            }
            var keyBuffer = new byte[key.Length];
            var position=0;
            foreach (var segment in key.Segments) {
                Array.Copy(this.DataBuffer, segment.Field.Position, keyBuffer, position, segment.Length);
                position += segment.Length;
            }
            return new KeyValue(key, keyBuffer);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        protected object GetValue() {
            var method = new StackFrame(1).GetMethod();
            var fieldInfo = Resource.GetFieldInfo(method);
            if (fieldInfo == null) {
                throw new InvalidDefinitionException();
            }
            var result = fieldInfo.Convert(this.DataBuffer);
            return result;
        }

        protected object GetValue(string propertyName) {
            if (propertyName == null) {
                throw new ArgumentNullException();
            }
            var fieldInfo = Resource.GetFieldInfo(typeof(TRecord), propertyName);
            if (fieldInfo == null) {
                throw new InvalidDefinitionException();
            }
            var result = fieldInfo.Convert(this.DataBuffer);
            return result;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        protected void SetValue(object value) {
            if (this.RecordState == RecordState.Deleted) {
                throw new InvalidOperationException();
            }
            var method = new StackFrame(1).GetMethod();
            var fieldInfo = Resource.GetFieldInfo(method);
            if (fieldInfo == null) {
                throw new InvalidDefinitionException();
            }
            if ((fieldInfo.IsPrimaryKeySegment ||
                    !fieldInfo.IsModifiable) &&
                this.RecordState != RecordState.Detached) {
                throw new InvalidOperationException();
            }
            fieldInfo.ConvertBack(value, this.DataBuffer);
            this.ChangeState(RecordStateTransitions.Modify);
        }

        protected void SetValue(string propertyName, object value) {
            if (this.RecordState == RecordState.Deleted) {
                throw new InvalidOperationException();
            }
            if (propertyName == null) {
                throw new ArgumentNullException();
            }
            var fieldInfo = Resource.GetFieldInfo(typeof(TRecord), propertyName);
            if (fieldInfo == null) {
                throw new InvalidDefinitionException();
            }
            if ((fieldInfo.IsPrimaryKeySegment ||
                    !fieldInfo.IsModifiable) &&
                this.RecordState != RecordState.Detached) {
                throw new InvalidOperationException();
            }
            fieldInfo.ConvertBack(value, this.DataBuffer);
            this.ChangeState(RecordStateTransitions.Modify);
        }

        public void SetModified() {
            this.ChangeState(RecordStateTransitions.Modify);
        }

        internal void Commit() {
            this.RollbackedState = this.RecordState;
        }

        internal void Rollback() {
            this.RecordState = this.RollbackedState;
        }

        internal void SetUnchanged() {
            this.RecordState = RecordState.Unchanged;
            this.RollbackedState = RecordState.Unchanged;
        }

        internal void ChangeState(RecordStateTransitions transition, bool saveInTransaction = false) {
            this.RecordState = GetChangedState(this.RecordState, transition);
            if (!saveInTransaction) {
                this.RollbackedState = GetChangedState(this.RollbackedState, transition);
            }
        }

        static RecordState GetChangedState(RecordState state, RecordStateTransitions transition) {
            switch (state) {
                case RecordState.Added:
                    switch (transition) {
                        case RecordStateTransitions.Save:
                            return RecordState.Unchanged;
                        case RecordStateTransitions.Delete:
                        case RecordStateTransitions.Detach:
                            return RecordState.Detached;
                        case RecordStateTransitions.Modify:
                            return RecordState.Added;
                        case RecordStateTransitions.Attach:
                            throw new InvalidOperationException();
                    }
                    break;
                case RecordState.Modified:
                    switch (transition) {
                        case RecordStateTransitions.Save:
                            return RecordState.Unchanged;
                        case RecordStateTransitions.Delete:
                            return RecordState.Deleted;
                        case RecordStateTransitions.Modify:
                            return RecordState.Modified;
                        case RecordStateTransitions.Attach:
                            throw new InvalidOperationException();
                        case RecordStateTransitions.Detach:
                            return RecordState.Detached;
                    }
                    break;
                case RecordState.Deleted:
                    switch (transition) {
                        case RecordStateTransitions.Save:
                        case RecordStateTransitions.Detach:
                            return RecordState.Detached;
                        case RecordStateTransitions.Delete:
                            throw new InvalidOperationException();
                        case RecordStateTransitions.Modify:
                            return RecordState.Deleted;
                        case RecordStateTransitions.Attach:
                            return RecordState.Modified;
                    }
                    break;
                case RecordState.Unchanged:
                    switch (transition) {
                        case RecordStateTransitions.Save:
                            return RecordState.Unchanged;
                        case RecordStateTransitions.Delete:
                            return RecordState.Deleted;
                        case RecordStateTransitions.Modify:
                            return RecordState.Modified;
                        case RecordStateTransitions.Attach:
                            throw new InvalidOperationException();
                        case RecordStateTransitions.Detach:
                            return RecordState.Detached;
                    }
                    break;
                case RecordState.Detached:
                    switch (transition) {
                        case RecordStateTransitions.Save:
                        case RecordStateTransitions.Delete:
                            throw new InvalidOperationException();
                        case RecordStateTransitions.Attach:
                            return RecordState.Added;
                        case RecordStateTransitions.Modify:
                        case RecordStateTransitions.Detach:
                            return RecordState.Detached;
                    }
                    break;
            }
            throw new NotSupportedException();
        }

        public static TKeyCollection GetKeyCollection<TKeyCollection>()
            where TKeyCollection : KeyCollection<TRecord>, new() {
            return Resource.GetKeyCollection<TKeyCollection>() as TKeyCollection;
        }
    }
}
