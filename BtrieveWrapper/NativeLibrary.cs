using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace BtrieveWrapper
{
    public class NativeLibrary : INativeLibrary
    {
        const string DefaultLibraryPath = "w3btrv7.dll";

        static Dictionary<string, NativeLibrary> _dictionary = new Dictionary<string, NativeLibrary>();

        public static NativeLibrary GetNativeLibrary(string dllPath = null, bool reload = false) {
            if (dllPath == null) {
                dllPath = DefaultLibraryPath;
            }
            if (reload || !_dictionary.ContainsKey(dllPath)) {
                _dictionary[dllPath] = new NativeLibrary(dllPath);
            }
            return _dictionary[dllPath];
        }

        delegate short BtrCallDelegate(ushort operationCode, byte[] positionBlock, byte[] dataBuffer, ref ushort dataLength, byte[] keyBuffer, ushort keyLength, sbyte keyNumber);
        delegate short BtrCallIdDelegate(ushort operationCode, byte[] positionBlock, byte[] dataBuffer, ref ushort dataLength, byte[] keyBuffer, ushort keyLength, sbyte keyNumber, byte[] clientId);

        IntPtr _hModule;
        BtrCallDelegate _btrCall = null;
        BtrCallIdDelegate _btrCallId = null;

        NativeLibrary(string dllPath = null) {
            if (dllPath == null) {
                dllPath = NativeLibrary.DefaultLibraryPath;
            }

            _hModule = NativeMethods.LoadLibrary(dllPath);

            if (_hModule == IntPtr.Zero) {
                throw new ArgumentException();
            }
            try {
                _btrCall = (BtrCallDelegate)Marshal.GetDelegateForFunctionPointer(NativeMethods.GetProcAddress(_hModule, "BTRCALL"), typeof(BtrCallDelegate));
                _btrCallId = (BtrCallIdDelegate)Marshal.GetDelegateForFunctionPointer(NativeMethods.GetProcAddress(_hModule, "BTRCALLID"), typeof(BtrCallIdDelegate));
            } catch {
                throw new ArgumentException();
            }
        }

        public short BtrCall(ushort operationCode, byte[] positionBlock, byte[] dataBuffer, ref ushort dataLength, byte[] keyBuffer, ushort keyLength, sbyte keyNumber) {
            if (_hModule == IntPtr.Zero) {
                throw new ObjectDisposedException(typeof(NativeLibrary).Name);
            }
            if (positionBlock == null || dataBuffer == null || keyBuffer == null) {
                throw new ArgumentNullException();
            }
            return _btrCall(operationCode, positionBlock, dataBuffer, ref dataLength, keyBuffer, keyLength, keyNumber);
        }

        public short BtrCallId(ushort operationCode, byte[] positionBlock, byte[] dataBuffer, ref ushort dataLength, byte[] keyBuffer, ushort keyLength, sbyte keyNumber, byte[] clientId) {
            if (_hModule == IntPtr.Zero) {
                throw new ObjectDisposedException(typeof(NativeLibrary).Name);
            }
            if (positionBlock == null || dataBuffer == null || keyBuffer == null || clientId == null) {
                throw new ArgumentNullException();
            }
            return _btrCallId(operationCode, positionBlock, dataBuffer, ref dataLength, keyBuffer, keyLength, keyNumber, clientId);
        }


        protected virtual void Dispose(bool disposing) {
            if (_hModule != IntPtr.Zero && disposing) {
                _btrCall = null;
                _btrCallId = null;
                NativeMethods.FreeLibrary(_hModule);
                _hModule = IntPtr.Zero;
            }
        }

        public void Dispose() {
            this.Dispose(true);
        }

        ~NativeLibrary() {
            this.Dispose(true);
        }
    }
}
