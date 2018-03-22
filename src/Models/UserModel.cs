using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Data;
using Microsoft.Extensions.Configuration;

namespace src.Models
{
    public class UserModel
    {
        public string Id { get; set; }
        [Required]
        [Display(Name = "Username")]
        public string UserName { get; set; }

        [Display(Name = "Remember on this computer")]
        public bool RememberMe { get; set; }
    }
}
