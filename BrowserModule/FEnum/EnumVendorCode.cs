using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DF_WebModule.FEnum
{
    public enum EnumVendorCode
    {
        [Display(Name = "7205761")]
        alwalw = 1,
        [Display(Name = "7201012")]
        alwpnr = 2,
        [Display(Name = "R64545")]
        tml = 3,
        [Display(Name = "RSB LKO")]
        rsblko = 4,
        [Display(Name = "7200868")]
        allh = 4
    }
}