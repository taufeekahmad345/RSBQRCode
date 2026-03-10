using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DF_WebModule.FEnum
{
public enum EnumCustomer
    {
    [Display(Name = "ALL ALW")]
    alwalw = 1,
    [Display(Name = "ALL PNR")]
    alwpnr = 2,
    [Display(Name = "TML")]
    tml =3,
    [Display(Name = "RSB LKO")]
    rsblko = 4
    }
}