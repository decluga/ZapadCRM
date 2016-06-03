using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace zapad.Public.WebInterface.Models.Tools
{
    public class KendoFilterDescription
    {
        public string @operator { get; set; }
        public string field { get; set; }
        public string value { get; set; }

        public List<KendoFilterDescription> filters { get; set; }
        public string logic { get; set; }
    }

    public class KendoFilter
    {
        public List<KendoFilterDescription> filters { get; set; }
        public string logic { get; set; }
    }

    public class KendoSort
    {
        public string field { get; set; }
        public string dir { get; set; }
    }
}