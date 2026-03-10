using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace DF_WebModule.Classes
{
    public class clsconfig
    {
        public int Id { get; set; }
        [DisplayName("Customer Name : ")]
        public string CompanyName { get; set; }
        [DisplayName("Address : ")]
        public string Address { get; set; }
        [DisplayName("City : ")]
        public string City { get; set; }
        [DisplayName("Pin Code : ")]
        public string PinNo { get; set; }
        [DisplayName("Phone : ")]
        public string Phone { get; set; }
        [DisplayName("Mobile : ")]
        public string Mobile { get; set; }
        [DisplayName("E-Mail : ")]
        public string Email { get; set; }
        [DisplayName("Fax : ")]
        public string Fax { get; set; }
        [DisplayName("TIN NO : ")]
        public string TINNO { get; set; }
        [DisplayName("Excise No : ")]
        public string ExiceNo { get; set; }
        [DisplayName("CST No : ")]
        public string CSTNo { get; set; }
       
    }
}