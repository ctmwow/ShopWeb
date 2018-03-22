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
        private List<Models.CartModel> ShopList;

        // SubPages
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


        // Database Fetch
        public ShopController(IConfiguration configuration)
        {
            this.connectionString = configuration.GetConnectionString("ConnectionString");
            using (var connection = new MySqlConnection(this.connectionString))
            {
                this.products = connection.Query<ShopModel>("SELECT * FROM products").ToList();
            }
        }

        // Action Handlers
        [HttpPost]
        public ActionResult cart(Models.CartModel p)
        {
            using (var connection = new MySqlConnection(this.connectionString))
            {
                connection.Query<CartModel>("INSERT INTO cart(`UserID`, `Name`, `Price`) VALUES(@user, @item, @p)",
                    new { user = onlineList.user, item = p.Name, p = p.Price}
                    );
                this.ShopList = connection.Query<CartModel>("SELECT * FROM cart").ToList();
            }


            return View(ShopList);
        }
    }
}
