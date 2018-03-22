using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using src.Models;
using System.Web;
using System.Data.SqlClient;
using Dapper;
using MySql.Data.MySqlClient;
using Microsoft.Extensions.Configuration;

namespace src.Controllers
{
    public static class onlineList
    {
        public static string user { get; set; }
    }

    public class HomeController : Controller
    {
        private readonly string connectionString;
        private List<Models.UserModel> users;

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "About ShopWeb";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Contact Mr. Spaghet!";

            return View();
        }

        [HttpPost]
        public ActionResult Login(Models.UserModel user)
        {
            if (ModelState.IsValid)
            {
                if (IsValid(user.UserName))
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return NotFound("Cannot find you, sexy... :(");
                }
            }
            return View(user);
        }

        public bool IsValid(string _username)
        {
            var user = this.users.SingleOrDefault(x => x.UserName == _username);
            onlineList.user = _username;
            return user != null;
        }

        public bool IsAvailable(string _username)
        {
            var available = this.users.SingleOrDefault(x => x.UserName == _username);
            return available == null;
        }

        [HttpPost]
        public ActionResult Register(Models.UserModel user)
        {
            if (ModelState.IsValid)
            {
                if (IsAvailable(user.UserName))
                {
                    using (var connection = new MySqlConnection(this.connectionString))
                    {
                        connection.Query<UserModel>("INSERT INTO Customers(`money`, `userName`) VALUES(0, @u)",
                            new { u =  user.UserName }
                            );

                        this.users = connection.Query<UserModel>("SELECT * FROM customers").ToList();
                        onlineList.user = user.UserName;
                        return RedirectToAction("Index", "Home");
                    }
                }

                else
                {
                    return NotFound("Your name is taken dude.. \n\n Find your own way back.");
                }
            }
            return View(user);
        }

        public HomeController(IConfiguration configuration)
        {
            this.connectionString = configuration.GetConnectionString("ConnectionString");
            using (var connection = new MySqlConnection(this.connectionString))
            {
                this.users = connection.Query<UserModel>("SELECT * FROM customers").ToList();
            }
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
