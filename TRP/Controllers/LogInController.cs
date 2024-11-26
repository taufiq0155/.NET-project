using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TRP.DTOs;
using TRP.EF;

namespace TRP.Controllers
{
    public class LogInController : Controller
    {
        // GET: LogIn
        PracticeEntities db = new PracticeEntities();
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult LogIn(LogInDTO log)
        {
            var user = (from u in db.Users
                        where u.UserName.Equals(log.UserName) &&
                        u.Password.Equals(log.Password)
                        select u).SingleOrDefault();
            if (user != null)
            {
                Session["user"] = user;
                return RedirectToAction("List", "Channel");
            }
            return View();
        }

    }
}