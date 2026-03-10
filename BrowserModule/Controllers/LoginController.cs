using DF_WebModule.Models;
using System;

using System.Web.Mvc;
using DF_WebModule.Classes;

using System.Configuration;
using DF_WebModule.DBModel;
using DF_WebModule.FEnum;
using System.Linq;
using System.Collections.Generic;
using System.Web.Security;
using System.IO;
using System.Web;
using DF_WebModule.HelpersClasess;

using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.Office.Interop.Excel;

namespace DF_WebModule.Controllers
{
    public class LoginController : Controller
    {
        protected SymlLogs _log;
        
        Engineering_DBEntities engineering_DBEntities = new Engineering_DBEntities();
        public LoginController()
        {
            _log = new SymlLogs(this.GetType().Name);
        }      
        private static string ForgetPasswordLink = ConfigurationManager.AppSettings["ForgetPasswordLink"];
        private static int Timeout = Convert.ToInt32(ConfigurationManager.AppSettings["Timeout"]);

        public ActionResult Index()      
        {
            try
            {
               
                _log.LogDebugMessage($"{nameof(this.Index)}, {LoggerMessage.ProcessStarted}, {DateTime.Now}");
                ViewBag.within30Days = false;        
               

                ViewBag.error = TempData["error"];
                LoginModal loginModal = new LoginModal();

                return View(loginModal);
            }

            catch (Exception ex)
            {

               //_log.LogException(ex);
               _log.LogError($"Error Thrown during : {ex.Message}, { nameof(this.Index)}, {DateTime.Now},{ex.StackTrace}");
                throw ex;

            }
            finally
            {
                _log.LogDebugMessage($"{nameof(this.Index)}, {LoggerMessage.ProcessEnded}, {DateTime.Now}");
            }
            
        }
        [ActionName("404")]
        public ActionResult NotFound()
        {
            return View("InvalidURL");
        }
        public ActionResult SessionTimeOut()
        {
            try
            {
                _log.LogDebugMessage($"{nameof(this.SessionTimeOut)}, {LoggerMessage.ProcessStarted}, {DateTime.Now}");
                HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
                if (authCookie != null)
                {
                    // Get the forms authentication ticket.
                    FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                    authCookie.Expires = DateTime.Now;
                    Response.Cookies.Add(authCookie);
                }
                //return Redirect(Request.UrlReferrer.PathAndQuery);
                return Json(new { success = true, responseText = "Session expired" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                _log.LogError($"Error Thrown during : {ex.Message}, { nameof(this.SessionTimeOut)}, {DateTime.Now},{ex.StackTrace}");

                return Json(new { success = false, responseText = ex.Message }, JsonRequestBehavior.AllowGet);
            }
            finally
            {
                _log.LogDebugMessage($"{nameof(this.SessionTimeOut)}, {LoggerMessage.ProcessEnded}, {DateTime.Now}");
            }
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Authorise(LoginModal LoginDetails, string ReturnUrl)
        {
            try
            {
                _log.LogDebugMessage($"{nameof(this.Authorise)}, {LoggerMessage.ProcessStarted}, {DateTime.Now}");
                ViewBag.within30Days = false;
                if (Session["ReturnUrl"] != null)
                {
                    ReturnUrl = Session["ReturnUrl"].ToString();
                }
                var year = 1;
                if (!string.IsNullOrEmpty(ConfigurationHelper.GetConfigValue(ConfigurationHelper.License)))
                {
                    year = Convert.ToInt32(ConfigurationHelper.GetConfigValue(ConfigurationHelper.License));
                }
                var startdate = ConfigurationHelper.ConvertStringToDate("01/09/2025");
                startdate = startdate.Value.AddYears(year);
                if (DateTime.Now < startdate)
                {
                    if (ModelState.IsValid)
                    {
                        List<MenuModel> menus = new List<MenuModel>();
                        var users = engineering_DBEntities.UserDetails.ToList();
                        var user = engineering_DBEntities.UserDetails.Where(x => x.Username == LoginDetails.Username && x.Password == LoginDetails.Password).FirstOrDefault();
                        if (user != null)
                        {
                            clsCarePad.UserId = Convert.ToInt32(user.Id);
                            Session["Userd"] = user.Id;
                            Session["Role"] = user.RoleId;
                            clsCarePad.UserName = user.Name;
                            Session["UserName"] = user.Name;
                            _log.LogInfoMessage($"{nameof(this.Authorise)} {Session["Role"]}, {DateTime.Now}");

                            ReturnUrl = Url.Action("Blankpage", "Home");
                            Session["Timeout"] = Timeout * 60; // Convert timeout to seconds for frontend.
                            bool ExistingSession = System.Web.HttpContext.Current.User.Identity.IsAuthenticated;
                            if (!ExistingSession)
                            {
                                FormsAuthentication.SetAuthCookie(LoginDetails.Username, false);
                                return Redirect(ReturnUrl);
                            }
                            else
                            {
                                return Redirect(ReturnUrl);
                            }


                        }
                        else
                        {
                            ViewBag.error = "Username or password is incorrect plase try again.";
                            return View("Index");
                        }

                    }
                    else
                    {
                        return RedirectToAction("Index");
                    }
                }
                else {
                    ViewBag.error = "Your license has been expired please contact the it support team.";
                    return View("Index");
                }
            }
            catch (Exception ex)
            {
                _log.LogError($"Error Thrown during : {ex.Message}, { nameof(this.Index)}, {DateTime.Now},{ex.StackTrace}");

                throw ex;
            }
            finally
            {
                _log.LogDebugMessage($"{nameof(this.Authorise)}, {LoggerMessage.ProcessEnded}, {DateTime.Now}");
            }
        }

        [HttpGet]
        public ActionResult ChangePassword()
        {
            return View();
        
        }

        [HttpPost]
        public ActionResult ChangePassword(ChangePassword data)
        {
            try
            {
                var id = Session["Userd"];
                var user = engineering_DBEntities.UserDetails.ToList().FirstOrDefault(x => x.Password == data.OldPassword);
                if (user != null)
                {
                    user.Password = data.NewPassword;
                    engineering_DBEntities.SaveChanges();
                    return Json(new { success = true, responseText = "Password updated successfully!" }, JsonRequestBehavior.AllowGet);

                }
                return Json(new { success = false, responseText = "Old Password does not exist!" }, JsonRequestBehavior.AllowGet);

            }
            catch
            {
                return Json(new { success = false, responseText = "something wrong please conatct to it support team" }, JsonRequestBehavior.AllowGet);


            }
            finally
            { 
            
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
       
        public ActionResult LogOut()
        {
            try
            {
                _log.LogDebugMessage($"{nameof(this.LogOut)}, {LoggerMessage.ProcessStarted}, {DateTime.Now}");
                Session.Clear();
                FormsAuthentication.SignOut();
                clsCarePad.MenuList = new List<MenuModel>();
                clsCarePad.UserId = null;
                return RedirectToAction("Index", "Login");
            }
            catch (Exception ex)
            {
                _log.LogError($"Error Thrown during : {ex.Message}, { nameof(this.LogOut)}, {DateTime.Now},{ex.StackTrace}");

                throw ex;
            }
            finally
            {
                _log.LogDebugMessage($"{nameof(this.LogOut)}, {LoggerMessage.ProcessStarted}, {DateTime.Now}");
            }


        }

    }
}