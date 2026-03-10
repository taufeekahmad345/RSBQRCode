 using DF_WebModule.Classes;
using DF_WebModule.DBModel;
using DF_WebModule.Models;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DF_WebModule.Controllers
{
    public class PartManagementController : Controller
    {

        protected SymlLogs _log;
        Engineering_DBEntities DBEntities = new Engineering_DBEntities();
        private static string Filefullpath = ConfigurationManager.AppSettings["OpenFilepath"];
        public PartManagementController()
        {
            _log = new SymlLogs(this.GetType().Name);
        }

        public ActionResult Index()
        {
            try
            {
                _log.LogDebugMessage($"{nameof(this.Index)}, {LoggerMessage.ProcessStarted}, {DateTime.Now}");
                List<EngineeringModel> eng = new List<EngineeringModel>();
                List<Classes.Customer> customers = new List<Classes.Customer>();
                List<Classes.Type> types = new List<Classes.Type>();
                var engi = DBEntities.PartDetails.ToList();
                foreach (var item in engi)
                {
                    eng.Add(new EngineeringModel
                    {
                        Id = item.Id,
                        CustomerName = item.CustomerName,
                        PartNumber = item.PartNumber,
                        Part_Description = item.Part_Description,                        
                        TypeOfPart = item.TypeOfPart,
                        Joint = item.Joint,
                        Tube_Length = item.Tube_Length,
                        ModNo = item.RevNo,
                        Vendorcode = item.VendorCode,
                        Remark = item.Remark
                        
                    });
                }
                //var customer = DBEntities.Customers.ToList();
                //foreach (var c in customer)
                //{
                //    customers.Add(new Classes.Customer
                //    {
                //        Id = c.Id,
                //        Name = c.Name
                //    });
                //}
                foreach (var t in engi.Where(x=> !string.IsNullOrEmpty(x.TypeOfPart)).GroupBy(x=>x.TypeOfPart))
                {
                    types.Add(new Classes.Type
                    {
                        Id = 1,
                        Name = t.Key
                    });
                }
                clsPartDetail clsPartDetail = new clsPartDetail();
                clsPartDetail.types = types;
                //clsPartDetail.customers = customers;

                clsPartDetail.PartList = ConvertViewToString("PartDetails", eng);
                return View(clsPartDetail);
            }
            catch(Exception ex)
            {
                return null;
            }
            finally
            {
                _log.LogDebugMessage($"{nameof(this.Index)}, {LoggerMessage.ProcessEnded}, {DateTime.Now}");

            }
        }

        public ActionResult EditItem(int ID)
        {
            try
            {
                _log.LogDebugMessage($"{nameof(this.EditItem)}, {LoggerMessage.ProcessStarted}, {DateTime.Now}");
                var item = DBEntities.PartDetails.FirstOrDefault(x => x.Id == ID);
                if (item != null)
                {
                    return Json(new { success = true,
                        customer = item.CustomerName,
                        partn = item.PartNumber,
                        description = item.Part_Description,
                        joint = item.Joint,
                        length = item.Tube_Length,
                        remark = item.Remark,
                        TypeOfPart = item.TypeOfPart,
                        revno = item.RevNo}, JsonRequestBehavior.AllowGet);
                }
                else {
                    return Json(new { success = false, responseText = "No Data found." }, JsonRequestBehavior.AllowGet);
                }
                
            }
            catch (Exception ex)
            {
                _log.LogError($"Error Thrown during Edititem: {ex.Message}, { nameof(this.EditItem)}, {DateTime.Now},{ex.StackTrace}");

                return RedirectToAction("RedirectError", "Error");
            }
            finally
            {
                _log.LogDebugMessage($"{nameof(this.EditItem)}, {LoggerMessage.ProcessEnded}, {DateTime.Now}");
            }
        }

        public ActionResult DeleteItem(int ID)
        {
            try
            {
                if (!string.IsNullOrEmpty(Session["Role"].ToString()) && Session["Role"].ToString() == "1")
                {
                    _log.LogDebugMessage($"{nameof(this.DeleteItem)}, {LoggerMessage.ProcessStarted}, {DateTime.Now}");
                    var item = DBEntities.PartDetails.FirstOrDefault(x => x.Id == ID);
                    if (item == null)
                    {
                        return Json(new { success = false, responseText = "No Data found." }, JsonRequestBehavior.AllowGet);
                    }
                    DBEntities.Entry(item).State = System.Data.Entity.EntityState.Deleted;
                    DBEntities.SaveChanges();
                    return Json(new { success = true, responseText = "Deleted Successfully!" }, JsonRequestBehavior.AllowGet);
                }
                else {
                    return Json(new
                    {
                        success = false,
                        responseText = "You are not authorized to delete."
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                _log.LogError($"Error Thrown during: {ex.Message}, { nameof(this.DeleteItem)}, {DateTime.Now},{ex.StackTrace}");

                return RedirectToAction("RedirectError", "Error");
            }
            finally
            {
                _log.LogDebugMessage($"{nameof(this.DeleteItem)}, {LoggerMessage.ProcessEnded}, {DateTime.Now}");
            }
        }
        [HttpGet]
        public JsonResult GetPartsByCustomer(string customerId)
        {
            var parts = DBEntities.PartDetails.ToList().Where(p => !string.IsNullOrEmpty(p.CustomerName) && p.CustomerName.Trim() == customerId.Trim())
                         .Select(p => new SelectListItem
                         {
                             Text = p.PartNumber,
                             Value = p.PartNumber
                         }).ToList();

            return Json(parts, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        public ActionResult AddNewitem(clsPartDetail item)
        {
            try
            {
                _log.LogDebugMessage($"{nameof(this.AddNewitem)}, {LoggerMessage.ProcessStarted}, {DateTime.Now}");
                _log.LogInfoMessage($"{nameof(this.AddNewitem)} {Session["Role"]}, {DateTime.Now}");

                if (Convert.ToString(Session["Role"]) == "2")
                {
                    if (ModelState.IsValid && !string.IsNullOrEmpty(item.PartNumber))
                    {
                        var message = "Added Successfully!";
                        var partnum = DBEntities.PartDetails.ToList().FirstOrDefault(x => x.PartNumber == item.PartNumber.Trim());
                        if (partnum == null)
                        {
                            PartDetail engineering = new PartDetail
                            {
                                IsDeleted = false,
                                IsActive = true,
                                CustomerName = item.CustomerName,
                                VendorCode = item.Vendorcode,
                                TypeOfPart = item.TypeOfPart,
                                Tube_Length = item.Tube_Length,
                                Joint = item.Joint,
                                PartNumber = item.PartNumber,
                                Part_Description = item.Part_Description,
                                Remark = item.Remark,
                                RevNo = item.ModNo
                            };
                            DBEntities.PartDetails.Add(engineering);

                        }
                        else
                        {

                            partnum.Part_Description = item.Part_Description;
                            partnum.Remark = item.Remark;
                            partnum.RevNo = item.ModNo;
                            partnum.Joint = item.Joint;
                            partnum.Tube_Length = item.Tube_Length;
                            partnum.TypeOfPart = item.TypeOfPart;
                            partnum.CustomerName = item.CustomerName;
                            message = "Updated Successfully!";
                        }
                        DBEntities.SaveChanges();


                        return Json(new { success = true, responseText = message }, JsonRequestBehavior.AllowGet);
                    }
                    return Json(new { success = true, responseText = "Part Number Requred" }, JsonRequestBehavior.AllowGet);
                }
                else {
                    return Json(new
                    {
                        success = false,
                        responseText = "You are not authorized to add or update the data."
                    }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {
                _log.LogError($"Error Thrown during Add Item: {ex.Message}, { nameof(this.AddNewitem)}, {DateTime.Now},{ex.StackTrace}");

                return RedirectToAction("RedirectError", "Error");
            }
            finally
            {
                _log.LogDebugMessage($"{nameof(this.AddNewitem)}, {LoggerMessage.ProcessEnded}, {DateTime.Now}");
            }
        }


        [HttpPost]
        public ActionResult ImportExcel(HttpPostedFileBase file)
        {
            try
            {
                _log.LogDebugMessage($"{nameof(this.ImportExcel)}, {LoggerMessage.ProcessStarted}, {DateTime.Now}");
                if (!string.IsNullOrEmpty(Session["Role"].ToString()) && Session["Role"].ToString() == "1")
                {
                    if (file != null && file.ContentLength > 0)
                    {
                        using (var package = new ExcelPackage(file.InputStream))
                        {
                            ExcelWorksheet worksheet = package.Workbook.Worksheets[0];

                            var rowCount = worksheet.Dimension.End.Row;
                            var colCount = worksheet.Dimension.End.Column;
                            Dictionary<int, string> columnMap = new Dictionary<int, string>();
                            clsCarePad.UserName = Session["UserName"] != null ? Session["UserName"].ToString() : clsCarePad.UserName;

                            for (int row = 2; row <= rowCount; row++)
                            {
                                var part = worksheet.Cells[row, 2].Text;
                                PartDetail newData = new PartDetail();
                                newData.PartNumber = worksheet.Cells[row, 1].Text;
                                newData.Part_Description = worksheet.Cells[row, 2].Text;
                                newData.TypeOfPart = worksheet.Cells[row, 3].Text;
                                newData.Joint = worksheet.Cells[row, 4].Text;
                                newData.Tube_Length = worksheet.Cells[row, 5].Text;
                                newData.RevNo = worksheet.Cells[row, 6].Text;
                                newData.CustomerName = worksheet.Cells[row, 7].Text;
                                newData.Remark = worksheet.Cells[row, 8].Text;
                                newData.IsActive = true;

                                var partnum = DBEntities.PartDetails.ToList().FirstOrDefault(x => x.PartNumber == newData.PartNumber);
                                if (partnum == null)
                                {

                                    DBEntities.PartDetails.Add(newData);


                                }
                                else
                                {

                                    partnum.Part_Description = newData.Part_Description;
                                    partnum.Remark = newData.Remark;
                                    partnum.RevNo = newData.RevNo;
                                    partnum.Joint = newData.Joint;
                                    partnum.Tube_Length = newData.Tube_Length;
                                    partnum.TypeOfPart = newData.TypeOfPart;
                                    partnum.CustomerName = newData.CustomerName;
                                }
                                DBEntities.SaveChanges();
                            }

                        }
                    }
                    else
                    {
                        return Json(new
                        {
                            success = false,
                            responseText = "Please provide valid excel."
                        }, JsonRequestBehavior.AllowGet);

                    }
                }
                else
                {
                    return Json(new
                    {
                        success = false,
                        responseText = "You are not authorized to import the new data."
                    }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {
                _log.LogError($"Error Thrown during: {ex.Message}, { nameof(this.ImportExcel)}, {DateTime.Now},{ex.StackTrace}");

            }
            finally
            {
                _log.LogDebugMessage($"{nameof(this.ImportExcel)}, {LoggerMessage.ProcessEnded}, {DateTime.Now}");

            }
            return Json(new
            {
                success = true,
                responseText = "The excel has been imported into the system."
            }, JsonRequestBehavior.AllowGet);


        }

        public ActionResult DownloadExcel()
        {


            try
            {
                _log.LogDebugMessage($"{nameof(this.DownloadExcel)}, {LoggerMessage.ProcessStarted}, {DateTime.Now}");

                var schedulin = DBEntities.PartDetails.ToList();

                // Create Excel package and worksheet
                using (var excelPackage = new ExcelPackage())
                {
                    var worksheet = excelPackage.Workbook.Worksheets.Add("Employees");
                    var col = 1;
                    // Set header row
                    worksheet.Cells[1, col].Value = "S.NO";
                    worksheet.Cells[1, ++col].Value = "Part NO";
                    worksheet.Cells[1, ++col].Value = "Description";
                    worksheet.Cells[1, ++col].Value = "Type";
                    worksheet.Cells[1, ++col].Value = "JT";
                    worksheet.Cells[1, ++col].Value = "Length";
                    worksheet.Cells[1, ++col].Value = "RevLvl";
                    worksheet.Cells[1, ++col].Value = "Customer Name";
                    worksheet.Cells[1, ++col].Value = "Remark";


                    int row = 2;
                    foreach (var items in schedulin)
                    {
                        var colum = 1;
                        worksheet.Cells[row, colum].Value = items.Id;
                        worksheet.Cells[row, ++colum].Value = items.PartNumber;
                        
                        worksheet.Cells[row, ++colum].Value = items.Part_Description;
                        worksheet.Cells[row, ++colum].Value = items.TypeOfPart;
                        worksheet.Cells[row, ++colum].Value = items.Joint;
                        worksheet.Cells[row, ++colum].Value = items.Tube_Length;
                        worksheet.Cells[row, ++colum].Value = items.RevNo;
                        worksheet.Cells[row, ++colum].Value = items.CustomerName;
                        worksheet.Cells[row, ++colum].Value = items.Remark;

                        row++;
                    }

                    // Prepare Excel file byte array
                    byte[] fileContents = excelPackage.GetAsByteArray();

                    // Provide Excel file for download
                    return File(fileContents, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Part Details.xlsx");
                }
            }
            catch (Exception ex)
            {
                _log.LogError($"Error Thrown during: {ex.Message}, { nameof(this.DownloadExcel)}, {DateTime.Now},{ex.StackTrace}");

                return RedirectToAction("RedirectError", "Error");
            }
            finally
            {
                _log.LogDebugMessage($"{nameof(this.DownloadExcel)}, {LoggerMessage.ProcessEnded}, {DateTime.Now}");

            }

        }
        public string ConvertViewToString(string viewName, object model)
        {
            //This is used to convert any view with a model into a string
            ViewData.Model = model;
            using (StringWriter writer = new StringWriter())
            {
                ViewEngineResult vResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                ViewContext vContext = new ViewContext(this.ControllerContext, vResult.View, ViewData, new TempDataDictionary(), writer);
                vResult.View.Render(vContext, writer);
                return writer.ToString();
            }
        }
    }
}