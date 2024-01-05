using System.Xml.Serialization;

namespace Lamp.Models

    {[XmlRoot(ElementName = "Response")]
    public class EventNotification
    {
      
        [XmlElement(ElementName = "EventType")]
        public string EventType { get; set; }

        [XmlElement(ElementName = "Content")]
        public bool Content { get; set; }
        
        public EventNotification() { }
        
        public EventNotification(string eventType, bool content)
        {
            EventType = eventType;
            Content = content;
        }
    }
}