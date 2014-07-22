using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace BtrieveWrapper
{
    [Serializable]
    [XmlRoot("status-collection", Namespace = "urn:btrieve-status-collection-schema", IsNullable = false)]
    public class StatusDefinition
    {
        [XmlElement("status")]
        public Status[] StatusCollection { get; set; }

    }
}
