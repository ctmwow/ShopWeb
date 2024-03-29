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
    public static class OnlineList
    {
        private static int products = 0;
        public static string User { get; set; }

        // Get amount of products in Cart
        public static int CartAmount()
        {
            return products;
        }

        // Update Product List (+)
        public static int UpdateAmount()
        {
            return products = products+1;
        }
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
                if (UserExists(user.UserName))
                {
                    OnlineList.User = user.UserName; // Add to OnlineList
                    return RedirectToAction("Index", "Home");
                }

                return NotFound("Cannot find you, sexy... :(");
            }

            return View(user);
        }

        // Function to see if Name exists in Database
        public bool UserExists(string _username)
        {
            var available = this.users.SingleOrDefault(x => x.UserName == _username);
            return available != null;
        }

        // Registration
        [HttpPost]
        public ActionResult Register(Models.UserModel user)
        {
            if (ModelState.IsValid)
            {
                if (!UserExists(user.UserName))
                {
                    using (var connection = new MySqlConnection(this.connectionString))
                    {
                        // Add user to database
                        connection.Query<UserModel>("INSERT INTO Customers(`money`, `userName`) VALUES(0, @u)",
                            new { u = user.UserName });

                        // Update MemberList
                        this.users = connection.Query<UserModel>("SELECT * FROM customers").ToList();

                        // Add current user to an OnlineList
                        OnlineList.User = user.UserName;

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
