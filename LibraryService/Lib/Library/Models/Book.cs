using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Library.Models
{
    public class Book
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "Book title")]
        public string NameBook { get; set; }
        [Display(Name = "Quantity")]
        public int Quantity { get; set; }
        [Display(Name = "Year of publication")]
        public int YearPublish { get; set; }

        public List<Author> authorList { get; set; }
    }
}