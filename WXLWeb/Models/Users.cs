using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace WXLWeb.Models
{
    public class Users
    {
        [Required(ErrorMessage = "*")]
        public string UserId { get; set; }
        [Required(ErrorMessage = "*")]
        public string Pwd { get; set; }
    }
}