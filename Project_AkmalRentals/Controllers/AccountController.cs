using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Project_AkmalRentals.Models;

namespace Project_AkmalRentals.Controllers
{
    public class AccountController : Controller
    {
        private db_akmalrentalsEntities db = new db_akmalrentalsEntities();

        // GET: Account
        public ActionResult Index()
        {
            var tb_user = db.tb_user.Include(t => t.tb_role).Include(t => t.tb_investor);
            return View(tb_user.ToList());
        }

        // GET: Account/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_user tb_user = db.tb_user.Find(id);
            if (tb_user == null)
            {
                return HttpNotFound();
            }
            return View(tb_user);
        }

        // GET: Account/Create
        public ActionResult Create()
        {
            ViewBag.u_role = new SelectList(db.tb_role, "role_id", "role_name");
            ViewBag.u_id = new SelectList(db.tb_investor, "i_id", "i_id");
            return View();
        }

        // POST: Account/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "u_id,u_email,u_pwd,u_name,u_role")] tb_user tb_user)
        {
            if (ModelState.IsValid)
            {
                db.tb_user.Add(tb_user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.u_role = new SelectList(db.tb_role, "role_id", "role_name", tb_user.u_role);
            ViewBag.u_id = new SelectList(db.tb_investor, "i_id", "i_id", tb_user.u_id);
            return View(tb_user);
        }

        // GET: Account/Edit/5
        public ActionResult Profile(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_user tb_user = db.tb_user.Find(id);
            if (tb_user == null)
            {
                return HttpNotFound();
            }
            ViewBag.u_role = new SelectList(db.tb_role, "role_id", "role_name", tb_user.u_role);
            ViewBag.u_id = new SelectList(db.tb_investor, "i_id", "i_id", tb_user.u_id);
            return View(tb_user);
        }

        // POST: Account/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Profile([Bind(Include = "u_id,u_email,u_pwd,u_name,u_role")] tb_user tb_user)
        {
            if (ModelState.IsValid)
            {
                var userCheck = db.tb_user.Any(x => x.u_email == tb_user.u_email);
                if (userCheck)
                {
                    ViewBag.m = "Invalid email address!";
                }
                else
                {

                    db.Entry(tb_user).State = EntityState.Modified;
                    db.SaveChanges();
                    ViewBag.ms = "Saved!";
                    return RedirectToAction("Profile");
                }
                
            }
            ViewBag.u_role = new SelectList(db.tb_role, "role_id", "role_name", tb_user.u_role);
            ViewBag.u_id = new SelectList(db.tb_investor, "i_id", "i_id", tb_user.u_id);
            return View(tb_user);
        }

        // GET: Account/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_user tb_user = db.tb_user.Find(id);
            if (tb_user == null)
            {
                return HttpNotFound();
            }
            return View(tb_user);
        }

        // POST: Account/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tb_user tb_user = db.tb_user.Find(id);
            db.tb_user.Remove(tb_user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
