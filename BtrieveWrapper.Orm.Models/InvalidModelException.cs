using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BtrieveWrapper.Orm.Models
{
    [Serializable]
    public class InvalidModelException : Exception
    {
        public InvalidModelException(Exception exception = null) : base(exception == null ? null : exception.Message, exception) { }
    }
}
