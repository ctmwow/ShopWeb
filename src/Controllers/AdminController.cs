using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using Dapper;

namespace src.Controllers
{
    public class AdminController : Controller
    {
        private List<Models.ShopModel> products;
        private String connectionString;

        public IActionResult Index()
        {
            return View(products);
        }

        public IActionResult AddProduct()
        {
            return View();
        }

        // Database Fetch
        public AdminController(IConfiguration configuration)
        {
            this.connectionString = configuration.GetConnectionString("ConnectionString");
            using (var connection = new MySqlConnection(this.connectionString))
            {
                this.products = connection.Query<Models.ShopModel>("SELECT * FROM products").ToList();
            }
        }

        // Delete Product
        [HttpPost]
        public ActionResult Delete(int id)
        {
            using (var connection = new MySqlConnection(this.connectionString))
            {
                connection.Query<Models.ShopModel>("DELETE FROM `products` WHERE id = @id", new { id });
            }
            return View("Product successfully removed, gg wp");
        }
    }
}
