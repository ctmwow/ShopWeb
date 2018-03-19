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
        public ActionResult Get(string id)
        {
            var products = this.products.SingleOrDefault(x => x.Id == id);
    
            if (products == null)
            {
                return NotFound("There are no products available. Wtf?!");
            }

            return View(products);
        }

        public ShopController(IConfiguration configuration)
        {
            this.connectionString = configuration.GetConnectionString("ConnectionString");
            using (var connection = new MySqlConnection(this.connectionString))
            {
                this.products = connection.Query<ShopModel>("SELECT * FROM News").ToList();
            }
        }
    }
}
