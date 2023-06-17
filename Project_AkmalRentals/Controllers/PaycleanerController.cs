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
    public class PaycleanerController : Controller
    {
        private db_akmalrentalsEntities db = new db_akmalrentalsEntities();

        // GET: Paycleaner
        public ActionResult Index()
        {
            var tb_paycleaner = db.tb_paycleaner.Include(t => t.tb_cleaner);
            return View(tb_paycleaner.ToList());
        }

        // GET: Paycleaner/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_paycleaner tb_paycleaner = db.tb_paycleaner.Find(id);
            if (tb_paycleaner == null)
            {
                return HttpNotFound();
            }
            return View(tb_paycleaner);
        }

        // GET: Paycleaner/Create
        public ActionResult Create()
        {
            ViewBag.pc_CleanerId = new SelectList(db.tb_cleaner, "c_id", "c_name");
            // Retrieve the available categories for the dropdown list
            var categories = new SelectList(db.tb_category, "cat_id", "cat_desc");
            ViewBag.pc_Category = categories;

            return View();
        }

        // POST: Paycleaner/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "pc_id,pc_Date,pc_Category,pc_Payment,pc_CleanerId,pc_Amount,pc_Receipt")] tb_paycleaner tb_paycleaner, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                if (file != null && file.ContentLength > 0)
                {
                    string _FileName = Path.GetFileName(file.FileName);
                    string _path = Path.Combine(Server.MapPath("~/Receipt"), _FileName);
                    file.SaveAs(_path);
                    tb_paycleaner.pc_Receipt = _FileName;
                }
                db.tb_paycleaner.Add(tb_paycleaner);
                db.SaveChanges();

                var cleaner = db.tb_cleaner.Where(x => x.c_id == tb_paycleaner.pc_CleanerId).FirstOrDefault();

                // For Expense 
                var expense = new tb_expenses
                {
                    e_Type = "Expenses",
                    e_Date = tb_paycleaner.pc_Date,
                    e_Category = tb_paycleaner.pc_Category,
                    e_Payment = tb_paycleaner.pc_Payment,
                    e_Detail = cleaner.c_name,
                    e_Amount = tb_paycleaner.pc_Amount,
                    e_Receipt = tb_paycleaner.pc_Receipt ?? String.Empty
                };

                // Add the expense to the database context
                db.tb_expenses.Add(expense);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.pc_CleanerId = new SelectList(db.tb_cleaner, "c_id", "c_name", tb_paycleaner.pc_CleanerId);
            return View(tb_paycleaner);
        }

        // GET: Paycleaner/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_paycleaner tb_paycleaner = db.tb_paycleaner.Find(id);
            if (tb_paycleaner == null)
            {
                return HttpNotFound();
            }
            ViewBag.pc_CleanerId = new SelectList(db.tb_cleaner, "c_id", "c_name", tb_paycleaner.pc_CleanerId);
            return View(tb_paycleaner);
        }

        // POST: Paycleaner/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "pc_id,pc_Date,pc_Category,pc_Payment,pc_CleanerId,pc_Amount,pc_Receipt")] tb_paycleaner tb_paycleaner)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tb_paycleaner).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.pc_CleanerId = new SelectList(db.tb_cleaner, "c_id", "c_name", tb_paycleaner.pc_CleanerId);
            return View(tb_paycleaner);
        }

        // GET: Paycleaner/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_paycleaner tb_paycleaner = db.tb_paycleaner.Find(id);
            if (tb_paycleaner == null)
            {
                return HttpNotFound();
            }
            return View(tb_paycleaner);
        }

        // POST: Paycleaner/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tb_paycleaner tb_paycleaner = db.tb_paycleaner.Find(id);
            db.tb_paycleaner.Remove(tb_paycleaner);
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
