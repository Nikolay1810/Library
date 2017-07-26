using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Library.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "First name user")]
        public string FirstName { get; set; }

        [Display(Name = "Last name user")]
        public string LastName { get; set; }

        [Display(Name = "Email")]
        public string Mail { get; set; }

        [Display(Name = "Phone number")]
        public long PhoneNumber { get; set; }
    }
}