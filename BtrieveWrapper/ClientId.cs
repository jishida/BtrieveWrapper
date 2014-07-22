using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BtrieveWrapper
{
    public class ClientId
    {
        const int FillerLength = 12;

        internal byte[] Buffer;

        internal ClientId() {
            this.Buffer = new byte[16];
            this.IsLocked = false;
        }

        internal ClientId(string applicationId, ushort threadId) {
            this.Buffer = new byte[16];
            this.ApplicationId = applicationId;
            this.ThreadId = threadId;
            this.IsLocked = true;
        }

        public bool IsLocked { get; private set; }

        public byte[] Filler {
            get {
                var result = new byte[FillerLength];
                Array.Copy(this.Buffer, result, FillerLength);
                return result;
            }
            set {
                if (this.IsLocked) {
                    throw new InvalidOperationException();
                }
                if (value == null) {
                    for (var i = 0; i < FillerLength; i++) {
                        this.Buffer[i] = 0;
                    }
                } else {
                    if (value.Length != FillerLength) {
                        throw new ArgumentException();
                    }
                    Array.Copy(value, this.Buffer, FillerLength);
                }
            }
        }

        public string ApplicationId {
            get {
                return new String(new char[] { (char)this.Buffer[12], (char)this.Buffer[13] });
            }
            set {
                if (this.IsLocked) {
                    throw new InvalidOperationException();
                }
                if (value == null || value == "\0\0") {
                    this.Buffer[12] = 0;
                    this.Buffer[13] = 0;
                } else {
                    if (value.Length != 2) {
                        throw new ArgumentException();
                    }
                    var code1 = (int)value[0];
                    var code2 = (int)value[1];
                    if (code1 < 0x41 || code1 > 0xff || code2 < 0x41 || code2 > 0xff) {
                        throw new ArgumentException();
                    }
                    this.Buffer[12] = (byte)code1;
                    this.Buffer[13] = (byte)code2;
                }
            }
        }

        public ushort ThreadId {
            get {
                return BitConverter.ToUInt16(this.Buffer, 14);
            }
            set {
                if (this.IsLocked) {
                    throw new InvalidOperationException();
                }
                Array.Copy(BitConverter.GetBytes(value), 0, this.Buffer, 14, 2);
            }
        }
    }
}
