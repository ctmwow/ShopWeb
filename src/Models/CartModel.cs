using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace src.Models
{
    public class CartModel
    {
        public string userID { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public int Amount { get; set; }
    }
}
