using Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
namespace Library.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            LibraryContext dbContext = new LibraryContext();
            //List<Book> bookList = dbContext.getBooks();
            Book book = new Book();
            return View(book);
        }

        [HttpPost]
        public string GetAllBooks()
        {
            LibraryContext dbContext = new LibraryContext();
            List<Book> bookList = dbContext.getBooks();

            var jsSerializer = new JavaScriptSerializer();
            return jsSerializer.Serialize(bookList);
        }

        [HttpPost]
        public string GetOtherBooks(string args)
        {
            var jsSerializer = new JavaScriptSerializer();
            List<Book> sortedBooksList = new List<Book>();

            try
            {

                LibraryContext dbContext = new LibraryContext();
                List<Book> bookList = dbContext.getBooks();
                var Id = int.Parse(args);

                switch (Id)
                {
                    case 1:
                        foreach (var book in bookList)
                        {
                            if(book.Quantity > 0)
                            {
                                sortedBooksList.Add(book);
                            }
                        }
                        break;
                    case 2:
                        var histories = dbContext.getListHistories();
                        foreach (var history in histories)
                        {
                            sortedBooksList.Add(bookList.FirstOrDefault(u => u.Id == history.BookId));
                        }
                        break;
                    default:
                        break;
                }

                
            }
            catch(Exception ex)
            {

            }
            return jsSerializer.Serialize(sortedBooksList.ToList().Distinct());
        }

        public ActionResult Create() {
            return View();
        }

        public ActionResult Edit()
        {
            return View();
        }

        [HttpPost]
        public string GetAuthorList()
        {
            LibraryContext dbContext = new LibraryContext();
            List<Author> authotsList = dbContext.getAuthors();
            var jsSerializer = new JavaScriptSerializer();
            return jsSerializer.Serialize(authotsList);
        }

        [HttpPost]
        public string AddNewBooks(string args) {
            var argsBook = args.Split('&').FirstOrDefault();
            var argsListAuthor = args.Split('&').LastOrDefault();
            var jsSerializer = new JavaScriptSerializer();
            var book = jsSerializer.Deserialize<BookRequest>(argsBook);
            var authorList = jsSerializer.Deserialize<List<Author>>(argsListAuthor);

            LibraryContext dbContext = new LibraryContext();
            dbContext.addBooks(book, authorList);

            return EmptyJsonResult.EmptyJsonResult.emptyJsonResult;
        }

        [HttpPost]
        public string DeleteBook(string args)
        {
            var jsSerializer = new JavaScriptSerializer();
            var book = jsSerializer.Deserialize<BookRequest>(args);

            LibraryContext dbContext = new LibraryContext();
            dbContext.deleteBook(book.Id);
            return EmptyJsonResult.EmptyJsonResult.emptyJsonResult;
        }

        [HttpPost]
        public string GetBookById(string args) {
            var jsSerializer = new JavaScriptSerializer();
            var book = jsSerializer.Deserialize<BookRequest>(args);

            LibraryContext dbContext = new LibraryContext();


            return jsSerializer.Serialize(dbContext.getBookById(book.Id));
        }

        [HttpPost]
        public string GetAutherForBook(string args) {
            var jsSerializer = new JavaScriptSerializer();
            var book = jsSerializer.Deserialize<BookRequest>(args);

            LibraryContext dbContext = new LibraryContext();


            return jsSerializer.Serialize(dbContext.GetAutherForBookById(book.Id));
        }

        [HttpPost]
        public string DeleteAuthorFromBook(string args) {
            var jsSerializer = new JavaScriptSerializer();
            var author = jsSerializer.Deserialize<Author>(args);

            LibraryContext dbContext = new LibraryContext();
            dbContext.deleteAuthorFromBookById(author.Id);

            return EmptyJsonResult.EmptyJsonResult.emptyJsonResult;
        }

        [HttpPost]
        public string UpdateBook(string args) {
            var argsBook = args.Split('&').FirstOrDefault();
            var argsListAuthor = args.Split('&').LastOrDefault();
            var jsSerializer = new JavaScriptSerializer();
            var book = jsSerializer.Deserialize<Book>(argsBook);
            var authorList = jsSerializer.Deserialize<List<Author>>(argsListAuthor);

            LibraryContext dbContext = new LibraryContext();
            dbContext.UpdateBookAndAuthor(book, authorList);

            return EmptyJsonResult.EmptyJsonResult.emptyJsonResult;
        }

        [HttpPost]
        public string SendMail() {
            LibraryContext dbContext = new LibraryContext();

            List<History> historiesList = dbContext.getListHistories();
            List<Book> booksList = new List<Book>();
            List<User> userList = new List<User>();
            foreach (var history in historiesList)
            {
                booksList.Add(dbContext.getBookById(history.BookId));
                userList.Add(dbContext.getUserById(history.UserId));
            }

            IEnumerable<Book> filteredBookList = booksList.GroupBy(book => book.Id).Select(group => group.First());
            IEnumerable<User> filteredUserList = userList.GroupBy(user => user.Id).Select(group => group.First());

            Email mail = new Email();

            foreach (var user in filteredUserList)
            {
                List<Book> booksByUser = new List<Book>();

                var historiesByUser = historiesList.Where(u => u.UserId == user.Id).ToList();
                foreach (var history in historiesByUser)
                {
                    booksByUser.Add(filteredBookList.FirstOrDefault(u => u.Id == history.BookId));

                }
                mail.Send(user.Mail, booksByUser);
            }


            return EmptyJsonResult.EmptyJsonResult.emptyJsonResult;
        }
    }
}