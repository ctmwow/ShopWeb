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
        public ActionResult Index() => View(this.products);
        private readonly String connectionString;
        private List<Models.ShopModel> products;
        private List<Models.CartModel> ShopList;

        // Specific Product Page
        public ActionResult product(string id)
        {
            var thisProduct = this.products.SingleOrDefault(x => x.Id == id);

            if (thisProduct == null)
            {
                return NotFound("There is no such product.");
            }

            return View(thisProduct);
        }

        public ActionResult Cart()
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
        public ActionResult Cart(Models.CartModel p)
        {
            using (var connection = new MySqlConnection(this.connectionString))
            {
                // See if product already is in cart
                var exists = connection.QuerySingleOrDefault<CartModel>("SELECT name FROM cart WHERE name = @product LIMIT 1",
                  new { product = p.Name });

                if (exists != null)
                { // Update Amount
                    connection.Query<CartModel>("UPDATE cart SET amount = amount +1 WHERE name = @product",
                   new { product = p.Name });
                }

                else
                { // Add Product into Cart
                    connection.Query<CartModel>("INSERT INTO cart(`UserID`, `Name`, `Price`) VALUES(@user, @item, @p)",
                        new { user = OnlineList.User, item = p.Name, p = p.Price });
                }

                // Reset List with updated content
                this.ShopList = connection.Query<CartModel>("SELECT * FROM cart").ToList();

                // Increase Users Amount of products counter
                OnlineList.UpdateAmount();
            }

            return View(ShopList);
        }

        // Checkout View
        [HttpPost]
        public ActionResult Checkout(int checkoutAmount)
        {
            using (var connection = new MySqlConnection(this.connectionString))
            {
                this.ShopList = connection.Query<CartModel>("SELECT * FROM cart").ToList();
            }
            return View(ShopList);
        }

        // Confirm Page - Cleart Cart
        public ActionResult confirm()
        {
            using (var connection = new MySqlConnection(this.connectionString))
            {
                connection.Query<CartModel>("DELETE FROM cart WHERE userID = @n",
                    new { n = OnlineList.User });

                // Reset List with updated content 
                this.ShopList = connection.Query<CartModel>("SELECT * FROM cart").ToList();
            }
            return View();
        }
    }
}
