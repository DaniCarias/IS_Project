using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace Lamp.Models
{
    [XmlRoot(ElementName = "Application")]
    public class Application
    {
        [XmlElement(ElementName = "Id")]
        public long Id { get; set; }

        [XmlElement(ElementName = "Name")]
        public string Name { get; set; }

        [XmlElement(ElementName = "Creation_dt")]
        public DateTime Creation_dt { get; set; }


        public Application() { }
        
        public Application(string name)
        {
            Name = name;
        }

    }
}