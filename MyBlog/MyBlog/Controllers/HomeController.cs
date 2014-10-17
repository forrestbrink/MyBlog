using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyBlog.Controllers
{
    public class HomeController : Controller
    {

        //set up the data context for the home controller
        Models.BlogEntities db = new Models.BlogEntities();
        //
        // GET: /Home/
 
        [HttpGet]
        public ActionResult Index()
        {
            return View(db.Posts.OrderByDescending(x => x.DateCreated));
        }
        [HttpPost]
        public ActionResult AddComment(Models.Comment commentToAdd)
        {
            //make sure the comment is fully filled out
            commentToAdd.DateCreated = DateTime.Now;
            
            //add the comment to the database
            db.Comments.Add(commentToAdd);
            db.SaveChanges();

            //return RedirectToAction("Index", "Home");
            return PartialView("Comment", commentToAdd);
        }

        public ActionResult LikePost(int id)
        {
            //get the post from the database that matches the ID
            Models.Post thePostToLike = db.Posts.Find(id);
            //increment the likes by one
            thePostToLike.Likes++;
            //save changes
            db.SaveChanges();
            //we need to return the number of likes
            return Content(thePostToLike.Likes + " Likes"); 
        }

    }
}
