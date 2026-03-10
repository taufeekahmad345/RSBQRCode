using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace DF_WebModule.Controllers
{
    public class ErrorController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult RedirectError()
        {
            FormsAuthentication.SignOut();
            return View();
        }
    }
}