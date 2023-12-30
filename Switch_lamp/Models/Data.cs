using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace Switch_lamp.Models
{
    [XmlRoot(ElementName = "Application")]
    public class Data
    {
        [XmlElement(ElementName = "Id")]
        public long Id { get; set; }

        [XmlElement(ElementName = "Content")]
        public string Content { get; set; }

        [XmlElement(ElementName = "Creation_dt")]
        public DateTime Creation_dt { get; set; }
        
        [XmlElement(ElementName = "Parent")]
        public string Parent { get; set;}

        public Data() { }
        
        public Data(string content, string parent) 
        {
            Content = content;
            Parent = parent;
        }
     
    }
}