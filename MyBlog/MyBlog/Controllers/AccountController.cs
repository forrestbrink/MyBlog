using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO; //import namespace for using path
using System.Web.Security; //import namespace for membership user

namespace MyBlog.Controllers
{
    public class AccountController : Controller
    {
        //set up my data context
        Models.BlogEntities db = new Models.BlogEntities();

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(Models.Registration registration, HttpPostedFileBase ImageURL)
        {
            if (ImageURL != null)
            {
                //save the image to our website
                string filename = Guid.NewGuid().ToString().Substring(0, 6) + ImageURL.FileName;
                //specify the path to save the file to
                string path = Path.Combine(Server.MapPath("~/content/"), filename);
                //save the file
                ImageURL.SaveAs(path);
                //update our registration object
                registration.ImageUrl = "/content/" + filename;
            }
            //create our membership user:
            Membership.CreateUser(registration.Username, registration.Password);
            //create our author object
            Models.Author author = new Models.Author();
            author.Name = registration.Name;
            author.ImageURL = registration.ImageUrl;
            author.Username = registration.Username;
            //add the author to the database
            db.Authors.Add(author);
            //save changes
            db.SaveChanges();

            //registration is complete! log-in the user
            FormsAuthentication.SetAuthCookie(registration.Username, false);

            //kick the user to the create post section
            return RedirectToAction("Index", "Post");
        }

        public ActionResult Logout()
        {
            //to logout a user, do this:
            FormsAuthentication.SignOut();
            //kick the user to the login screen 
            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Models.Login login)
        {
            if (Membership.ValidateUser(login.UserName, login.Password))
            {
                //credentials are gold, log them in
                FormsAuthentication.SetAuthCookie(login.UserName, false);
                //kick the user to the create post page
                return RedirectToAction("Index", "Post");
            }
            else
            {
                //bad password or username
                ViewBag.ErrorMessage = "Invalid username and/or password";
                return View(login);
            }
        }
    }
}
