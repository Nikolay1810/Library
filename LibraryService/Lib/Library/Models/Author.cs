using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Library.Models
{
    public class Author
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "First name author")]
        public string FirstName { get; set; }

        [Display(Name = "Last name author")]
        public string LastName { get; set; }

        public List<Book> booksList { get; set; }
    }
}