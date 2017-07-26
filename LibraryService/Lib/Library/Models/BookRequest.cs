using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Library.Models
{
    public class BookRequest
    {
        public int Id { get; set; }
        public string NameBook { get; set; }
        public int Quantity { get; set; }
        public int YearPublish { get; set; }
        public int AuthorId { get; set; }
    }
}