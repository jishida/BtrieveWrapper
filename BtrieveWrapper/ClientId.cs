using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using BtrieveWrapper.Utilities;

namespace BtrieveWrapper
{
    public class ClientId
    {
        internal byte[] Buffer;

        internal ClientId(string applicationId, ushort threadId) {
            this.Buffer = new byte[16];
            this.ApplicationId = applicationId;
            this.ThreadId = threadId;
        }

        public string ApplicationId {
            get {
                return new String(new char[] { (char)this.Buffer[12], (char)this.Buffer[13] });
            }
            private set {
                if (value == null || value == "\0\0") {
                    this.Buffer[12] = 0;
                    this.Buffer[13] = 0;
                } else {
                    var applicationId = Encoding.ASCII.GetBytes(value);
                    if (applicationId.Length != 2) {
                        throw new ArgumentException();
                    }
                    Array.Copy(applicationId, 0, this.Buffer, 12, 2);
                }
            }
        }

        public ushort ThreadId {
            get {
                return this.Buffer.GetUInt16(14);
            }
            private set {
                value.SetBytes(this.Buffer, 14);
            }
        }
    }
}
