using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Library.Models
{
    public class History
    {
        [Key]
        public int Id { get; set; }
        public int BookId { get; set; }

        [Display(Name = "Book")]
        public string BookName{ get; set; }

        public int UserId { get; set; }

        [Display(Name = "User")]
        public string UserName { get; set; }

        [Display(Name = "Date of issue")]
        public DateTime DateOfIssue { get; set; }

        [Display(Name = "Return date")]
        public DateTime DateReturn { get; set; }

    }
}