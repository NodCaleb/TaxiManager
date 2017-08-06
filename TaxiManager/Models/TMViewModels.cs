using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

//Модели для отправки данных на сервер через AJAX
namespace TaxiManager.Models
{
    //Данные заявки
    public class TMRideRequestsVM
    {
        public List<TMRideRequest> Requests { get; set; }
        public List<ApplicationUser> Drivers { get; set; }
        public bool Dispatcher { get; set; }
        public bool Driver { get; set; }

        public TMRideRequestsVM()
        {
            Dispatcher = false;
            Driver = false;
        }
    }
    //Данные пользователя
    public class TMUsersVM
    {
        public List<ApplicationUser> Users { get; set; }
        public List<IdentityRole> Roles { get; set; }
    }
}