using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DF_WebModule.Models
{
    public class PrintRequest
    {
        public string Printer { get; set; }
        public string PartNumber { get; set; }
        public int PrintType { get; set; }
        public string Customer { get; set; }
        public string ScanText { get; set; }
        public int printcount { get; set; }
    }
}