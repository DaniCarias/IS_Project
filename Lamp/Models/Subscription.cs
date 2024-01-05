using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace Lamp.Models
{

    [XmlRoot(ElementName = "Subscription")]
    public class Subscription
    {
        [XmlElement(ElementName = "Id")]
        public long Id { get; set; }

        [XmlElement(ElementName = "Name")]
        public string Name { get; set; }

        [XmlElement(ElementName = "Creation_dt")]
        public DateTime Creation_dt { get; set; }

        [XmlElement(ElementName = "Parent")]
        public string Parent { get; set; } //Container id

        [XmlElement(ElementName = "EventType")]
        public string EventType { get; set; }

        [XmlElement(ElementName = "Endpoint")]
        public string Endpoint { get; set; }


        public Subscription() { }

        public Subscription(string name, string parent, string eventType, string endpoint)
        {
            Name = name;
            Parent = parent;
            EventType = eventType;
            Endpoint = endpoint;
        }
    }
}