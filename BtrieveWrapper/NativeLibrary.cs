using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace BtrieveWrapper
{
    public class NativeLibrary : INativeLibrary
    {
#if WINDOWS
		static readonly string DefaultLibraryPath = IntPtr.Size == 8 ? "w64btrv7.dll" : "w3btrv7.dll";
		static readonly IEnumerable<string> DefaultDependencyPaths = null;
#endif
#if LINUX
		const int RTLD_NOW = 0x0002;
		const int RTLD_GLOBAL = 0x0100;
		static readonly string DefaultLibraryPath = IntPtr.Size == 8 
			? "/usr/local/psql/lib64/libpsqlmif.so" 
			: "/usr/local/psql/lib/libpsqlmif.so";
		static readonly IEnumerable<string> DefaultDependencyPaths = IntPtr.Size == 8 
			? new[] { "/usr/local/psql/lib64/libpscore.so.3", "/usr/local/psql/lib64/libpscl.so.3"}
			: new[] { "/usr/local/psql/lib/libpscore.so.3", "/usr/local/psql/lib/libpscl.so.3"};
#endif


        static Dictionary<string, NativeLibrary> _dictionary = new Dictionary<string, NativeLibrary>();

		public static NativeLibrary GetNativeLibrary(string dllPath = null, IEnumerable<string> dependencyPaths = null , bool reload = false) {
            if (dllPath == null) {
                dllPath = DefaultLibraryPath;
            }
            if (reload || !_dictionary.ContainsKey(dllPath)) {
				_dictionary[dllPath] = new NativeLibrary(dllPath, dependencyPaths);
            }
            return _dictionary[dllPath];
        }

        delegate short BtrCallDelegate(ushort operationCode, byte[] positionBlock, byte[] dataBuffer, ref ushort dataLength, byte[] keyBuffer, ushort keyLength, sbyte keyNumber);
        delegate short BtrCallIdDelegate(ushort operationCode, byte[] positionBlock, byte[] dataBuffer, ref ushort dataLength, byte[] keyBuffer, ushort keyLength, sbyte keyNumber, byte[] clientId);

		IntPtr _handle;
		List<IntPtr> _dependencyHandles = null;
        BtrCallDelegate _btrCall = null;
        BtrCallIdDelegate _btrCallId = null;

		NativeLibrary(string dllPath = null, IEnumerable<string> dependencyPaths = null) {
			if (dllPath == null) {
				dllPath = NativeLibrary.DefaultLibraryPath;
			}
			if (dependencyPaths == null) {
				dependencyPaths = NativeLibrary.DefaultDependencyPaths;
			}
#if WINDOWS
			if (dependencyPaths != null) {
				_dependencyHandles = new List<IntPtr>();
				foreach(var dependencyPath in dependencyPaths) {
					var handle = NativeMethods.LoadLibrary(dependencyPath);
					if(handle != IntPtr.Zero) {
						_dependencyHandles.Add(handle);
					}
				}
			}
			_handle = NativeMethods.LoadLibrary(dllPath);
			if (_handle == IntPtr.Zero) {
                if (_dependencyHandles != null) {
                    foreach (var dependencyHandle in Enumerable.Reverse(_dependencyHandles)) {
                        NativeMethods.FreeLibrary(dependencyHandle);
                    }
                }
                throw new ArgumentException();
			}
			var btrCallFunctionPointer = NativeMethods.GetProcAddress(_handle, "BTRCALL");
			var btrCallIdFunctionPointer = NativeMethods.GetProcAddress(_handle, "BTRCALLID");
			try {
				_btrCall = (BtrCallDelegate)Marshal.GetDelegateForFunctionPointer(btrCallFunctionPointer, typeof(BtrCallDelegate));
				_btrCallId = (BtrCallIdDelegate)Marshal.GetDelegateForFunctionPointer(btrCallIdFunctionPointer, typeof(BtrCallIdDelegate));
			} catch {
				foreach(var dependencyHandle in _dependencyHandles) {
					NativeMethods.FreeLibrary(dependencyHandle);
				}
				NativeMethods.FreeLibrary(_handle);
				throw new ArgumentException();
			}
#endif
#if LINUX
			if (dependencyPaths != null) {
				_dependencyHandles = new List<IntPtr>();
				foreach(var dependencyPath in dependencyPaths) {
					var handle = NativeMethods.dlopen(dependencyPath, RTLD_NOW|RTLD_GLOBAL);
					if(handle != IntPtr.Zero) {
						_dependencyHandles.Add(handle);
					}
				}
			}
			_handle = NativeMethods.dlopen(dllPath, RTLD_NOW|RTLD_GLOBAL);
			if (_handle == IntPtr.Zero) {
                if (_dependencyHandles != null) {
                    foreach (var dependencyHandle in Enumerable.Reverse(_dependencyHandles)) {
		    			NativeMethods.dlclose(dependencyHandle);
			    	}
                }
				throw new ArgumentException();
			}
			var btrCallFunctionPointer = NativeMethods.dlsym(_handle, "BTRCALL");
			var btrCallIdFunctionPointer = NativeMethods.dlsym(_handle, "BTRCALLID");
			try {
				_btrCall = (BtrCallDelegate)Marshal.GetDelegateForFunctionPointer(btrCallFunctionPointer, typeof(BtrCallDelegate));
				_btrCallId = (BtrCallIdDelegate)Marshal.GetDelegateForFunctionPointer(btrCallIdFunctionPointer, typeof(BtrCallIdDelegate));
			} catch {
				foreach(var dependencyHandle in _dependencyHandles) {
					NativeMethods.dlclose(dependencyHandle);
				}
				NativeMethods.dlclose(_handle);
				throw new ArgumentException();
			}
#endif
        }

        public short BtrCall(ushort operationCode, byte[] positionBlock, byte[] dataBuffer, ref ushort dataLength, byte[] keyBuffer, ushort keyLength, sbyte keyNumber) {
            if (_handle == IntPtr.Zero) {
                throw new ObjectDisposedException(typeof(NativeLibrary).Name);
            }
            if (positionBlock == null || dataBuffer == null || keyBuffer == null) {
                throw new ArgumentNullException();
            }
            return _btrCall(operationCode, positionBlock, dataBuffer, ref dataLength, keyBuffer, keyLength, keyNumber);
        }

        public short BtrCallId(ushort operationCode, byte[] positionBlock, byte[] dataBuffer, ref ushort dataLength, byte[] keyBuffer, ushort keyLength, sbyte keyNumber, byte[] clientId) {
            if (_handle == IntPtr.Zero) {
                throw new ObjectDisposedException(typeof(NativeLibrary).Name);
            }
            if (positionBlock == null || dataBuffer == null || keyBuffer == null || clientId == null) {
                throw new ArgumentNullException();
            }
            return _btrCallId(operationCode, positionBlock, dataBuffer, ref dataLength, keyBuffer, keyLength, keyNumber, clientId);
        }


        protected virtual void Dispose(bool disposing) {
            if (_handle != IntPtr.Zero && disposing) {
                _btrCall = null;
                _btrCallId = null;
#if WINDOWS
                if (_dependencyHandles != null) {
                    foreach (var dependencyHandle in Enumerable.Reverse(_dependencyHandles)) {
                        NativeMethods.FreeLibrary(dependencyHandle);
                    }
                }
                NativeMethods.FreeLibrary(_handle);
#endif
#if LINUX
                if (_dependencyHandles != null) {
                    foreach (var dependencyHandle in Enumerable.Reverse(_dependencyHandles)) {
    					NativeMethods.dlclose(dependencyHandle);
				    }
                }
				NativeMethods.dlclose(_handle);
#endif
                _dependencyHandles = null;
                _handle = IntPtr.Zero;
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
