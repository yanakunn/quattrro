using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Project_AkmalRentals.Models;

namespace Project_AkmalRentals.Controllers
{
    public class PaylandlordController : Controller
    {
        private db_akmalrentalsEntities db = new db_akmalrentalsEntities();

        // GET: Paylandlord
        public ActionResult Index()
        {
            string message = TempData["Message"] as string;

            // Check if a message exists and pass it to the view
            if (!string.IsNullOrEmpty(message))
            {
                ViewBag.Message = message;
            }

            var tb_paylandlord = db.tb_paylandlord
                .OrderByDescending(pl => pl.pl_Date)
                .Include(t => t.tb_category).Include(t => t.tb_landlord);
            return View(tb_paylandlord.ToList());
        }

        // GET: Paylandlord/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_paylandlord tb_paylandlord = db.tb_paylandlord.Find(id);
            if (tb_paylandlord == null)
            {
                return HttpNotFound();
            }
            return View(tb_paylandlord);
        }

        // GET: Paylandlord/Create
        public ActionResult Create()
        {
            ViewBag.pl_Category = new SelectList(db.tb_category, "cat_id", "cat_desc");
            ViewBag.pl_LandlordId = new SelectList(db.tb_landlord, "l_id", "l_name");
            return View();
        }

        // POST: Paylandlord/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "pl_id,pl_Date,pl_Category,pl_Payment,pl_LandlordId,pl_Amount,pl_Receipt")] tb_paylandlord tb_paylandlord, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                if (tb_paylandlord.pl_Amount > 0)
                {
                    if (file != null && file.ContentLength > 0)
                    {
                        string _FileName = Path.GetFileName(file.FileName);
                        string _path = Path.Combine(Server.MapPath("~/Receipt"), _FileName);
                        file.SaveAs(_path);
                        tb_paylandlord.pl_Receipt = _FileName;
                    }

                    db.tb_paylandlord.Add(tb_paylandlord);

                    try
                    {
                        db.SaveChanges();

                        var landlord = db.tb_landlord.FirstOrDefault(x => x.l_id == tb_paylandlord.pl_LandlordId);

                        // For Expense 
                        var expense = new tb_expenses
                        {
                            e_Type = "Expenses",
                            e_Date = tb_paylandlord.pl_Date,
                            e_Category = tb_paylandlord.pl_Category,
                            e_Payment = tb_paylandlord.pl_Payment,
                            e_Detail = landlord.l_name,
                            e_Amount = tb_paylandlord.pl_Amount,
                            e_Receipt = tb_paylandlord.pl_Receipt ?? String.Empty
                        };

                        // Add the expense to the database context
                        db.tb_expenses.Add(expense);
                        db.SaveChanges();
                        TempData["Message"] = "Landlord payment successfully recorded";
                        return RedirectToAction("Index");
                    }
                    catch (DbEntityValidationException ex)
                    {
                        foreach (var validationErrors in ex.EntityValidationErrors)
                        {
                            foreach (var validationError in validationErrors.ValidationErrors)
                            {
                                Console.WriteLine($"Property: {validationError.PropertyName} Error: {validationError.ErrorMessage}");
                            }
                        }
                        throw;
                    }
                }
                else
                {
                    ViewBag.Error = "Invalid Amount";
                    ViewBag.p_Category = new SelectList(db.tb_category, "cat_id", "cat_desc", tb_paylandlord.pl_Category);
                    return View("Create", tb_paylandlord);
                }
            }

            ViewBag.pl_Category = new SelectList(db.tb_category, "cat_id", "cat_desc", tb_paylandlord.pl_Category);
            ViewBag.pl_LandlordId = new SelectList(db.tb_landlord, "l_id", "l_name", tb_paylandlord.pl_LandlordId);
            return View(tb_paylandlord);
        }


        // GET: Paylandlord/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_paylandlord tb_paylandlord = db.tb_paylandlord.Find(id);
            if (tb_paylandlord == null)
            {
                return HttpNotFound();
            }
            ViewBag.pl_Category = new SelectList(db.tb_category, "cat_id", "cat_desc", tb_paylandlord.pl_Category);
            ViewBag.pl_LandlordId = new SelectList(db.tb_landlord, "l_id", "l_name", tb_paylandlord.pl_LandlordId);
            return View(tb_paylandlord);
        }

        // POST: Paylandlord/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "pl_id,pl_Date,pl_Category,pl_Payment,pl_LandlordId,pl_Amount,pl_Receipt")] tb_paylandlord tb_paylandlord)
        {
            if (ModelState.IsValid)
            {
                if (tb_paylandlord.pl_Amount > 0)
                {
                    db.Entry(tb_paylandlord).State = EntityState.Modified;
                    db.SaveChanges();
                    TempData["Message"] = "Payment record has been saved";
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Error = "Invalid Amount";
                    ViewBag.e_Category = new SelectList(db.tb_category, "cat_id", "cat_desc", tb_paylandlord.pl_Category);
                    return View(tb_paylandlord);
                }
            }
            ViewBag.pl_Category = new SelectList(db.tb_category, "cat_id", "cat_desc", tb_paylandlord.pl_Category);
            ViewBag.pl_LandlordId = new SelectList(db.tb_landlord, "l_id", "l_name", tb_paylandlord.pl_LandlordId);
            return View(tb_paylandlord);
        }

        // GET: Paylandlord/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_paylandlord tb_paylandlord = db.tb_paylandlord.Find(id);
            if (tb_paylandlord == null)
            {
                return HttpNotFound();
            }
            return View(tb_paylandlord);
        }

        // POST: Paylandlord/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tb_paylandlord tb_paylandlord = db.tb_paylandlord.Find(id);
            db.tb_paylandlord.Remove(tb_paylandlord);
            db.SaveChanges();
            TempData["Message"] = "Payment record successfully deleted";
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
