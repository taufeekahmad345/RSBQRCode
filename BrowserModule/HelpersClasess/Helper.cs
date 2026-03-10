using DF_WebModule.DBModel;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;

namespace DF_WebModule.HelpersClasess
{
    public static class Helper
    {

            public static string ExtractNumber(string input)
        {
            input = input == null ? "" : input;
            Match match = Regex.Match(input, @"\d+");
            return match.Success ? match.Value : string.Empty;           
        }
    }
}