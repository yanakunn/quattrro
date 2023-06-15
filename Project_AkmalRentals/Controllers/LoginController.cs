using Project_AkmalRentals.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Project_AkmalRentals.Controllers
{
    public class LoginController : Controller
    {
        db_akmalrentalsEntities db = new db_akmalrentalsEntities();

        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(tb_user log)
        {
            if (ModelState.IsValid)
            {
                var user = db.tb_user.Where(x => x.u_email == log.u_email && x.u_pwd == log.u_pwd).FirstOrDefault();
                if (user != null)
                {
                    Session["uid"] = user.u_id;
                    Session["name"] = user.u_name;
                    Session["role"] = user.tb_role.role_name;

                    string role = (string)Session["role"];

                    if (role == "Admin")
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        return RedirectToAction("InvDashboard", "Investor");
                    }
                    
                }
                else
                {
                    ModelState.AddModelError("", "Email or Password is incorrect.");

                }
            }

            return View();
        }

        public ActionResult Logout()
        {
            //Session.Clear();
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Login");
        }
    }
}