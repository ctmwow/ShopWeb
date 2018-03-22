using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using src.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Dapper;
using MySql.Data.MySqlClient;

namespace src.Controllers
{
    public class ShopController : Controller
    {
        // Actions
        public ActionResult Index() => View(this.products);
        private readonly String connectionString;
        private List<Models.ShopModel> products;
        private List<Models.ShopModel> ShopList;

        public ActionResult product(string id)
        {
            var thisProduct = this.products.SingleOrDefault(x => x.Id == id);
    
            if (thisProduct == null)
            {
                return NotFound("There is no such product.");
            }

            return View(thisProduct);
        }

        public ActionResult cart()
        {
            return View(ShopList);
        }

        public ShopController(IConfiguration configuration)
        {
            this.connectionString = configuration.GetConnectionString("ConnectionString");
            using (var connection = new MySqlConnection(this.connectionString))
            {
                this.products = connection.Query<ShopModel>("SELECT * FROM products").ToList();
                this.ShopList = connection.Query<ShopModel>("SELECT * FROM cart").ToList();
            }
        }
    }
}
