using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace BtrieveWrapper
{
    [Serializable]
    [XmlType(AnonymousType = true, Namespace = "urn:btrieve-status-collection-schema")]
    public class Status
    {
        [XmlAttribute("code")]
        public short Code { get; set; }

        [XmlText]
        public string Message { get; set; }
    }
}
