using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DF_WebModule.Classes
{
    public class FilterModel
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string Plant { get;
            set;
        }
      public string CustomerName { get; set; }
        public string ReportId { get; set; }
    }
}