using DF_WebModule.Classes;
using DF_WebModule.DBModel;
using DF_WebModule.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DF_WebModule.Controllers
{
    public class CustomerController : Controller
    {
        protected SymlLogs _log;
        Engineering_DBEntities DBEntities = new Engineering_DBEntities();
        private static string Filefullpath = ConfigurationManager.AppSettings["OpenFilepath"];
        public CustomerController()
        {
            _log = new SymlLogs(this.GetType().Name);
        }

        public ActionResult Index()
        {
            clsCustomer customer = new clsCustomer();
            List<clsCustomer> clsCustomers = new List<clsCustomer>();
            var custom = DBEntities.Customers.ToList();
            foreach (var item in custom)
            {
                clsCustomers.Add(new clsCustomer
                {
                    Address = item.Address,
                    City = item.City,
                    CustomerId = item.CustomerId,
                    CustomerName = item.Name,
                    Email1 = item.Email1,
                    Email2 = item.Email2,
                    Mobile = item.Mobile,
                     Phone = item.Phone,
                     PinCode = item.PinNo,
                     State = item.State,
                     Status = item.Status,
                     Id = item.Id
                
                });
            }
            customer.ListCustomer = ConvertViewToString("Customers", clsCustomers);
            return View(customer);
        }
        public ActionResult EditItem(int ID)
        {
            try
            {
                _log.LogDebugMessage($"{nameof(this.EditItem)}, {LoggerMessage.ProcessStarted}, {DateTime.Now}");
                var item = DBEntities.Customers.FirstOrDefault(x => x.Id == ID);
                if (item != null)
                {
                    return Json(new
                    {
                        success = true,
                        customer = item.Name,
                        address = item.Address,
                        city = item.City,
                        state = item.State,
                        pin = item.PinNo,
                        phone = item.Phone,
                        mobile = item.Mobile,
                        email1 = item.Email1,
                        email2 = item.Email2,
                        status = item.Status,
                        cId = item.CustomerId
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
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
                _log.LogDebugMessage($"{nameof(this.DeleteItem)}, {LoggerMessage.ProcessStarted}, {DateTime.Now}");
                var item = DBEntities.Customers.FirstOrDefault(x => x.Id == ID);
                if (item == null)
                {
                    return Json(new { success = false, responseText = "No Data found." }, JsonRequestBehavior.AllowGet);
                }
                DBEntities.Entry(item).State = System.Data.Entity.EntityState.Deleted;
                DBEntities.SaveChanges();
                return Json(new { success = true, responseText = "Deleted Successfully!" }, JsonRequestBehavior.AllowGet);

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
        [HttpPost]
        public ActionResult AddNewitem(clsCustomer item)
        {
            try
            {
                _log.LogDebugMessage($"{nameof(this.AddNewitem)}, {LoggerMessage.ProcessStarted}, {DateTime.Now}");
                if (ModelState.IsValid && !string.IsNullOrEmpty(item.CustomerId))
                {
                    var partnum = DBEntities.Customers.ToList().FirstOrDefault(x => x.CustomerId == item.CustomerId.Trim());
                    if (partnum == null)
                    {
                        DBModel.Customer engineering = new DBModel.Customer
                        {
                            CreatedOn = DateTime.Now,
                            CreatedBy = "",
                            CustomerId = item.CustomerId,
                            Name = item.CustomerName,
                            Email1 = item.Email1,
                            Email2 = item.Email2,
                            Mobile = item.Mobile,
                            Phone = item.Phone,
                            Address = item.Address,
                            City = item.City,
                            State = item.State,
                                Status = item.Status,
                            PinNo = item.PinCode,
                            IsActive = true
                        };
                        DBEntities.Customers.Add(engineering);

                    }
                    else
                    {

                        partnum.Name = item.CustomerName;
                        partnum.Address = item.Address;
                        partnum.City = item.City;
                        partnum.Status = item.Status;
                        partnum.PinNo = item.PinCode;
                        partnum.Mobile = item.Mobile;
                        partnum.Phone = item.Phone;
                        partnum.Email1 = item.Email1;
                        partnum.Email2 = item.Email2;
                        partnum.Status = item.Status;
                    }
                    DBEntities.SaveChanges();


                    return Json(new { success = true, responseText = "New Item has been added into the system, please click okay to return to the Item list" }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { success = true, responseText = "Customer Id Requred" }, JsonRequestBehavior.AllowGet);

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
        public ActionResult AddSetting(clsconfig item)
        {
            try
            {
                _log.LogDebugMessage($"{nameof(this.AddNewitem)}, {LoggerMessage.ProcessStarted}, {DateTime.Now}");
                if (ModelState.IsValid)
                {
                    var partnum = DBEntities.ConfigSettings.ToList().FirstOrDefault();
                    if (partnum == null)
                    {
                        DBModel.ConfigSetting engineering = new DBModel.ConfigSetting
                        {
                            CreatedOn = DateTime.Now,
                            CreatedBy = "",
                            CompanyName = item.CompanyName,
                            CSTNo = item.CSTNo,
                            TINNO = item.TINNO,
                            Fax = item.Fax,
                            Mobile = item.Mobile,
                            Phone = item.Phone,
                            Address = item.Address,
                            City = item.City,
                            ExiceNo = item.ExiceNo,
                            Email = item.Email,
                            PinNo = item.PinNo,
                            IsActive = true,

                        };
                        DBEntities.ConfigSettings.Add(engineering);

                    }
                    else
                    {

                        partnum.CompanyName = item.CompanyName;
                        partnum.Address = item.Address;
                        partnum.City = item.City;
                        partnum.CSTNo = item.CSTNo;
                        partnum.PinNo = item.PinNo;
                        partnum.Mobile = item.Mobile;
                        partnum.Phone = item.Phone;
                        partnum.Email = item.Email;
                        partnum.TINNO = item.TINNO;
                        partnum.Fax = item.Fax;
                        partnum.ExiceNo = item.ExiceNo;
                    }
                    DBEntities.SaveChanges();


                    return Json(new { success = true, responseText = "Updated Successfully!" }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { success = true, responseText = "Customer Id Requred" }, JsonRequestBehavior.AllowGet);

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
        [HttpGet]
        public ActionResult Setting()
        {
            clsconfig clsconfig = new clsconfig();
            var setting = DBEntities.ConfigSettings.FirstOrDefault();
            if (setting != null)
            {
                clsconfig.Address = setting.Address;
                clsconfig.City = setting.City;
                clsconfig.CompanyName = setting.CompanyName;
                clsconfig.CSTNo = setting.CSTNo;
                clsconfig.Email = setting.Email;
                clsconfig.ExiceNo = setting.ExiceNo;
                clsconfig.Fax = setting.Fax;
                clsconfig.Mobile = setting.Mobile;
                clsconfig.Phone = setting.Phone;
                clsconfig.PinNo = setting.PinNo;
                clsconfig.TINNO = setting.TINNO;
            }

            return View(clsconfig);
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