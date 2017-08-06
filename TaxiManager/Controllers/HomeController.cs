using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TaxiManager.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //Поскольку создавать заявки может неавторизованный пользователь, для его идентификации используем куки
            var cookie = Request.Cookies["user_guid"];

            if (cookie == null)
            {
                cookie = new HttpCookie("user_guid", Guid.NewGuid().ToString());
                Response.AppendCookie(cookie);
            }

            return View();
        }

        //public ActionResult About()
        //{
        //    ViewBag.Message = "Your application description page.";

        //    return View();
        //}

        //public ActionResult Contact()
        //{
        //    ViewBag.Message = "Your contact page.";

        //    return View();
        //}
    }
}