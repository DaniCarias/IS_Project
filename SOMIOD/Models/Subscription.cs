using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace SOMIOD.Models
{
    public class Subscription
    {
        public int id { get; set; }
        public string content { get; set; }
        public DateTime creation_dt { get; set; }
        public int parent { get; set; }
        public string evt { get; set; }
        public string endpoint { get; set; }

    }
}