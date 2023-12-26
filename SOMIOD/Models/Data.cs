using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace SOMIOD.Models
{
    [XmlRoot(ElementName = "Data")]
    public class Data
    {
        [XmlElement(ElementName = "Id")]
        public int Id { get; set; }

        [XmlElement(ElementName = "Content")]
        public string Content { get; set; }

        [XmlElement(ElementName = "Creation_dt")]
        public DateTime Creation_dt { get; set; }

        [XmlElement(ElementName = "Parent")]
        public int Parent { get; set; }


        public Data() { }

        public Data(int id, string content, DateTime creationDate, int parent)
        {
            {
                Id = id;
                Content = content;
                Creation_dt = creationDate; //.ToString("yyyy-MM-dd HH:mm:ss");
                Parent = parent;
            }
        }
    }
}