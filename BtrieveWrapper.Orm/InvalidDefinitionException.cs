using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BtrieveWrapper.Orm
{
    [Serializable]
    public class InvalidDefinitionException : Exception
    {
        public InvalidDefinitionException(string message = null, Exception innerException = null)
            : base(message, innerException) { }
    }
}
