
using DF_WebModule.Classes;
using DF_WebModule.DBModel;
using SkiaSharp;
using System;
using DF_WebModule.Models;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using DF_WebModule.HelpersClasess;
using System.Web.Mvc;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.IO;
using OfficeOpenXml;
using Customer = DF_WebModule.DBModel.Customer;

namespace DF_WebModule.Controllers
{
    public class PrintController : Controller
    {
        protected SymlLogs _log;      
        public PrintController()
        {
            _log = new SymlLogs(this.GetType().Name);
           
        }
        Engineering_DBEntities entity = new Engineering_DBEntities();

        public ActionResult Index()
        {
            clsPrint clsPrint = new clsPrint();
            try
            {
                _log.LogDebugMessage($"{nameof(this.Index)}, {LoggerMessage.ProcessStarted}, {DateTime.Now}");


                List<Classes.Customer> customers = new List<Classes.Customer>();
                List<clsPartDetail> clsPartDetails = new List<clsPartDetail>();
                List<clsPartDetail> type = new List<clsPartDetail>();
                var customer = entity.Customers.Where(x => x.IsActive.HasValue && x.IsActive.Value).ToList();
                var PartDetails = entity.PartDetails.Where(x => x.IsActive).ToList();

                foreach (var item in customer)
                {
                    customers.Add(new Classes.Customer
                    {
                        Id = item.Id,
                        Name = item.Name
                    });
                }
                foreach (var pitem in PartDetails)
                {
                    clsPartDetails.Add(new Classes.clsPartDetail
                    {
                        Id = pitem.Id,
                        PartNumber = pitem.PartNumber
                    });
                }
                var id = 1;
                foreach (var titem in PartDetails.Where(x => !string.IsNullOrEmpty(x.TypeOfPart)).GroupBy(x => x.TypeOfPart).ToList())
                {
                    type.Add(new Classes.clsPartDetail
                    {
                        Id = id,
                        TypeOfPart = titem.Key
                    });
                    id++;
                }

                clsPrint.parttype = type;

                clsPrint.customers = customers;

                clsPrint.clsPartDetails = clsPartDetails;
                clsPrint.Printers = PrinterSettings.InstalledPrinters.Cast<string>().ToList().Where(x => !string.IsNullOrEmpty(x) && !x.ToLower().Contains("anydesk") && !x.ToLower().Contains("microsoft") && !x.ToLower().Contains("fax") && !x.ToLower().Contains("onenote")).ToList();
                clsPrint.Date = DateTime.Now.ToString("dd/MM/yyyy");
                var partnocount = entity.PartQRCounts.ToList().FirstOrDefault(x => x.PartDate.Month == DateTime.Now.Date.Month && x.PartDate.Year == DateTime.Now.Date.Year);
                clsPrint.SLNO = "000000";
                if (partnocount != null)
                {
                    clsPrint.SLNO = partnocount.Count.ToString("D6");
                }
            }
            catch (Exception ex)
            {
                _log.LogError($"Error Thrown during Edititem: {ex.Message}, { nameof(this.Index)}, {DateTime.Now},{ex.StackTrace}");


            }
            finally
            {
                _log.LogDebugMessage($"{nameof(this.Index)}, {LoggerMessage.ProcessEnded}, {DateTime.Now}");

            }
            return View(clsPrint);
        }

        public ActionResult Scan()
        {

            return View();

        }
        public ActionResult Export()
        {
            return View();
        }
        public ActionResult GetScanData(string qrtext)
        {
            try
            {
                var scandata = entity.PrintandScanQRDatas.FirstOrDefault(x => x.QrText.Equals(qrtext));
                if (scandata != null)
                {
                    var item = entity.PartDetails.FirstOrDefault(x => x.PartNumber.Equals(scandata.PartNumber));
                    return Json(new
                    {
                        success = true,
                        des = item.Part_Description,
                        rev = item.RevNo,
                        joint = item.Joint,
                        customer = item.CustomerName,
                        code = item.VendorCode,
                        type = item.TypeOfPart,
                        part = item.PartNumber
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = false, responseText = "No Data found." }, JsonRequestBehavior.AllowGet);
                }

            }
            catch
            {
                return Json(new { success = false, responseText = "Something wrong please try again." }, JsonRequestBehavior.AllowGet);

            }
            finally
            {
            }

        }
        public ActionResult Getpartdetailbyscan(string part)
        {
            try
            {
                _log.LogDebugMessage($"{nameof(this.Getpartdetailbyscan)}, {LoggerMessage.ProcessStarted}, {DateTime.Now}");

                var item = entity.PartDetails.ToList().Where(x => !string.IsNullOrEmpty(x.PartNumber) && part.Trim().ToLower().Contains(x.PartNumber.Trim().ToLower())).FirstOrDefault();
                if (item != null)
                {

                    return Json(new
                    {
                        success = true,
                        des = item.Part_Description,
                        rev = item.RevNo,
                        joint = item.Joint,
                        length = item.Tube_Length,
                        code = item.VendorCode,
                        type = item.TypeOfPart,
                        part = item.PartNumber
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = false, responseText = "No Data found." }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                _log.LogError($"Error Thrown during Edititem: {ex.Message}, { nameof(this.Getpartdetailbyscan)}, {DateTime.Now},{ex.StackTrace}");


                return null;
            }
            finally
            {
                _log.LogDebugMessage($"{nameof(this.Getpartdetailbyscan)}, {LoggerMessage.ProcessEnded}, {DateTime.Now}");

            }
        }
        public ActionResult Getpartdetail(string part)
        {
            try
            {
                _log.LogDebugMessage($"{nameof(this.Getpartdetail)}, {LoggerMessage.ProcessStarted}, {DateTime.Now}");

                var item = entity.PartDetails.ToList().Where(x => x.PartNumber.Equals(part)).FirstOrDefault();
                if (item != null)
                {

                    return Json(new
                    {
                        success = true,
                        des = item.Part_Description,
                        rev = item.RevNo,
                        joint = item.Joint,
                        length = item.Tube_Length,
                        code = item.VendorCode,
                        type = item.TypeOfPart,
                        part = item.PartNumber
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = false, responseText = "No Data found." }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                _log.LogError($"Error Thrown during Edititem: {ex.Message}, { nameof(this.Getpartdetail)}, {DateTime.Now},{ex.StackTrace}");


                return null;
            }
            finally
            {
                _log.LogDebugMessage($"{nameof(this.Getpartdetail)}, {LoggerMessage.ProcessEnded}, {DateTime.Now}");

            }
        }

        public ActionResult Indexx()
        {
            var printers = PrinterSettings.InstalledPrinters.Cast<string>().ToList();
            ViewBag.Printers = new SelectList(printers);
            return View();
        }
        [HttpPost]
        public ActionResult DubPrintQRCode(string partnumber, int printetype, int slno, DateTime dubdate, string Customer)
        {
            try
            {
                _log.LogDebugMessage($"{nameof(this.DubPrintQRCode)}, {LoggerMessage.ProcessStarted}, {DateTime.Now}");
                BarcodePrinter barcodePrinter = new BarcodePrinter();
                var printList = new List<PrintandScanQRData>();
                string zpltext = null;               
                string qrtext = null;
                _log.LogInfoMessage($"{nameof(this.DubPrintQRCode)} {Session["Role"]}, {DateTime.Now}");
                _log.LogInfoMessage($"{nameof(this.DubPrintQRCode)} SR No :{slno} - PartNUmber {partnumber} - Customer : {Customer} Datetime : {dubdate}, {DateTime.Now}");

                if (Convert.ToString(Session["Role"]) == "2")
                {
                    var printDocument = new PrintDocument();                   
                    //var customerdetails = entity.Customers.ToList().FirstOrDefault(x => !string.IsNullOrEmpty(x.Name) && x.Name.Trim().Equals(customer.Trim()));
                    var partdetails = entity.PartDetails.ToList().FirstOrDefault(x => !string.IsNullOrEmpty(x.PartNumber) && x.PartNumber.Trim().Equals(partnumber.Trim()) && x.CustomerName.Trim().Contains(Customer.Trim()));
                    if (partdetails != null)
                    {
                        var customerdetails = entity.Customers.ToList().FirstOrDefault(x => !string.IsNullOrEmpty(x.Name) && x.Name.Trim().Equals(Customer.Trim()));
                        if (customerdetails != null)
                        {
                            var scanneddata = entity.PrintandScanQRDatas.Where(x => x.QrText != null && x.QrText.Contains(partnumber) && x.QrText.Contains(slno.ToString())).FirstOrDefault();
                            if (scanneddata != null)
                            {
                                if (printetype == (int)FEnum.QRCodeType.SQRCoder)
                                {
                                    QRCodeHelper.BuildZPL_74charQRCode(partdetails, customerdetails, slno, printList, out zpltext, out qrtext, dubdate);
                                    var qrprinter = ConfigurationHelper.GetConfigValue(ConfigurationHelper.QrPrinter);
                                    barcodePrinter.SendStringToPrinter(qrprinter, zpltext, FEnum.EnumScanMothod.DublicatePrint.ToString());

                                }
                                else if (printetype == (int)FEnum.QRCodeType.LQRCode)
                                {
                                    QRCodeHelper.BuildZPL_QRCode(partdetails, customerdetails, slno, printList, out zpltext, out qrtext, dubdate);

                                    var barcodeprinter = ConfigurationHelper.GetConfigValue(ConfigurationHelper.BarcodePrinter);
                                    barcodePrinter.SendStringToPrinter(barcodeprinter, zpltext, FEnum.EnumScanMothod.DublicatePrint.ToString());
                                }
                                else if (printetype == (int)FEnum.QRCodeType.BarCode)
                                {
                                    QRCodeHelper.BuildZPL_Barcode(partdetails, customerdetails, slno, printList, out zpltext, out qrtext, dubdate);
                                    var barcodeprinter = ConfigurationHelper.GetConfigValue(ConfigurationHelper.BarcodePrinter);
                                    barcodePrinter.SendStringToPrinter(barcodeprinter, zpltext, FEnum.EnumScanMothod.DublicatePrint.ToString());
                                }
                                else if (printetype == (int)FEnum.QRCodeType.TMLQR)
                                {
                                    QRCodeHelper.TMLQRCode(partdetails, customerdetails, slno, printList, out zpltext, out qrtext, dubdate);
                                    var barcodeprinter = ConfigurationHelper.GetConfigValue(ConfigurationHelper.BarcodePrinter);
                                    barcodePrinter.SendStringToPrinter(barcodeprinter, zpltext, FEnum.EnumScanMothod.DublicatePrint.ToString());
                                }
                                else
                                {
                                    return Json(new { success = false, responseText = "Please select QR Type" }, JsonRequestBehavior.AllowGet);
                                }
                            }
                            else
                            {
                                return Json(new { success = false, responseText = "Dublicate scanned not avilable for this part" }, JsonRequestBehavior.AllowGet);
                            }
                        }
                        else
                        {
                            return Json(new { success = false, responseText = "Customer not found" }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        return Json(new { success = false, responseText = "Part number Invalid or not found for this customer" }, JsonRequestBehavior.AllowGet);
                    }
                    _log.LogInfoMessage($"{nameof(this.DubPrintQRCode)} Full Text :{zpltext}, {DateTime.Now}");

                    return Json(new { success = true, responseText = "Printed successfully!" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = false, responseText = "Dublicate QR Only print by Admin" }, JsonRequestBehavior.AllowGet);

                }



            }
            catch (Exception ex)
            {
                _log.LogError($"Error Thrown during Edititem: {ex.Message}, { nameof(this.DubPrintQRCode)}, {DateTime.Now},{ex.StackTrace}");



                return Json(new { success = false, responseText = "Something wrong! please connect IT Team" }, JsonRequestBehavior.AllowGet);

            }
            finally
            {
                _log.LogDebugMessage($"{nameof(this.DubPrintQRCode)}, {LoggerMessage.ProcessEnded}, {DateTime.Now}");

            }

        }

        [HttpPost]
        public ActionResult PrintQRCode(PrintRequest item)
        {

            try
            {
                _log.LogDebugMessage($"{nameof(this.PrintQRCode)}, {LoggerMessage.ProcessStarted}, {DateTime.Now}");
               
                var srno = 25001;
                PartQRCount sr = new PartQRCount();
                if (string.IsNullOrEmpty(item.PartNumber))
                {
                    return Json(new { success = false, responseText = "Please provide the PartNumber" }, JsonRequestBehavior.AllowGet);
                }
                bool isscaned = false;
                   if (item.printcount == 0)
                    {
                        return Json(new { success = false, responseText = "Please provide the PrintCount" }, JsonRequestBehavior.AllowGet);

                    }
                    sr = entity.PartQRCounts.ToList().FirstOrDefault(x => x.PartDate.Date.Month == DateTime.Now.Date.Month && x.PartDate.Date.Year == DateTime.Now.Date.Year);
                    if (sr != null)
                    {
                        srno = sr.Count;
                    }
                if (string.IsNullOrEmpty(item.Printer))
                {
                    return Json(new { success = false, responseText = "Please select The Printer" }, JsonRequestBehavior.AllowGet);

                }
                var qrdata = "";                
                var startcount = srno;
                var printDocument = new PrintDocument();
                BarcodePrinter barcodePrinter = new BarcodePrinter();
                int totalPrintCount = item.printcount; // This is the number of QR codes you want to print                
                var partdetails = entity.PartDetails.ToList().FirstOrDefault(x => !string.IsNullOrEmpty(x.PartNumber) && x.PartNumber.Trim().Equals(item.PartNumber.Trim()) && x.CustomerName.Trim().Contains(item.Customer.Trim()));
                List<PrintandScanQRData> printandScanQRDatalist = new List<PrintandScanQRData>();
                _log.LogInfoMessage($"TypeOfPart : {partdetails?.TypeOfPart}");
                _log.LogInfoMessage($"Printer Name : {item?.Printer}");
                string zpltext = null;
                string qrtext = null;
                StringBuilder zplBuilder = new StringBuilder();
                if (partdetails != null)
                {
                   var customerdetails = entity.Customers.ToList().FirstOrDefault(x => !string.IsNullOrEmpty(x.Name) && x.Name.Trim().Equals(item.Customer.Trim()));
                    if (customerdetails != null)
                    {
                        if (item.PrintType == (int)FEnum.QRCodeType.LQRCode)
                        {
                            for (int i = 1; i <= item.printcount; i++)
                            {
                                printandScanQRDatalist = QRCodeHelper.BuildZPL_QRCode(partdetails, customerdetails, srno, printandScanQRDatalist, out zpltext, out qrtext);
                                zplBuilder.AppendLine(zpltext);
                                var printedqr = entity.PrintandScanQRDatas.FirstOrDefault(x => x.QrText.Equals(qrdata));
                                if (printedqr != null)
                                {
                                    return Json(new { success = false, responseText = "This part already printed! can not print dublicate" }, JsonRequestBehavior.AllowGet);
                                }
                                srno++;
                            }
                            string fullZPL = zplBuilder.ToString();
                            barcodePrinter.SendStringToPrinter(item.Printer, fullZPL, FEnum.EnumScanMothod.NormalPrint.ToString());
                            DublicateSRNumber dublicateSRNumber = new DublicateSRNumber
                            {
                                Count = item.printcount,
                                StartSRNo = startcount,
                                Customer = customerdetails.CustomerId,
                                Date = DateTime.Now,
                                PartNo = item.PartNumber,
                                Status = 1,
                                RevNo = partdetails.RevNo
                            };
                            entity.DublicateSRNumbers.Add(dublicateSRNumber);
                            entity.SaveChanges();
                        }
                        else if (item.PrintType == (int)FEnum.QRCodeType.BarCode)
                        {
                            for (int i = 1; i <= item.printcount; i++)
                            {
                                printandScanQRDatalist = QRCodeHelper.BuildZPL_Barcode(partdetails, customerdetails, srno, printandScanQRDatalist, out zpltext, out qrtext);
                                zplBuilder.AppendLine(zpltext);
                                var printedqr = entity.PrintandScanQRDatas.FirstOrDefault(x => x.QrText.Equals(qrdata));
                                if (printedqr != null)
                                {
                                    return Json(new { success = false, responseText = "This part already printed! can not print dublicate" }, JsonRequestBehavior.AllowGet);
                                }
                                srno++;
                            }
                            _log.LogInfoMessage("Final ZPL Command:\n" + zplBuilder);
                            barcodePrinter.SendStringToPrinter(item.Printer, zplBuilder.ToString(), FEnum.EnumScanMothod.NormalPrint.ToString());

                        }
                        else if (item.PrintType == (int)FEnum.QRCodeType.TMLQR)
                        {
                            for (int i = 1; i <= item.printcount; i++)
                            {
                                printandScanQRDatalist = QRCodeHelper.TMLQRCode(partdetails, customerdetails, srno, printandScanQRDatalist, out zpltext, out qrtext);
                                zplBuilder.AppendLine(zpltext);
                                var printedqr = entity.PrintandScanQRDatas.FirstOrDefault(x => x.QrText.Equals(qrtext));
                                if (printedqr != null)
                                {
                                    return Json(new { success = false, responseText = "This part already printed! can not print dublicate" }, JsonRequestBehavior.AllowGet);
                                }
                                srno++;
                            }
                            string fullZPL = zplBuilder.ToString();
                            _log.LogInfoMessage($"QR ZPL Text : {fullZPL}");
                            barcodePrinter.SendStringToPrinter(item.Printer, fullZPL.ToString(), FEnum.EnumScanMothod.NormalPrint.ToString());
                        }
                        else
                        {
                            return Json(new { success = false, responseText = "Please select QR Type" }, JsonRequestBehavior.AllowGet);

                        }
                    }
                    else
                    {
                        return Json(new { success = false, responseText = "Customer not found" }, JsonRequestBehavior.AllowGet);

                    }
                }
                else
                {
                    return Json(new { success = false, responseText = "Part Number is invalid or Customer not found for this part" }, JsonRequestBehavior.AllowGet);
                }
                if (sr != null && !isscaned)
                {
                    sr.Count = srno;
                    entity.SaveChanges();
                }
                else if (!isscaned)
                {
                    PartQRCount partQRCount = new PartQRCount
                    {
                        Count = srno,
                        PartDate = DateTime.Now

                    };
                    entity.PartQRCounts.Add(partQRCount);
                    entity.SaveChanges();
                }
                foreach (var items in printandScanQRDatalist)
                {
                    entity.PrintandScanQRDatas.Add(items);
                    entity.SaveChanges();
                }

                var totalcount = srno.ToString("D6");
                var srreturn = srno - 1;
                return Json(new { success = true, responseText = $"Part: {item.PartNumber} - SR.NO {startcount} To : {srreturn} Printed successfully!", totalcount = totalcount }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                _log.LogError($"Error Thrown during Edititem: {ex.Message}, { nameof(this.PrintQRCode)}, {DateTime.Now},{ex.StackTrace}");
                return Json(new { success = false, responseText = "Something wrong! please connect IT Team" }, JsonRequestBehavior.AllowGet);
            }
            finally
            {
                _log.LogDebugMessage($"{nameof(this.PrintQRCode)}, {LoggerMessage.ProcessEnded}, {DateTime.Now}");
            }
        }
        [HttpPost]
        public ActionResult PrintHoldQRCode(string printer)
        {

            try
            {
                _log.LogDebugMessage($"{nameof(this.PrintHoldQRCode)}, {LoggerMessage.ProcessStarted}, {DateTime.Now}");


                //BarcodePrinter barcodePrinter = new BarcodePrinter();
                List<PrintandScanQRData> printandScanQRDatas = new List<PrintandScanQRData>();

                var dubpart = entity.DublicateSRNumbers.Where(x => !string.IsNullOrEmpty(x.PartNo) && x.Status == 1).ToList();
                Customer customer = new Customer();
                if (dubpart != null && dubpart.Count > 0)
                {
                    string filePath = $"{ConfigurationHelper.GetConfigValue(ConfigurationHelper.QRData)}lebel.txt";
                    string text = System.IO.File.ReadAllText(filePath);

                    BarcodePrinter barcodePrinter = new BarcodePrinter();
                    //bool sent = barcodePrinter.SendStringToPrinter(printer, text);
                    StringBuilder zplBuilder = new StringBuilder();
                    dubpart.ToList().ForEach(items =>
                    {
                        var partdescription = entity.PartDetails.ToList().FirstOrDefault(x => !string.IsNullOrEmpty(x.PartNumber) && x.PartNumber.Equals(items.PartNo));

                        var srno = items.StartSRNo;
                        string zplqrtext74char = null;
                        string qrtext74char = null;
                        for (int i = 1; i <= items.Count; i++)
                        {
                            customer.CustomerId = items.Customer;
                            printandScanQRDatas = QRCodeHelper.BuildZPL_74charQRCode(partdescription, customer, srno, printandScanQRDatas, out zplqrtext74char, out qrtext74char);
                            if (!string.IsNullOrEmpty(qrtext74char))
                            {
                                zplBuilder.Append(zplqrtext74char);
                            }
                            srno++;
                        }
                        items.Status = 2;
                    });
                    string finalZpl = zplBuilder.ToString();
                    bool sent = barcodePrinter.SendStringToPrinter(printer, finalZpl, FEnum.EnumScanMothod.HoldPrint.ToString());

                    //entity.SaveChanges();
                    foreach (var item in printandScanQRDatas)
                    {
                        entity.PrintandScanQRDatas.Add(item);
                        entity.SaveChanges();
                    }
                }
                else
                {
                    return Json(new { success = true, responseText = "No Pending Qr available!" }, JsonRequestBehavior.AllowGet);

                }
                return Json(new { success = true, responseText = "Printed successfully!" }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                _log.LogError($"Error Thrown: {ex.Message}, { nameof(this.PrintHoldQRCode)}, {DateTime.Now},{ex.StackTrace}");



                return Json(new { success = false, responseText = "Something wrong! please connect IT Team" }, JsonRequestBehavior.AllowGet);

            }
            finally
            {
                _log.LogDebugMessage($"{nameof(this.PrintHoldQRCode)}, {LoggerMessage.ProcessEnded}, {DateTime.Now}");

            }

        }
        [HttpPost]
        public ActionResult QRData(DateTime fromdate, DateTime todate)
        {
            var data = entity.PrintandScanQRDatas.ToList().Where(x => x.PrintingTime.Date >= fromdate.Date && x.PrintingTime.Date <= todate.Date).ToList();
            using (var excelPackage = new ExcelPackage())
            {
                var worksheet = excelPackage.Workbook.Worksheets.Add("Employees");

                // Set header row
                worksheet.Cells[1, 1].Value = "Part Number";
                worksheet.Cells[1, 2].Value = "QR/Bar Code Text";
                worksheet.Cells[1, 3].Value = "Print Time";
                int row = 2;
                foreach (var qr in data)
                {
                    worksheet.Cells[row, 1].Value = qr.PartNumber;
                    worksheet.Cells[row, 2].Value = qr.QrText;
                    worksheet.Cells[row, 3].Value = qr.PrintingTime.ToString("dd/MM/yyyy hh:ss");
                    row++;
                }

                // Prepare Excel file byte array
                byte[] fileContents = excelPackage.GetAsByteArray();

                // Provide Excel file for download
                return File(fileContents, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "PDI_Details.xlsx");
            }
        }

        //Print QR & Barcode using scanned data
        public ActionResult PrintScannedCode(string part)
        {
            try
            {
                _log.LogDebugMessage($"{nameof(PrintScannedCode)}, Started, {DateTime.Now}");
                int srno = 0;                
                var printer = new BarcodePrinter();
                _log.LogInfoMessage($"PrintScannedCode - Scanned Data : {part} Date Time {DateTime.Now}");
                part = part.Trim();
                if(part.Length > 28)
                {
                    return Json(new { success = false, responseText = "Can not scan more then one qr at a time" });
                }
                var customer = GetCustomerByScanOrName(part, ref srno);
                if (customer == null)
                    return Json(new { success = false, responseText = "Invalid customer or scan text" });
                var partdetail = entity.PartDetails.FirstOrDefault(x => part.Trim().Contains(x.PartNumber));
                if (partdetail == null)
                    return Json(new { success = false, responseText = "Invalid Scanned text" });
                _log.LogInfoMessage($"TypeOfPart : {partdetail.TypeOfPart}");
                _log.LogInfoMessage($"PrintScannedCode - SR No : {srno} Date Time {DateTime.Now}");
                if(srno == 0)
                {
                    return Json(new { success = false, responseText = "Invalid SR No" });
                }

                var printList = new List<PrintandScanQRData>();
                string zpltext = null;
                string qrtext = null;
                string zplqrtext74char = null;
                string qrtext74char = null;
                if (customer.Name == "TML")
                {
                    printList = QRCodeHelper.BuildZPL_Barcode(partdetail, customer, srno, printList, out zpltext, out qrtext);

                }
                else if (customer != null)
                {

                    printList = QRCodeHelper.BuildZPL_QRCode(partdetail, customer, srno, printList, out zpltext, out qrtext);
                    if (customer.Name.Trim().ToLower() != "iptl")
                    {
                        //_log.LogInfoMessage($"Small QR Scanned ZPL Text : {zpl.ToString()}");
                        printList = QRCodeHelper.BuildZPL_74charQRCode(partdetail, customer, srno, printList, out zplqrtext74char, out qrtext74char);
                        if (!string.IsNullOrEmpty(qrtext74char))
                        {
                            if (entity.PrintandScanQRDatas.Any(x => x.QrText == qrtext74char))
                                return Json(new { success = false, responseText = "This part already printed! cannot print duplicate" });
                            var qrprinter = ConfigurationHelper.GetConfigValue(ConfigurationHelper.QrPrinter);
                            printer.SendStringToPrinter(qrprinter, zplqrtext74char.ToString(), FEnum.EnumScanMothod.ScannedPrint.ToString());
                        }
                    }
                }
                else
                {
                    return Json(new { success = false, responseText = "Please select QR Type" });
                }
                _log.LogInfoMessage($"PrintScannedCode - Full QR Text : {qrtext} Date Time {DateTime.Now}");
                // Check duplicate
                if (entity.PrintandScanQRDatas.Any(x => x.QrText == qrtext))
                    return Json(new { success = false, responseText = "This part already printed! cannot print duplicate" });


                // Send ZPL to Printer
                var barcodeprinter = ConfigurationHelper.GetConfigValue(ConfigurationHelper.BarcodePrinter);

                printer.SendStringToPrinter(barcodeprinter, zpltext.ToString(), FEnum.EnumScanMothod.ScannedPrint.ToString());

                entity.PrintandScanQRDatas.AddRange(printList);
                entity.SaveChanges();

                return Json(new
                {
                    success = true,
                    responseText = "Printed successfully!"
                });
            }
            catch (Exception ex)
            {
                _log.LogError($"Error in PrintQRCode: {ex.Message}, {ex.StackTrace}");
                return Json(new { success = false, responseText = "Something wrong! please connect IT Team" });
            }
            finally
            {
                _log.LogDebugMessage($"{nameof(PrintScannedCode)}, Ended, {DateTime.Now}");
            }
        }
        public ActionResult TestPeint(string printer)
        {
            try
            {
                _log.LogDebugMessage($"{nameof(TestPeint)}, Started, {DateTime.Now}");
                string filePath = $"{ConfigurationHelper.GetConfigValue(ConfigurationHelper.QRData)}testlebel.txt";
                string text = System.IO.File.ReadAllText(filePath);

                var print = new BarcodePrinter();
                print.SendStringToPrinter(printer, text.ToString(), FEnum.EnumScanMothod.TestPrint.ToString());

                return Json(new
                {
                    success = true,
                    responseText = $"Printed successfully!",

                });
            }
            catch (Exception ex)
            {
                _log.LogError($"Error in PrintQRCode: {ex.Message}, {ex.StackTrace}");
                return Json(new { success = false, responseText = "Something wrong! please connect IT Team" });
            }
            finally
            {
                _log.LogDebugMessage($"{nameof(TestPeint)}, Ended, {DateTime.Now}");
            }
        }

        private Customer GetCustomerByScanOrName(string scannedtext, ref int srno)
        {
           
            if (!string.IsNullOrEmpty(scannedtext))
            {               
                var customer = entity.Customers
                    .FirstOrDefault(x => scannedtext.StartsWith(x.CustomerId));

                if (scannedtext.Length >= 9)
                {
                    string withoutSuffix = scannedtext.Substring(0, scannedtext.Length - 3);

                    // get the last 6 digits
                    srno = int.Parse(withoutSuffix.Substring(withoutSuffix.Length - 6));
                }

                return customer;
            }
            return null;
        }  
        public class BarcodePrinter
        {
            protected SymlLogs _log;
            public BarcodePrinter()
            {
                _log = new SymlLogs(this.GetType().Name);
            }
            [DllImport("winspool.Drv", EntryPoint = "OpenPrinterA", SetLastError = true)]
            static extern bool OpenPrinter(string pPrinterName, out IntPtr phPrinter, IntPtr pDefault);

            [DllImport("winspool.Drv", EntryPoint = "ClosePrinter", SetLastError = true)]
            static extern bool ClosePrinter(IntPtr hPrinter);

            [DllImport("winspool.Drv", EntryPoint = "StartDocPrinterA", SetLastError = true)]
            static extern bool StartDocPrinter(IntPtr hPrinter, int level, [In] ref DOCINFOA di);

            [DllImport("winspool.Drv", EntryPoint = "EndDocPrinter", SetLastError = true)]
            static extern bool EndDocPrinter(IntPtr hPrinter);

            [DllImport("winspool.Drv", EntryPoint = "StartPagePrinter", SetLastError = true)]
            static extern bool StartPagePrinter(IntPtr hPrinter);

            [DllImport("winspool.Drv", EntryPoint = "EndPagePrinter", SetLastError = true)]
            static extern bool EndPagePrinter(IntPtr hPrinter);

            [DllImport("winspool.Drv", EntryPoint = "WritePrinter", SetLastError = true)]
            static extern bool WritePrinter(IntPtr hPrinter, IntPtr pBytes, int dwCount, out int dwWritten);

            [StructLayout(LayoutKind.Sequential)]
            public struct DOCINFOA
            {
                [MarshalAs(UnmanagedType.LPStr)] public string pDocName;
                [MarshalAs(UnmanagedType.LPStr)] public string pOutputFile;
                [MarshalAs(UnmanagedType.LPStr)] public string pDataType;
            }

            public bool SendStringToPrinter(string printerName, string zplCommand, string methodtype)
            {
                try
                {
                    _log.LogInfoMessage($"Method Name : {methodtype}");
                    _log.LogInfoMessage($"Printer Name : {printerName}");
                    _log.LogInfoMessage($"ZPL Command : {zplCommand}");
                    IntPtr pPrinter;
                    DOCINFOA di = new DOCINFOA { pDocName = "ZPL Barcode", pDataType = "RAW" };
                    bool success = false;

                    if (OpenPrinter(printerName.Normalize(), out pPrinter, IntPtr.Zero))
                    {
                        if (StartDocPrinter(pPrinter, 1, ref di))
                        {
                            StartPagePrinter(pPrinter);

                            IntPtr pBytes = Marshal.StringToCoTaskMemAnsi(zplCommand);
                            WritePrinter(pPrinter, pBytes, zplCommand.Length, out int bytesWritten);
                            Marshal.FreeCoTaskMem(pBytes);

                            EndPagePrinter(pPrinter);
                            EndDocPrinter(pPrinter);
                            success = true;
                        }
                        ClosePrinter(pPrinter);
                    }

                    return success;
                }
                catch (Exception ex)
                {

                    throw;
                }
                finally
                {

                }
            }
        }
    }
}