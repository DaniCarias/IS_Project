using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace SOMIOD.Models
{
    [XmlRoot(ElementName = "Container")]
    public class Container
    {
        [XmlElement(ElementName = "Id")]
        public int Id { get; set; }

        [XmlElement(ElementName = "Name")]
        public string Name { get; set; }

        [XmlElement(ElementName = "Creation_dt")]
        public DateTime Creation_dt { get; set; }

        [XmlElement(ElementName = "Parent")]
        public int Parent { get; set;}


        public Container() { }

        public Container(int id, string name, DateTime creationDate, int parent)
        {
            Id = id;
            Name = name;
            Creation_dt = creationDate; //.ToString("yyyy-MM-dd HH:mm:ss");
            Parent = parent;
        }

    }
}