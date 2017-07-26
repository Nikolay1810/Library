using Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Library.Controllers.Histories
{
    public class HistoryController : Controller
    {
        // GET: History
        public ActionResult CreateHistory()
        {
            return View();
        }

        public ActionResult Histories()
        {
            LibraryContext dbContext = new LibraryContext();
            List<History> historiesList = dbContext.getListHistories();
            return View(historiesList);
        }

        [HttpPost]
        public string GetListBooks()
        {
            LibraryContext dbContext = new LibraryContext();

            List<Book> bookList = dbContext.getBooks();
            List<Book> sortedBookList = new List<Book>();

            foreach (var book in bookList)
            {
                if (book.Quantity > 0)
                {
                    sortedBookList.Add(book);
                }
            }

            var jsSerializer = new JavaScriptSerializer();
            return jsSerializer.Serialize(sortedBookList);
        }

        [HttpPost]
        public string GetLstUsers()
        {
            LibraryContext dbContext = new LibraryContext();

            var jsSerializer = new JavaScriptSerializer();
            return jsSerializer.Serialize(dbContext.getUsers());
        }

        [HttpPost]
        public string GetDateOfIssue()
        {
            var DataOfIssue = new DateTime();
            DataOfIssue = DateTime.Now;

            var jsSerializer = new JavaScriptSerializer();
            return jsSerializer.Serialize(DataOfIssue.ToString("MM/dd/yy"));
        }

        [HttpPost]
        public string AddNewHistory(string args)
        {
            var jsSerializer = new JavaScriptSerializer();
            var newHistory = jsSerializer.Deserialize<History>(args);

            LibraryContext dbContext = new LibraryContext();
            var book = dbContext.getBookById(newHistory.BookId);
            if (book != null)
            {
                if (book.Quantity > 0)
                {
                    dbContext.AddHistory(book.Id, book.Quantity, newHistory);
                }
            }


            return EmptyJsonResult.EmptyJsonResult.emptyJsonResult;
        }
    }
}