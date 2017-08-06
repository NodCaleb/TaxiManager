using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;
using TaxiManager.Models;

namespace TaxiManager.Controllers
{
    //Контроллер для обработки AJAX-запросов
    [SessionState(SessionStateBehavior.Disabled)] //Это на случай, если вдруго понадобится отбрабатывать несколько запросов от пользователя одновременно (потому и контроллер отдельный)
    public class AjaxController : Controller
    {
        //Все методы - POST, чтобы пользователь по GET-запросам ничего не получил (типа /Ajax/GetRideRequests в адресной строке)
        [HttpPost]
        //Список заявок
        public ActionResult GetRideRequests()
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                try
                {
                    TMRideRequestsVM model = new TMRideRequestsVM();
                    //Если авторизован диспетчер - отображаем заявки в работе в списке для диспетчера
                    if (Request.IsAuthenticated && User.IsInRole("Диспетчер"))
                    {
                        //Это диспетчер
                        model.Dispatcher = true;
                        //Список заявок в работе
                        model.Requests = db.RideRequests.Include("Driver").Where(m => m.Status != 3 && m.Status != 4).ToList();
                        //Роль водителя
                        IdentityRole driverRole = db.Roles.SingleOrDefault(m => m.Name == "Водитель");
                        //Список водителей для назначения на заявки
                        model.Drivers = db.Users.Where(m => m.Roles.Any(n => n.RoleId == driverRole.Id)).ToList();
                    }
                    //Если авторизован водитель - отображаем заявки в работе в списке для водителя
                    else if (Request.IsAuthenticated && User.IsInRole("Водитель"))
                    {
                        ApplicationUser user = db.Users.SingleOrDefault(m => m.UserName == User.Identity.Name);
                        //Это водитель
                        model.Driver = true;
                        //Списко заявок, назначенных на этого водителя
                        model.Requests = db.RideRequests.Where(m => (m.Status == 1 || m.Status == 2) && m.Driver.Id == user.Id).ToList();
                    }
                    //Если не водиель и не диспетчер - показываем заявки текущего пользователя
                    else
                    {
                        //Куки текущего пользователя
                        var cookie = Request.Cookies["user_guid"];
                        string userGuid = cookie != null ? cookie.Value : "";
                        //Его заявки
                        model.Requests = db.RideRequests.Include("Driver").Where(m => m.UserGUID == userGuid).ToList();
                    }

                    return PartialView(model);
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }
        [HttpPost]
        //Создаем заявку
        public JsonResult CreateRideRequest(TMRequestPM data)
        {
            try
            {
                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    //Куки текущего пользователя
                    var cookie = Request.Cookies["user_guid"];
                    string userGuid = cookie != null ? cookie.Value : "";

                    TMRideRequest request = new TMRideRequest
                    {
                        CreationTime = DateTime.Now,
                        StartLocation = data.Start,
                        FinalDestination = data.Destination,
                        Status = 0,
                        UserGUID = userGuid
                    };

                    db.RideRequests.Add(request);
                    db.SaveChanges();

                    //Если все получилось - возвращаем ID новой заявки (на всякий)
                    return Json(new { Id = request.Id });
                }
            }
            catch (Exception e)
            {
                //Если что-то пошло не так - возвращаем сообщение об ошибке
                return Json(new { ErrorMessage = e.Message });
            }
        }
        [HttpPost]
        //Изменяем заявку
        public JsonResult EditRideRequest(TMRequestPM data)
        {
            try
            {
                //Только водитель или диспетчер могут менять заявки
                if (!User.IsInRole("Диспетчер") && !User.IsInRole("Водитель"))
                {
                    throw new Exception("Неавторизованный доступ");
                }

                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    //Ищем заявку по ID
                    TMRideRequest request = db.RideRequests.Include("Driver").Single(m => m.Id == data.Id);

                    //Назначаем статус
                    if (data.Status != -1)
                    {
                        request.Status = data.Status;
                        if(data.Status == 4)
                        {
                            request.FinishTime = DateTime.Now;
                        }
                    }
                    //Назначаем водителя
                    if (!string.IsNullOrEmpty(data.DriverId))
                    {
                        ApplicationUser driver = db.Users.SingleOrDefault(m => m.Id == data.DriverId);
                        request.Driver = driver;
                        request.Status = 1;
                    }
                    //Отказ водителя от заявки
                    if (data.Reject)
                    {
                        request.Driver = null;
                        request.Status = 1;
                    }
                    
                    db.SaveChanges();

                    return Json(new { Id = request.Id });
                }
            }
            catch (Exception e)
            {
                return Json(new { ErrorMessage = e.Message });
            }
        }
        [HttpPost]
        //Список пользователей для админа
        public ActionResult GetUsers()
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                try
                {
                    TMUsersVM model = new TMUsersVM
                    {
                        Users = db.Users.Include("Roles").ToList(),
                        Roles = db.Roles.ToList()
                    };

                    return View(model);
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }
        [HttpPost]
        //Создание пользователя
        public JsonResult CreateUser(TMUserPM data)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                try
                {
                    var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
                    var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));

                    var newUser = new ApplicationUser
                    {
                        UserName = data.UserName,
                        Email = data.Email,
                        PersonalName = data.PersonalName
                    };

                    var chkUser = UserManager.Create(newUser, data.Password);

                    //Add default User to Role Admin 
                    if (chkUser.Succeeded)
                    {
                        UserManager.AddToRole(newUser.Id, data.Role);

                        return Json(new { Id = newUser.Id });
                    }
                    else
                    {
                        StringBuilder sb = new StringBuilder();

                        foreach (var error in chkUser.Errors)
                        {
                            sb.Append(error);
                            sb.Append(" ");
                        }

                        return Json(new { ErrorMessage = sb.ToString().Trim() });
                    }
                }
                catch (Exception e)
                {
                    return Json(new { ErrorMessage = e.Message });
                }
                
            }
        }
        [HttpPost]
        //Назначение роли пользователю
        public JsonResult AssignRole(TMUserPM data)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                try
                {
                    var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
                    var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));

                    ApplicationUser user = db.Users.Single(m => m.Id == data.Id);

                    UserManager.RemoveFromRoles(user.Id, UserManager.GetRoles(user.Id).ToArray());

                    UserManager.AddToRole(user.Id, data.Role);

                    return Json(new { Id = user.Id });
                }
                catch (Exception e)
                {
                    return Json(new { Message = e.Message });
                }

            }
        }
    }
}