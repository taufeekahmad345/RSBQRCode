using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DF_WebModule.Classes;
using DF_WebModule.Models;
using Newtonsoft.Json;
using System.Globalization;
using DF_WebModule.FEnum;
using Newtonsoft.Json;
using System.IO;
using System.Configuration;
using DF_WebModule.DBModel;
using System.Drawing;
using System.Drawing.Imaging;
//using iText.Kernel.Pdf;
//using iText.Forms;

namespace DF_WebModule.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        protected SymlLogs _log;
        Engineering_DBEntities engineering_DBEntities = new Engineering_DBEntities();
        public HomeController()
        {
            _log = new SymlLogs(this.GetType().Name);
        }
        
        public ActionResult Blankpage()
        {
            Dashboard dashboard = new Dashboard();
            dashboard.From = DateTime.Now.ToString("dd/MM/yyyy");

            dashboard.To = DateTime.Now.ToString("dd/MM/yyyy");
            var user = engineering_DBEntities.UserDetails.ToList();

            dashboard.UserList = ConvertViewToString("UserList", user);
            return View(dashboard);
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
