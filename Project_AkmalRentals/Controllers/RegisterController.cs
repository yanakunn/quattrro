using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using Project_AkmalRentals.Models;
using Microsoft.Ajax.Utilities;

namespace Project_AkmalRentals.Controllers
{
    public class RegisterController : Controller
    {
        private db_akmalrentalsEntities db = new db_akmalrentalsEntities();
        public string email = "quattroprov@gmail.com";
        public string password = "bsuhihkgocpgybth\r\n";
        // GET: Register
        public ActionResult Index()
        {
            var tb_user = db.tb_user.Include(t => t.tb_role).Include(t => t.tb_investor);
            return View(tb_user.ToList());
        }

        // GET: Register/Create
        public ActionResult Create()
        {
            ViewBag.u_role = new SelectList(db.tb_role, "role_id", "role_name");
            ViewBag.u_id = new SelectList(db.tb_investor, "i_id", "i_id");
            return View();
        }

        // POST: Register/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "u_id,u_email,u_pwd,u_name,u_role")] tb_user tb_user)
        {
            if (ModelState.IsValid)
            {
                var userCheck = db.tb_user.Any(x => x.u_email == tb_user.u_email);

                if (userCheck)
                {
                    ViewBag.error = "Email address already registered.";
                    ViewBag.u_role = new SelectList(db.tb_role, "role_id", "role_name", tb_user.u_role);

                    return View(tb_user);
                }
                else
                {
                    tb_user.u_pwd = tb_user.u_name;
                    var role = db.tb_role.Where(x=>x.role_id == tb_user.u_role).FirstOrDefault();

                    MailMessage mail = new MailMessage(email, tb_user.u_email);
                    mail.Subject = "Akmal Rentals Registration Confirmation";
                    mail.Body = "Hi " + tb_user.u_name + ",\r\n\r\nThank you for joining AkmalRentals! You are now registered as an " +
                                role.role_name + ". Here are your credentials to sign in to our website:" +
                                "\r\n\r\nEmail: " + tb_user.u_email + "\r\nPassword: " + tb_user.u_pwd+"" +
                                "\r\n\r\n Regards,\r\n AkmalRentals Team";

                    mail.IsBodyHtml = false;
                    SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
                    smtp.Timeout = 1000000;
                    smtp.EnableSsl = true;
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    NetworkCredential nc = new NetworkCredential(email, password);
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = nc;
                    smtp.Send(mail);

                    db.tb_user.Add(tb_user);
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }

                
               
            }

            ViewBag.u_role = new SelectList(db.tb_role, "role_id", "role_name", tb_user.u_role);
            ViewBag.u_id = new SelectList(db.tb_investor, "i_id", "i_id", tb_user.u_id);
            return View(tb_user);
        }


    // GET: Register/Edit/5
    public ActionResult Edit(int? id)
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

        // POST: Register/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "u_id,u_email,u_pwd,u_name,u_role")] tb_user tb_user)
        {
            if (ModelState.IsValid)
            {
                var userCheck = db.tb_user.Any(x => x.u_email == tb_user.u_email);

                if (userCheck)
                {
                    ViewBag.error = "Email address already registered.";
                    ViewBag.u_role = new SelectList(db.tb_role, "role_id", "role_name", tb_user.u_role);

                    return View(tb_user);
                }
                else
                {
                    db.Entry(tb_user).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                
            }
            ViewBag.u_role = new SelectList(db.tb_role, "role_id", "role_name", tb_user.u_role);
            ViewBag.u_id = new SelectList(db.tb_investor, "i_id", "i_id", tb_user.u_id);
            return View(tb_user);
        }

        // GET: Register/Delete/5
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

        // POST: Register/Delete/5
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
        public ActionResult ForgotPasswordSent()
        {
            return View();
        }
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotPassword(tb_user log)
        {
            if (ModelState.IsValid)
            {
                var userCheck = db.tb_user.Any(x => x.u_email == log.u_email);
                int id = db.tb_user.Where(x => x.u_email == log.u_email).Select(x=>x.u_id).FirstOrDefault();
                if (userCheck)
                {
                    tb_user tb_user = db.tb_user.Find(id);
                    tb_user.u_pwd = tb_user.u_name.Trim();
                    MailMessage mail = new MailMessage(email, tb_user.u_email);
                    mail.Subject = "Akmal Rentals Password Reset";
                    mail.Body = "Hi " + tb_user.u_name + ",\r\n\r\nWe reset your password. Here are your credentials to sign in to our website:" +
                                "\r\n\r\nEmail: " + tb_user.u_email + "\r\nPassword: " + tb_user.u_name + "" +
                                "\r\n\r\n Regards,\r\n AkmalRentals Team";

                    mail.IsBodyHtml = false;
                    SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
                    smtp.Timeout = 1000000;
                    smtp.EnableSsl = true;
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    NetworkCredential nc = new NetworkCredential(email, password);
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = nc;
                    smtp.Send(mail);

                    db.Entry(tb_user).State = EntityState.Modified;
                    db.SaveChanges();
                    return View("ForgotPasswordSent");

                }
                else
                    
                {
                    ViewBag.error = "Invalid email address.";
                    return View(log);
                }

            }

            return View(log);
        }
    }
}
