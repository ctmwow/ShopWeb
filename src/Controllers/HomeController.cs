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

        // About Page
        public IActionResult About()
        {
            ViewData["Message"] = "About ShopWeb";
            return View();
        }

        // Contact Page
        public IActionResult Contact()
        {
            ViewData["Message"] = "Contact Mr. Spaghet!";
            return View();
        }


        // Login Functionality
        [HttpPost]
        public ActionResult Login(Models.UserModel user)
        {
            if (ModelState.IsValid)
            {
                if (IsAvailable(user.UserName))
                {
                    onlineList.user = user.UserName; // Add to OnlineList
                    return RedirectToAction("Index", "Home");
                }

                return NotFound("Cannot find you, sexy... :(");
            }
            return View(user);
        }

        // Function to see if Name exists in Database
        public bool IsAvailable(string _username)
        {
            var available = this.users.SingleOrDefault(x => x.UserName == _username);
            return available == null;
        }

        // Registration
        [HttpPost]
        public ActionResult Register(Models.UserModel user)
        {
            if (ModelState.IsValid)
            {
                if (IsAvailable(user.UserName))
                {
                    using (var connection = new MySqlConnection(this.connectionString))
                    {
                        // Add user to database
                        connection.Query<UserModel>("INSERT INTO Customers(`money`, `userName`) VALUES(0, @u)",
                            new { u = user.UserName });

                        // Update MemberList
                        this.users = connection.Query<UserModel>("SELECT * FROM customers").ToList();

                        // Add current user to an OnlineList
                        onlineList.user = user.UserName;

                        return RedirectToAction("Index", "Home");
                    }
                }

                return NotFound("Your name is taken dude.. \n\n Find your own way back.");
            }

            return View(user);
        }

        // Fetch MemberList from Database
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
