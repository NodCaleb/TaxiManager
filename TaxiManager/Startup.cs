using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using System;
using TaxiManager.Models;

[assembly: OwinStartupAttribute(typeof(TaxiManager.Startup))]
namespace TaxiManager
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            CreateRolesandUsers();
        }

        //Создаем тестовых пользователей, если база пустая
        private void CreateRolesandUsers()
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));

                //Если нет роли Администратор - создаем эту роль и пользователя в ней
                if (!roleManager.RoleExists("Администратор"))
                {
                    //Новая роль
                    var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                    role.Name = "Администратор";
                    roleManager.Create(role);

                    //Новый пользователь
                    var adminUser = new ApplicationUser
                    {
                        UserName = "Admin",
                        Email = "admin@taxi.ru",
                        PersonalName = "Александр"
                    };

                    string password0 = "!QAZ2wsx";

                    var chkUser0 = UserManager.Create(adminUser, password0);

                    //Назначаем новому пользователю роль 
                    if (chkUser0.Succeeded)
                    {
                        UserManager.AddToRole(adminUser.Id, "Администратор");

                    }
                }

                //Если нет роли Диспетчер - создаем эту роль и пользователей в ней
                if (!roleManager.RoleExists("Диспетчер"))
                {
                    var role = new IdentityRole();
                    role.Name = "Диспетчер";
                    roleManager.Create(role);

                    var dispatcherUser0 = new ApplicationUser()
                    {
                        UserName = "Dispatcher0",
                        Email = "sveta@taxi.ru",
                        PersonalName = "Светлана"
                    };

                    string password0 = "Qwaszx@3";
                    var chkUser0 = UserManager.Create(dispatcherUser0, password0);

                    if (chkUser0.Succeeded)
                    {
                        UserManager.AddToRole(dispatcherUser0.Id, "Диспетчер");
                    }

                    var dispatcherUser1 = new ApplicationUser()
                    {
                        UserName = "Dispatcher1",
                        Email = "julie@taxi.ru",
                        PersonalName = "Юлия"
                    };

                    string password1 = "Qwaszx@3";
                    var chkUser1 = UserManager.Create(dispatcherUser1, password1);

                    if (chkUser1.Succeeded)
                    {
                        UserManager.AddToRole(dispatcherUser1.Id, "Диспетчер");
                    }
                }

                //Если нет роли Водитель - создаем эту роль и пользователей в ней
                if (!roleManager.RoleExists("Водитель"))
                {
                    var role = new IdentityRole();
                    role.Name = "Водитель";
                    roleManager.Create(role);

                    var driverUser0 = new ApplicationUser()
                    {
                        UserName = "Driver0",
                        Email = "ivan@taxi.ru",
                        PersonalName = "Иван"
                    };

                    string password0 = "Qwaszx@3";
                    var chkUser0 = UserManager.Create(driverUser0, password0);

                    if (chkUser0.Succeeded)
                    {
                        UserManager.AddToRole(driverUser0.Id, "Водитель");
                    }

                    var driverUser1 = new ApplicationUser()
                    {
                        UserName = "Driver1",
                        Email = "ibrahim@taxi.ru",
                        PersonalName = "Ибрагим"
                    };

                    string password1 = "Qwaszx@3";
                    var chkUser1 = UserManager.Create(driverUser1, password1);

                    if (chkUser1.Succeeded)
                    {
                        UserManager.AddToRole(driverUser1.Id, "Водитель");
                    }
                }
            }
        }
    }
}
