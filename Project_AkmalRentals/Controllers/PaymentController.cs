using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Project_AkmalRentals.Models;

namespace Project_AkmalRentals.Controllers
{
    public class PaymentController : Controller
    {
        private db_akmalrentalsEntities db = new db_akmalrentalsEntities();

        // GET: Payment
        public ActionResult Index()
        {
            var tb_payment = db.tb_payment.Include(t => t.tb_tenant);
            return View(tb_payment.ToList());
        }

        // GET: Payment/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_payment tb_payment = db.tb_payment.Find(id);
            if (tb_payment == null)
            {
                return HttpNotFound();
            }

            ViewBag.p_Category = new SelectList(db.tb_category, "cat_id", "cat_desc", tb_payment.p_Category);
            return View(tb_payment);
        }

        // GET: Payment/Create
        public ActionResult Create()
        {
            ViewBag.p_TenantId = new SelectList(db.tb_tenant, "t_ic", "t_name");

            // Retrieve the available categories for the dropdown list
            var categories = new SelectList(db.tb_category, "cat_id", "cat_desc");
            ViewBag.p_Category = categories;

            return View();
        }

        // POST: Payment/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "p_id,p_Date,p_Category,p_Payment,p_TenantId,p_Amount,p_Receipt")] tb_payment tb_payment, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                if (file != null && file.ContentLength > 0)
                {
                    string _FileName = Path.GetFileName(file.FileName);
                    string _path = Path.Combine(Server.MapPath("~/Receipt"), _FileName);
                    file.SaveAs(_path);
                    tb_payment.p_Receipt = _FileName;
                }
                db.tb_payment.Add(tb_payment);
                db.SaveChanges();

                var tenant = db.tb_tenant.Where(x=>x.t_ic == tb_payment.p_TenantId).FirstOrDefault();

                // For Expense 
                var expense = new tb_expenses
                {
                    e_Type = "Income",
                    e_Date = tb_payment.p_Date,
                    e_Category = tb_payment.p_Category, 
                    e_Payment = tb_payment.p_Payment,
                    e_Detail = tenant.t_name,
                    e_Amount = tb_payment.p_Amount,
                    e_Receipt = tb_payment.p_Receipt ?? String.Empty
                };

                // Add the expense to the database context
                db.tb_expenses.Add(expense);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            ViewBag.p_TenantId = new SelectList(db.tb_tenant, "t_ic", "t_name", tb_payment.p_TenantId);
            return View(tb_payment);
        }


        // GET: Payment/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_payment tb_payment = db.tb_payment.Find(id);
            if (tb_payment == null)
            {
                return HttpNotFound();
            }
            ViewBag.p_TenantId = new SelectList(db.tb_tenant, "t_ic", "t_name", tb_payment.p_TenantId);
            return View(tb_payment);
        }

        // POST: Payment/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "p_id,p_Date,p_Category,p_Payment,p_TenantId,p_Amount,p_Receipt")] tb_payment tb_payment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tb_payment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.p_TenantId = new SelectList(db.tb_tenant, "t_ic", "t_name", tb_payment.p_TenantId);
            return View(tb_payment);
        }

        // GET: Payment/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_payment tb_payment = db.tb_payment.Find(id);
            if (tb_payment == null)
            {
                return HttpNotFound();
            }
            return View(tb_payment);
        }

        // POST: Payment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tb_payment tb_payment = db.tb_payment.Find(id);
            db.tb_payment.Remove(tb_payment);
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
