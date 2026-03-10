using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DF_WebModule.Classes.Constant
{
    public static class ValidationRegex
    {
        public const string RegexName = @"^(?<FName>\w+\s(?:\w\.\s)?)(\b(?<MName>.+)?)\b(?<LName>\w+)$";
        public const string AllowedfileExtension = ".jpeg, .jpg, .png, .svg, .pdf, .doc, .docx, .PDF, .JPEG, .JPG, .PNG, .SVG, .DOC, DOCX";
       
    }
}