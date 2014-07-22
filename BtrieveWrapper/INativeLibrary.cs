using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BtrieveWrapper
{
    public interface INativeLibrary : IDisposable
    {
        short BtrCall(ushort operationCode, byte[] positionBlock, byte[] dataBuffer, ref ushort dataLength, byte[] keyBuffer, ushort keyLength, sbyte keyNumber);
        short BtrCallId(ushort operationCode, byte[] positionBlock, byte[] dataBuffer, ref ushort dataLength, byte[] keyBuffer, ushort keyLength, sbyte keyNumber, byte[] clientId);
    }
}
