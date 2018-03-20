using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using Dapper;
using MySql.Data.MySqlClient;

namespace src.Models
{
    public class UserModel
    {
        private readonly String connectionString;
        private String user;

        [Required]
        [Display(Name = "Username")]
        public string UserName { get; set; }

        [Display(Name = "Remember on this computer")]
        public bool RememberMe { get; set; }

        public bool IsValid(string _username) 
        {
            using (var connection = new MySqlConnection(this.connectionString))
            {
                this.user = connection.Query<ShopModel>("SELECT name FROM customers WHERE name = @name",
                    new { _username }).ToString();

               return (this.user == _username) ? true : false;

            }
        }
    }
}
