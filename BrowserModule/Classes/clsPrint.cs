using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DF_WebModule.Classes
{
    public class clsPrint
    {
        public List<Customer> customers { get; set; }
        public int cId { get; set; }
        public List<clsPartDetail> clsPartDetails { get; set; }
        public string PartNumber { get; set; }
        public string ScanText { get; set; }
        public List<clsPartDetail> parttype { get; set; }
        public string Type { get; set; }
        public string Printer { get; set; }

        public string PrintType { get; set; }
        public List<string> Printers { get; set; }

        public string VendorCode { get; set; }
        public string Date { get; set; }

        public string SLNO { get; set; }
        

        public int NoOfPrint { get; set; }
        public string Joint { get; set; }

        public string Description { get; set; }
        public string RevNo { get; set; }
        public string TubeLength { get; set; }
    }
}