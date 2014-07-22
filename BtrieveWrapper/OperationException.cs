using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Text;
using System.Xml.Serialization;

namespace BtrieveWrapper
{
    [Serializable]
    public class OperationException : Exception
    {
        static readonly string _statusDefinitionPath = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name + ".StatusCollection.xml";
        static readonly Dictionary<short, string> _dictionary = new Dictionary<short, string>();
        static string GetMessage(Operation operationType, short statusCode){
            var result = new StringBuilder("Status code ");
            result.Append(statusCode);
            result.Append(" (");
            result.Append(operationType);
            result.Append(") : ");

            if (_dictionary.ContainsKey(statusCode)) {
                result.Append(_dictionary[statusCode]);
            } else {
                result.Append("This code is undefined.");
            }
            return result.ToString();
        }

        static OperationException() {
            try {
                StatusDefinition statusDefinition;
                using (var stream = new FileStream(_statusDefinitionPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)) {
                    var serializer = new XmlSerializer(typeof(StatusDefinition));
                    statusDefinition = (StatusDefinition)serializer.Deserialize(stream);
                }
                foreach (var status in statusDefinition.StatusCollection) {
                    _dictionary[status.Code] = status.Message;
                }
            } catch { }
        }

        internal OperationException(Operation operationType, short statusCode, Exception innerException = null)
            : base(GetMessage(operationType, statusCode), innerException) {
            this.StatusCode = statusCode;
        }

        public short StatusCode { get; private set; }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context) {
            base.GetObjectData(info, context);
        }
    }
}
