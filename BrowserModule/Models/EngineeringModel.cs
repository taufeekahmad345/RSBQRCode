using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using DF_WebModule.Classes;

namespace DF_WebModule.Models
{
    public class EngineeringModel
    {
        public long Id { get; set; }
       
        [DisplayName("Customer Name")]
        public string CustomerName { get; set; }
        [DisplayName("Vendor Code")]
        public string Vendorcode { get; set; }
        [Required]
        [DisplayName("Part No.")]
        public string PartNumber { get; set; }
        [DisplayName("Part Description")]
        public string Part_Description { get; set; }
        [DisplayName("Series")]
        public string Joint { get; set; }
        [DisplayName("Tube Length")]
        public string Tube_Length { get; set; }
        [DisplayName("Part Type")]
        public string TypeOfPart { get; set; }
        [DisplayName("Rev No")]
        public string ModNo { get; set; }
        public string Remark { get; set; }
        public string SRNo { get; set; }
       

    }
}