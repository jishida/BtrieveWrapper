using System;
using System.Runtime.InteropServices;

namespace BtrieveWrapper
{
    static class NativeMethods
    {
#if WINDOWS
        [DllImport("kernel32.dll", EntryPoint = "LoadLibrary", BestFitMapping = false, ThrowOnUnmappableChar = true)]
        public static extern IntPtr LoadLibrary([MarshalAs(UnmanagedType.LPStr)]string lpFileName);
        [DllImport("kernel32.dll", EntryPoint = "GetProcAddress", BestFitMapping = false, ThrowOnUnmappableChar = true)]
        public static extern IntPtr GetProcAddress(IntPtr hModule, [MarshalAs(UnmanagedType.LPStr)]string lpProcName);
        [DllImport("kernel32.dll", EntryPoint = "FreeLibrary")]
        public static extern bool FreeLibrary(IntPtr hModule);
#endif
#if LINUX
		[DllImport("libdl.so")]
		public static extern IntPtr dlopen(string path, int mode);
		[DllImport("libdl.so")]
		public static extern IntPtr dlsym(IntPtr handle, string symbol);
		[DllImport("libdl.so")]
		public static extern int dlclose(IntPtr handle);
#endif
    }
}
