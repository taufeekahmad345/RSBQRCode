using DF_WebModule.DBModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace DF_WebModule.HelpersClasess
{
    public static class QRCodeHelper
    {
        public static List<PrintandScanQRData> TMLQRCode(PartDetail partdetails, Customer customerdetails, int srno, List<PrintandScanQRData> printandScanQRDatalist, out string zpltext, out string qrtext, DateTime? date = null)
        {
            StringBuilder zplBuilder = new StringBuilder();
            if (!date.HasValue)
            {
                date = DateTime.Now;

            }
            
            var firstdata = $"00{partdetails.PartNumber}{partdetails.RevNo}{customerdetails.CustomerId}{date:MMyy}{srno:D6}";

            var textdata = $"{partdetails.Part_Description}  {partdetails.TypeOfPart} JT {partdetails.Joint} L {partdetails.Tube_Length}";

            if (!string.IsNullOrEmpty(partdetails.TypeOfPart) &&
               (partdetails.TypeOfPart.Trim().ToLower().Equals("dumb") || partdetails.TypeOfPart.Trim().ToLower().Equals("utz")))
            {
                zplBuilder.AppendLine("^XA");
                zplBuilder.AppendLine("^MD25");
                zplBuilder.AppendLine("^FO50,50");           // Label width: 70mm @ 203dpi
                zplBuilder.AppendLine("^BXN,5,200");           // Label height: 30mm @ 203dpi
                zplBuilder.AppendLine($"^FD{firstdata}^FS");

                // === Text (Right of QR code with 5px margin) ===
                zplBuilder.AppendLine("^FO180,70");        // Position text to the right of QR code with 5px margin
                zplBuilder.AppendLine("^A0N,30,30");      // Smaller, readable font (adjust if necessary)                        
                zplBuilder.AppendLine($"^FD{firstdata}^FS");

                zplBuilder.AppendLine("^FO180,100");        // Position text to the right of QR code with 5px margin
                zplBuilder.AppendLine("^A0,N,26,26");      // Smaller, readable font (adjust if necessary)                        
                zplBuilder.AppendLine($"^FD{textdata}^FS");
                zplBuilder.AppendLine("^XZ");
                printandScanQRDatalist.Add(new PrintandScanQRData
                {
                    IsScaned = false,
                    PrintingTime = DateTime.Now,
                    PartNumber = partdetails.PartNumber,
                    QrText = firstdata
                });

            }
            zplBuilder.AppendLine("^XA");
            zplBuilder.AppendLine("^MD25");
            zplBuilder.AppendLine("^FO50,50");           // Label width: 70mm @ 203dpi
            zplBuilder.AppendLine("^BXN,5,200");           // Label height: 30mm @ 203dpi
            zplBuilder.AppendLine($"^FD{firstdata}^FS");
            // === Text (Right of QR code with 5px margin) ===
            zplBuilder.AppendLine("^FO180,70");        // Position text to the right of QR code with 5px margin
            zplBuilder.AppendLine("^A0N,30,30");      // Smaller, readable font (adjust if necessary)                        
            zplBuilder.AppendLine($"^FD{firstdata}^FS");

            zplBuilder.AppendLine("^FO180,100");        // Position text to the right of QR code with 5px margin
            zplBuilder.AppendLine("^A0,N,26,26");      // Smaller, readable font (adjust if necessary)                        
            zplBuilder.AppendLine($"^FD{textdata}^FS");
            zplBuilder.AppendLine("^XZ");
            printandScanQRDatalist.Add(new PrintandScanQRData
            {
                IsScaned = false,
                PrintingTime = DateTime.Now,
                PartNumber = partdetails.PartNumber,
                QrText = firstdata
            });
            zpltext = zplBuilder.ToString();
            qrtext = firstdata;

            return printandScanQRDatalist;
        }

        public static List<PrintandScanQRData> BuildZPL_QRCode(PartDetail partdetails, Customer customerdetails, int srno, List<PrintandScanQRData> printandScanQRDatalist, out string zpltext, out string qrtext, DateTime? date = null)
        {
            StringBuilder zplBuilder = new StringBuilder();
            if (!date.HasValue)
            {
                date = DateTime.Now;

            }
            var firstdata = $"{partdetails.PartNumber}Rev No{partdetails.RevNo} {customerdetails.CustomerId}{date:MMyy}{srno:D6} {customerdetails.Name}";

            var textdata = $"{partdetails.Part_Description} {partdetails.TypeOfPart} JT {partdetails.Joint} L {partdetails.Tube_Length}";

            var qrdata = $"{partdetails.PartNumber}Rev No{partdetails.RevNo} {customerdetails.CustomerId}{DateTime.Now:MMyy}{srno:D6}";

            if (!string.IsNullOrEmpty(partdetails.TypeOfPart) &&
                (partdetails.TypeOfPart.Trim().ToLower().Equals("dumb") || partdetails.TypeOfPart.Trim().ToLower().Equals("utz")))
            {
                zplBuilder.AppendLine("^XA");
                zplBuilder.AppendLine("^MD27");
                zplBuilder.AppendLine("^FO40,50");           // Label width: 70mm @ 203dpi
                zplBuilder.AppendLine("^BXN,5,200");           // Label height: 30mm @ 203dpi
                zplBuilder.AppendLine($"^FD{qrdata}^FS");  // Load the QR Code data

                // === Text (Right of QR code with 5px margin) ===
                zplBuilder.AppendLine("^FO170,70");        // Position text to the right of QR code with 5px margin
                zplBuilder.AppendLine("^A0N,30,30");      // Smaller, readable font (adjust if necessary)                        
                zplBuilder.AppendLine($"^FD{firstdata}^FS");

                zplBuilder.AppendLine("^FO170,100");        // Position text to the right of QR code with 5px margin
                zplBuilder.AppendLine("^A0,N,26,26");      // Smaller, readable font (adjust if necessary)                        
                zplBuilder.AppendLine($"^FD{textdata}^FS");
                zplBuilder.AppendLine("^XZ");

                printandScanQRDatalist.Add(new PrintandScanQRData
                {
                    IsScaned = false,
                    PrintingTime = DateTime.Now,
                    PartNumber = partdetails.PartNumber,
                    QrText = qrdata
                });
            }
            zplBuilder.AppendLine("^XA");
            zplBuilder.AppendLine("^MD27");
            zplBuilder.AppendLine("^FO40,50");           // Label width: 70mm @ 203dpi
            zplBuilder.AppendLine("^BXN,5,200");           // Label height: 30mm @ 203dpi
            zplBuilder.AppendLine($"^FD{qrdata}^FS");  // Load the QR Code data

            // === Text (Right of QR code with 5px margin) ===
            zplBuilder.AppendLine("^FO170,70");        // Position text to the right of QR code with 5px margin
            zplBuilder.AppendLine("^A0N,30,30");      // Smaller, readable font (adjust if necessary)                        
            zplBuilder.AppendLine($"^FD{firstdata}^FS");

            zplBuilder.AppendLine("^FO170,100");        // Position text to the right of QR code with 5px margin
            zplBuilder.AppendLine("^A0,N,26,26");      // Smaller, readable font (adjust if necessary)                        
            zplBuilder.AppendLine($"^FD{textdata}^FS");
            zplBuilder.AppendLine("^XZ");
            printandScanQRDatalist.Add(new PrintandScanQRData
            {
                IsScaned = false,
                PrintingTime = DateTime.Now,
                PartNumber = partdetails.PartNumber,
                QrText = qrdata
            });
            zpltext = zplBuilder.ToString();
            qrtext = qrdata;
            return printandScanQRDatalist;
        }

        public static List<PrintandScanQRData> BuildZPL_Barcode(PartDetail partdetails, Customer customerdetails, int srno, List<PrintandScanQRData> printandScanQRDatalist, out string zpltext, out string qrtext, DateTime? date = null)
        {
            StringBuilder zplBuilder = new StringBuilder();
            if (!date.HasValue)
            {
                date = DateTime.Now;

            }
            var qrdata = $"00{partdetails.PartNumber}{partdetails.RevNo}{customerdetails.CustomerId}{date:MMyy}{srno:D6}";
            var textdata = $"{partdetails.Part_Description} {partdetails.Joint} {partdetails.TypeOfPart} {partdetails.Tube_Length}";
            if (!string.IsNullOrEmpty(partdetails.TypeOfPart) &&
               (partdetails.TypeOfPart.Trim().ToLower().Equals("dumb") || partdetails.TypeOfPart.Trim().ToLower().Equals("utz")))
            {
                zplBuilder.AppendLine("^XA");
                zplBuilder.AppendLine("^MNY");
                zplBuilder.AppendLine("^JC");
                zplBuilder.AppendLine("^LH0,0");
                zplBuilder.AppendLine("^PW812");
                zplBuilder.AppendLine("^FO50,40");
                zplBuilder.AppendLine("^BY2,2,65");
                zplBuilder.AppendLine("^A0N,30,38");
                zplBuilder.AppendLine("^BCN,65,Y,N,N");
                zplBuilder.AppendLine($"^FD>;00{partdetails.PartNumber}>6{partdetails.RevNo}{customerdetails.CustomerId.Substring(0, 2)}>;{customerdetails.CustomerId.Substring(2)}{DateTime.Now:MMyy}{srno:D6}^FS");
                zplBuilder.AppendLine("^FO50,160");   // Below Text 
                zplBuilder.AppendLine("^A0N,30,38");
                zplBuilder.AppendLine($"^FD{textdata}^FS");
                zplBuilder.AppendLine("^XZ");
                printandScanQRDatalist.Add(new PrintandScanQRData
                {
                    IsScaned = false,
                    PrintingTime = DateTime.Now,
                    PartNumber = partdetails.PartNumber,
                    QrText = qrdata
                });
            }
            zplBuilder.AppendLine("^XA");
            zplBuilder.AppendLine("^MNY");
            zplBuilder.AppendLine("^JC");
            zplBuilder.AppendLine("^LH0,0");
            zplBuilder.AppendLine("^PW812");
            zplBuilder.AppendLine("^FO50,40");
            zplBuilder.AppendLine("^BY2,2,65");
            zplBuilder.AppendLine("^A0N,30,38");
            zplBuilder.AppendLine("^BCN,65,Y,N,N");
            zplBuilder.AppendLine($"^FD>;00{partdetails.PartNumber}>6{partdetails.RevNo}{customerdetails.CustomerId.Substring(0, 2)}>;{customerdetails.CustomerId.Substring(2)}{DateTime.Now:MMyy}{srno:D6}^FS");
            zplBuilder.AppendLine("^FO50,160");   // Below Text 
            zplBuilder.AppendLine("^A0N,30,38");
            zplBuilder.AppendLine($"^FD{textdata}^FS");
            zplBuilder.AppendLine("^XZ");
            printandScanQRDatalist.Add(new PrintandScanQRData
            {
                IsScaned = false,
                PrintingTime = DateTime.Now,
                PartNumber = partdetails.PartNumber,
                QrText = qrdata
            });
            zpltext = zplBuilder.ToString();
            qrtext = qrdata;
            return printandScanQRDatalist;
        }
        public static List<PrintandScanQRData> BuildZPL_74charQRCode(PartDetail partdetails, Customer customerdetails, int srno, List<PrintandScanQRData> printandScanQRDatalist, out string zpltext, out string qrtext, DateTime? date = null)
        {
            try
            {               
                StringBuilder zplBuilder = new StringBuilder();
                if (!date.HasValue)
                {
                    date = DateTime.Now;
                
                }
                //string filePath = $"{ConfigurationHelper.GetConfigValue(ConfigurationHelper.QRData)}lebel.txt";
                //string newtext = System.IO.File.ReadAllText(filePath);
                var qrdata = $"{customerdetails.CustomerId}${partdetails.PartNumber}${srno:D6}${date.Value.ToString("dd.MM.yyyy")}$NA$NA$NA${partdetails.RevNo}$ASSY.PROP.SHAFT$RSBTRANSMISSION$";
                //newtext = newtext.ToString().Replace("#qrdata#", qrdata);
                if (!string.IsNullOrEmpty(partdetails.TypeOfPart) &&
                       (partdetails.TypeOfPart.Trim().ToLower().Equals("dumb") || partdetails.TypeOfPart.Trim().ToLower().Equals("utz")))
                {
                    zplBuilder.AppendLine("^XA");
                    zplBuilder.AppendLine("^LL600");
                    zplBuilder.AppendLine("^PW240");
                    zplBuilder.AppendLine("^MMT");
                    zplBuilder.AppendLine("^MTT");
                    zplBuilder.AppendLine("^LH0,0");
                    zplBuilder.AppendLine("^LT0");
                    zplBuilder.AppendLine("^MD0");

                    zplBuilder.AppendLine("^FO50,50");
                    zplBuilder.AppendLine("^BQN,2,4");
                    zplBuilder.AppendLine($"^FDLA,{qrdata}^FS");

                    zplBuilder.AppendLine("^FO17,200");
                    zplBuilder.AppendLine("^A0N,14,14");
                    zplBuilder.AppendLine("^FB220,3,0,C");
                    zplBuilder.AppendLine($"^FD{qrdata}^FS");

                    zplBuilder.AppendLine("^FO50,300");
                    zplBuilder.AppendLine("^BQN,2,4");
                    zplBuilder.AppendLine($"^FDLA,{qrdata}^FS");

                    zplBuilder.AppendLine("^FO17,450");
                    zplBuilder.AppendLine("^A0N,14,14");
                    zplBuilder.AppendLine("^FB220,3,0,C");
                    zplBuilder.AppendLine($"^FD{qrdata}^FS");

                    zplBuilder.AppendLine("^XZ");




                    printandScanQRDatalist.Add(new PrintandScanQRData
                    {
                        IsScaned = false,
                        PrintingTime = DateTime.Now,
                        PartNumber = partdetails.PartNumber,
                        QrText = qrdata
                    });

                }
                zplBuilder.AppendLine("^XA");
                zplBuilder.AppendLine("^LL600");
                zplBuilder.AppendLine("^PW240");
                zplBuilder.AppendLine("^MMT");
                zplBuilder.AppendLine("^MTT");
                zplBuilder.AppendLine("^LH0,0");
                zplBuilder.AppendLine("^LT0");
                zplBuilder.AppendLine("^MD0");

                zplBuilder.AppendLine("^FO50,50");
                zplBuilder.AppendLine("^BQN,2,4");
                zplBuilder.AppendLine($"^FDLA,{qrdata}^FS");

                zplBuilder.AppendLine("^FO17,200");
                zplBuilder.AppendLine("^A0N,16,16");
                zplBuilder.AppendLine("^FB220,3,0,C");
                zplBuilder.AppendLine($"^FD{qrdata}^FS");

                zplBuilder.AppendLine("^FO50,300");
                zplBuilder.AppendLine("^BQN,2,4");
                zplBuilder.AppendLine($"^FDLA,{qrdata}^FS");

                zplBuilder.AppendLine("^FO17,450");
                zplBuilder.AppendLine("^A0N,16,16");
                zplBuilder.AppendLine("^FB220,3,0,C");
                zplBuilder.AppendLine($"^FD{qrdata}^FS");

                zplBuilder.AppendLine("^XZ");

                printandScanQRDatalist.Add(new PrintandScanQRData
                {
                    IsScaned = false,
                    PrintingTime = DateTime.Now,
                    PartNumber = partdetails.PartNumber,
                    QrText = qrdata
                });
                zpltext = zplBuilder.ToString();
                qrtext = qrdata;

                return printandScanQRDatalist;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {

            }

        }
        // Barcode
        //zplBuilder.AppendLine("^XA");
        //zplBuilder.AppendLine("^FO50,40^BY2");
        //zplBuilder.AppendLine("^A0N,30,38");
        //zplBuilder.AppendLine("^BCN,65,Y,N,N");
        //zplBuilder.AppendLine($"^FD>;00{partdetails.PartNumber}>6{partdetails.RevNo}{customerdetails.CustomerId.Substring(0, 2)}>;{customerdetails.CustomerId.Substring(2)}{DateTime.Now:MMyy}{srno:D6}^FS");
        //zplBuilder.AppendLine("^FO50,160");   // Below Text 
        //zplBuilder.AppendLine("^A0N,30,38");
        //zplBuilder.AppendLine($"^FD{textdata}^FS");
        //zplBuilder.AppendLine("^XZ");
    }
}