using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Web;

namespace DF_WebModule.FEnum
{
    public enum EnumColumnName
    {
        [Display(Name = "Tube Length")]
        Tubelenth = 1,
        [Display(Name = "Tube Dia & Thickness")]
        Tube_Dia_Thickness = 2,
        [Display(Name = "Series")]
        Joint = 3,
        [Display(Name = "Part Type")]
        TypeOfPart = 4,
        [Display(Name = "Available Noise Deadener")]
        Noise_Deadener = 5,       
        [Display(Name = "Fep Press H. Stock Positions")]
        Fep_Pressing_H_Stock_Position = 6,
        [Display(Name = "Rear Housing length")]
        RH = 7,
        [Display(Name = "Long Fork Length")]
        LF = 8,
        [Display(Name = "S.F Details")]
        SF = 9,
        [Display(Name = "PDC Length")]
        PDC_Length = 10,            
        [Display(Name = "Front End Piece Details")]
        FEP = 11,
        [Display(Name = "Flange yoke Details")]
        Flange_yoke_Details = 12,
        [Display(Name = "Greaseable or Non Greasable")]
        GreaseableorNonGreasable = 13,
        [Display(Name = "Coupling Flange Details")]
        Coupling_Flange_Details = 14,
        [Display(Name = "Coupling Flange Orientation")]
        Coupling_Flange_Orientation = 15,
        [Display(Name = "CB Kit Details")]
        CB_Kit = 16,

        [Display(Name = "Loctite Grade Use")]
        Loctite = 17,
        [Display(Name = "Hex bolt/Hex nut Tightening")]
        Hex_Bolt_Hex_Nut_Tightening = 18,
        [Display(Name = "Balancing RPM")]
        RPM = 19,
        [Display(Name = "Unbalance Value CMG")]
        Unbalance_Value_in_CMG = 20,
        [Display(Name = "Unbalance Value GM")]
        Unbalance_Value_in_GM = 21,
        [Display(Name = "I/A Bellow Details")]
        Inter_Axle_Blow = 22,   
        [Display(Name ="Total Length")]
        TotalLength = 23,
        [Display(Name = "RearSlip")]
        RearSlip = 24,
        [Display(Name = "ModNo")]
        ModNo = 25,
        [Display(Name = "Vendor code")]
        Vendorcode = 26,       
        [Display(Name = "Customer Name")]
        CustomerName = 27,
        [Display(Name = "DWG weight")]
        DWGweight = 28
    }

    public static class EnumDescriptionExtension
    {
        public static string GetEnumDisplayName(Enum value)
        {
            // Ensure the value is not null
            if (value == null)
                throw new ArgumentNullException(nameof(value), "Enum value cannot be null.");

            // Get the FieldInfo for the enum value
            FieldInfo field = value.GetType().GetField(value.ToString());

            // Get the Display attribute for the enum value
            DisplayAttribute attribute = (DisplayAttribute)Attribute.GetCustomAttribute(field, typeof(DisplayAttribute));

            // Return the Name property of the Display attribute if it exists, otherwise return the enum value's string representation
            return attribute?.Name ?? value.ToString();
        }
        //public static string GetEnumDisplayName(string value)
        //{
        //    // Get the Display attribute for the enum value
        //    FieldInfo field = value.GetType().GetField(value.ToString());
        //    DisplayAttribute attribute = (DisplayAttribute)Attribute.GetCustomAttribute(field, typeof(DisplayAttribute));

        //    // Return the Name property of the Display attribute if it exists, otherwise return the enum value's string representation
        //    return attribute?.Name ?? value.ToString();
        //}
    }
}