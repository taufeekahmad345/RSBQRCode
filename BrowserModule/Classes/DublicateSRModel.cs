using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DF_WebModule.Classes
{
    public class DublicateSRModel
    {
        public int id { get; set; }
        public int ExistingCount { get; set; }
        public int NewCount { get; set; }
        public int DuductCount { get; set; }
        public int srno { get; set; }
        public int type { get; set; }
        public DateTime PrintDate { get; set; }
    }
}