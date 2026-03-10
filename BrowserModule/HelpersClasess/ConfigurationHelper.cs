using CsvHelper;
using DF_WebModule.Classes;
using DF_WebModule.Classes.Constant;
using DF_WebModule.FEnum;
using DF_WebModule.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;

using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Xml;
using System.IO;
using System.Xml.Linq;
using System.Xml.Serialization;
using CsvHelper.Configuration;
using System.Security.Cryptography;

namespace DF_WebModule.HelpersClasess
{
    public static class ConfigurationHelper
    {
        public static string Cypherkey = "hlckXL80xFLg3TpmGYS9";
        public const string License = "Demo";
        public const string Filepath = "Filepath";
        public const string QRData = "QRData";
        public const string Testing = "Testing";
        public const string BarcodePrinter = "BarcodePrinter";
        public const string QrPrinter = "QrPrinter";
      
        public static string Encrypt(string data, string key = "")
        {

            if (key.Trim().Length == 0) key = Cypherkey;
            RijndaelManaged rijndaelCipher = new RijndaelManaged();
            rijndaelCipher.Mode = CipherMode.CBC; //remember this parameter
            rijndaelCipher.Padding = PaddingMode.PKCS7; //remember this parameter

            rijndaelCipher.KeySize = 0x80;
            rijndaelCipher.BlockSize = 0x80;
            byte[] pwdBytes = Encoding.UTF8.GetBytes(key);
            byte[] keyBytes = new byte[0x10];
            int len = pwdBytes.Length;

            if (len > keyBytes.Length)
            {
                len = keyBytes.Length;
            }

            Array.Copy(pwdBytes, keyBytes, len);
            rijndaelCipher.Key = keyBytes;
            rijndaelCipher.IV = keyBytes;
            ICryptoTransform transform = rijndaelCipher.CreateEncryptor();
            byte[] plainText = Encoding.UTF8.GetBytes(data);

            return Convert.ToBase64String
            (transform.TransformFinalBlock(plainText, 0, plainText.Length));
        }

        public static string Decrypt(string data, string key = "")
        {
            if (key.Trim().Length == 0) key = Cypherkey;

            RijndaelManaged rijndaelCipher = new RijndaelManaged();
            rijndaelCipher.Mode = CipherMode.CBC;
            rijndaelCipher.Padding = PaddingMode.PKCS7;

            rijndaelCipher.KeySize = 0x80;
            rijndaelCipher.BlockSize = 0x80;
            byte[] encryptedData = Convert.FromBase64String(data);
            byte[] pwdBytes = Encoding.UTF8.GetBytes(key);
            byte[] keyBytes = new byte[0x10];
            int len = pwdBytes.Length;

            if (len > keyBytes.Length)
            {
                len = keyBytes.Length;
            }

            Array.Copy(pwdBytes, keyBytes, len);
            rijndaelCipher.Key = keyBytes;
            rijndaelCipher.IV = keyBytes;
            byte[] plainText = rijndaelCipher.CreateDecryptor().TransformFinalBlock
                        (encryptedData, 0, encryptedData.Length);

            return Encoding.UTF8.GetString(plainText);
        }
        public static string GetFilename(string filepath)
        {
            string path = filepath;
            string[] pathArr = path.Split('\\');
            return pathArr.Last().ToString();
        }
        public static string GetConfigValue(string key)

        {
            if (!string.IsNullOrEmpty(key))
            {
                return ConfigurationManager.AppSettings[key].ToString();
            }
            return string.Empty;
        }
        public static DateTime? ConvertStringToDate(string date)
        {
            try
            {   
                DateTime declineDate = DateTime.ParseExact(date, new string[] {"MM/dd/yyyy hh:mm tt","MM/dd/yyyy h:mm tt","MM/dd/yyyy HH:mm tt","MM/dd/yyyy h:mm tt",
               "dd/MM/yyyy", "dd/MM/yyyy hh:mm","dd/MM/yyyy hh:mm:ss", "dd-MM-yyyy", "dd-MM-yyyy hh:mm","dd-MM-yyyy hh:mm:ss", "d/MM/yyyy", "d/MM/yyyy hh:mm", "d/M/yyyy", "d/M/yyyy hh:mm",
                "d-MM-yyyy", "d-MM-yyyy hh:mm", "d-M-yyyy", "d-M-yyyy hh:mm", "dd/MM/yyyy HH:mm", "dd/MM/yyyy HH:mm ss",
                "MM/dd/yyyy HH:mm", "MM/dd/yyyy HH:mm ss", "MM/dd/yyyy hh:mm tt","yyyy/MM/dd", "yyyy/MM/dd hh:mm", "yyyy/MM/dd hh:mm:ss","yyyy-MM-dd", "yyyy-MM-dd hh:mm", "yyyy-MM-dd hh:mm:ss", "dd.MM.yy"}, CultureInfo.InstalledUICulture, DateTimeStyles.None);
                return declineDate;
            }
            catch(Exception ex)
            {
                return null;
            }
        }
        public static bool IsValidExtension(string allowedExtension, string uploadedMedia)
        {
            var fileextention = GetFileExtension(uploadedMedia);
            if (allowedExtension.Contains(fileextention))
            {
                return true;
            }
            return false;
        }
        public static string GetFileExtension(string file)
        {
            string extension = string.Empty;
            try
            {
                string[] arr = file.Split('.');
                extension = "." + arr[arr.Length - 1].ToString();
                if (!extension.Equals(".jpg", StringComparison.OrdinalIgnoreCase)
                    && !extension.Equals(".jpeg", StringComparison.OrdinalIgnoreCase)
                    && !extension.Equals(".png", StringComparison.OrdinalIgnoreCase)
                    && !extension.Equals(".pdf", StringComparison.OrdinalIgnoreCase)
                    && !extension.Equals(".doc", StringComparison.OrdinalIgnoreCase)
                    && !extension.Equals(".docx", StringComparison.OrdinalIgnoreCase))
                {
                    return extension;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return extension;
        }
       


    }
}