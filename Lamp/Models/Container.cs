using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace Lamp.Models
{
    [XmlRoot(ElementName = "Container")]
    public class Container
    {
        [XmlElement(ElementName = "Id")]
        public long Id { get; set; }

        [XmlElement(ElementName = "Name")]
        public string Name { get; set; }

        [XmlElement(ElementName = "Creation_dt")]
        public DateTime Creation_dt { get; set; }

        [XmlElement(ElementName = "Parent")]
        public string Parent { get; set;}


        public Container() { }

     
        public Container(string name, string parent) 
        {
            Name = name;
            Parent = parent;
        }

    }
}