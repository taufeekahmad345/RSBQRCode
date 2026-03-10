using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace DF_WebModule.Classes
{
    public class clsPartDetail
    {
        public long Id { get; set; }
        [DisplayName("Customer Name")]
        public string CustomerName { get; set; }
        [DisplayName("Vendor")]
        public string Vendorcode { get; set; }
        [DisplayName("Part NO.")]
        public string PartNumber { get; set; }
        [DisplayName("Description")]
        public string Part_Description { get; set; }
        [DisplayName("JT")]
        public string Joint { get; set; }
        [DisplayName("Length")]
        public string Tube_Length { get; set; }
        [DisplayName("Type")]
        public string TypeOfPart { get; set; }
        [DisplayName("RevLvl")]
        public string ModNo { get; set; }
        [DisplayName("Remark")]
        public string Remark { get; set; }
        [DisplayName("SR NO.")]
        public string SRNo { get; set; }
        [DisplayName("Customer Name")]
        public string PartList { get; set; }
        public List<Type> types { get; set; }
        public List<Customer> customers { get; set; }
        

    }
    public class Type
    {
        public int Id { get; set; }
        public string Name { get; set; }
    
    }
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }

    }
}