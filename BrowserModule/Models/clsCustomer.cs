using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace DF_WebModule.Models
{
    public class clsCustomer
    {
        public int Id { get; set; }
        [DisplayName("S.NO")]
        public string SNo { get; set; }
        [DisplayName("Customer Id*")]
        public string CustomerId { get; set; }
        [DisplayName("Customer Name")]
        public string CustomerName { get; set; }
        [DisplayName("Address*")]
        public string Address { get; set; }
        [DisplayName("City*")]
        public string City { get; set; }
        [DisplayName("State")]
        public string State { get; set; }
        [DisplayName("PIN")]
        public string PinCode { get; set; }
        [DisplayName("Phone")]
        public string Phone { get; set; }
        [DisplayName("Mobile")]
        public string Mobile { get; set; }
        [DisplayName("E-mail")]
        public string Email1 { get; set; }
        [DisplayName("E-mail")]
        public string Email2 { get; set; }
        [DisplayName("Status")]
        public string Status { get; set; }
      
        public string ListCustomer { get; set; }
    }
}