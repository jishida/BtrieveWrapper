using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BtrieveWrapper.Tests
{
    public class Settings
    {
#if WINDOWS
        public const string TemporaryDirectory = @"C:\tmp\BtrieveWrapper.Tests";
#endif
#if LINUX
		public const string TemporaryDirectory = @"~/tmp/BtrieveWrapper.Tests";
#endif
    }
}
