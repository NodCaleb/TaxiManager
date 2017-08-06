using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TaxiManager.Models;

namespace TaxiManager.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        // GET: Admin
        //Редактирование пользователей
        public ActionResult Users()
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                //Если не админ - не пускаем
                if (User.IsInRole("Администратор")) return RedirectToAction("Index", "Home");
                //Спиок ролей, чтобы назначать пользователям
                ViewData["roles"] = db.Roles.ToList();

                return View();
            }
        }
    }
}