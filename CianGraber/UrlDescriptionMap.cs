using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CianGraber
{
    public class UrlDescriptionMap
    {
        public List<District> Districts { get; set; }
        public List<UrlDescription> UrlDescriptions { get; set; }
        


    }

    public class UrlDescription
    {
        public string Url { get; set; }
        public string Description { get; set; }

        public List<District> Districts { get; set; }
    }
}
