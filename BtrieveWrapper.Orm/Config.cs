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

        static Config() {
            Config.TemporaryBufferQueueCapacity = 10;
            Config.ComparedStringEncoding = Encoding.Default;
        }
    }
}
