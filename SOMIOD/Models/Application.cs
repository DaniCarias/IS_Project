using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace SOMIOD.Models
{
    [XmlRoot(ElementName = "Application")]
    public class Application
    {
        [XmlElement(ElementName = "Id")]
        public int Id { get; set; }

        [XmlElement(ElementName = "Name")]
        public string Name { get; set; }

        [XmlElement(ElementName = "Creation_dt")]
        public DateTime Creation_dt { get; set; }


        public Application() { }

        public Application(int id, string name, DateTime creationDate)
        {
            Id = id;
            Name = name;
            Creation_dt = creationDate; //.ToString("yyyy-MM-dd HH:mm:ss");
        }

    }
}