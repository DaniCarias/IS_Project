using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Serialization;

namespace SOMIOD.Models
{

    [XmlRoot(ElementName = "Subscription")]
    public class Subscription
    {
        [XmlElement(ElementName = "Id")]
        public int Id { get; set; }

        [XmlElement(ElementName = "Content")]
        public string Content { get; set; }

        [XmlElement(ElementName = "Creation_dt")]
        public DateTime Creation_dt { get; set; }

        [XmlElement(ElementName = "Parent")]
        public int Parent { get; set; }

        [XmlElement(ElementName = "EventType")]
        public string EventType { get; set; }

        [XmlElement(ElementName = "Endpoint")]
        public string Endpoint { get; set; }


        public Subscription() { }

        public Subscription(int id, string content, DateTime creationDate, int parent, string eventType, string endpoint)
        {
            Id = id;
            Content = content;
            Creation_dt = creationDate; //.ToString("yyyy-MM-dd HH:mm:ss");
            Parent = parent;
            EventType = eventType;
            Endpoint = endpoint;
        }
    }
}