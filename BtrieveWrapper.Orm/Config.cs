using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BtrieveWrapper.Orm
{
    public class Config
    {
        public static int TemporaryBufferQueueCapacity { get; set; }
        public static Encoding ComparedStringEncoding { get; set; }
        public static ushort MaxBufferLength { get; set; }

        static Config() {
            Config.TemporaryBufferQueueCapacity = 10;
            Config.ComparedStringEncoding = Encoding.Default;
            Config.MaxBufferLength = 65135;
        }
    }
}
