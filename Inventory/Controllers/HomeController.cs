using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Inventory.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        [HttpGet]
        public ActionResult CheckAuth()
        {
            Request.RequestContext.HttpContext.Response.AddHeader("Access-Control-Allow-Origin", "*");
            Request.RequestContext.HttpContext.Response.AddHeader("Access-Control-Allow-Header", "*");
            if(User.Identity.IsAuthenticated)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult Contact(string say)
        {
            Request.RequestContext.HttpContext.Response.AddHeader("Access-Control-Allow-Origin", "*");
            Request.RequestContext.HttpContext.Response.AddHeader("Access-Control-Allow-Header", "*");
            return Json(new { Respon = "You said " + say }, JsonRequestBehavior.AllowGet);
        }
    }
}