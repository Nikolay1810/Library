using Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Library.Controllers.Users
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Users()
        {
            LibraryContext dbContext = new LibraryContext();
            List<User> usersList = dbContext.getUsers();
            return View(usersList);
        }
        public ActionResult CreateUser()
        {
            return View();
        }

        [HttpPost]
        public string CreateUser(string args) {
            var jsSerializer = new JavaScriptSerializer();
            var user = jsSerializer.Deserialize<User>(args);

            LibraryContext dbContext = new LibraryContext();

            return jsSerializer.Serialize(dbContext.CreateNewUser(user));
        }
    }
}