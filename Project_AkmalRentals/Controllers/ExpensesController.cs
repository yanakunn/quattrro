using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Dapper;
using System.IO;
using Project_AkmalRentals.Models;

namespace Project_AkmalRentals.Controllers
{
	public class ExpensesController : Controller
	{
		private db_akmalrentalsEntities db = new db_akmalrentalsEntities();

		// GET: Expenses
		// GET: Expenses
		public ActionResult Index()
		{

			var tb_expenses = db.tb_expenses
				.OrderByDescending(e => e.e_Date)
				.Include(t => t.tb_category)
				.ToList();


			return View(tb_expenses);
		}


		// GET: Expenses/Details/5
		public ActionResult Details(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			tb_expenses tb_expenses = db.tb_expenses.Find(id);
			if (tb_expenses == null)
			{
				return HttpNotFound();
			}
			return View(tb_expenses);
		}

		// GET: Expenses/Create
		public ActionResult Create()
		{
			ViewBag.e_Category = new SelectList(db.tb_category, "cat_id", "cat_desc");
			return View();
		}

		// POST: Expenses/Create
		// To protect from overposting attacks, enable the specific properties you want to bind to, for 
		// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create([Bind(Include = "e_id,e_Type,e_Date,e_Category,e_Payment,e_Detail,e_Amount,e_Receipt")] tb_expenses tb_expenses)
		{
			if (ModelState.IsValid)
			{
				db.tb_expenses.Add(tb_expenses);
				db.SaveChanges();
				return RedirectToAction("Index");
			}

			ViewBag.e_Category = new SelectList(db.tb_category, "cat_id", "cat_desc", tb_expenses.e_Category);
			return View(tb_expenses);
		}

		// GET: Expenses/Edit/5
		public ActionResult Edit(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			tb_expenses tb_expenses = db.tb_expenses.Find(id);
			if (tb_expenses == null)
			{
				return HttpNotFound();
			}
			ViewBag.e_Category = new SelectList(db.tb_category, "cat_id", "cat_desc", tb_expenses.e_Category);
			return View(tb_expenses);
		}

		// POST: Expenses/Edit/5
		// To protect from overposting attacks, enable the specific properties you want to bind to, for 
		// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit([Bind(Include = "e_id,e_Type,e_Date,e_Category,e_Payment,e_Detail,e_Amount,e_Receipt")] tb_expenses tb_expenses)
		{
			if (ModelState.IsValid)
			{
				db.Entry(tb_expenses).State = EntityState.Modified;
				db.SaveChanges();
				return RedirectToAction("Index");
			}
			ViewBag.e_Category = new SelectList(db.tb_category, "cat_id", "cat_desc", tb_expenses.e_Category);
			return View(tb_expenses);
		}

		// GET: Expenses/Delete/5
		public ActionResult Delete(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			tb_expenses tb_expenses = db.tb_expenses.Find(id);
			if (tb_expenses == null)
			{
				return HttpNotFound();
			}
			return View(tb_expenses);
		}

		// POST: Expenses/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public ActionResult DeleteConfirmed(int id)
		{
			tb_expenses tb_expenses = db.tb_expenses.Find(id);
			db.tb_expenses.Remove(tb_expenses);
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
		// GET: Expenses/Create
		public ActionResult CreateIncome()
		{
			ViewBag.e_Category = new SelectList(db.tb_category, "cat_id", "cat_desc");
			return View();
		}
		// Other actions...
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult CreateIncome([Bind(Include = "e_id,e_Type,e_Date,e_Category,e_Payment,e_Detail,e_Amount,e_Receipt")] tb_expenses tb_expenses, HttpPostedFileBase file)
		{
			if (ModelState.IsValid)
			{
				if (tb_expenses.e_Amount > 0)
				{
					tb_expenses.e_Type = "Income"; // Set the e_Type property to "Income"
												   // Save upload file in File folder
					if (file != null && file.ContentLength > 0)
					{
						string _FileName = Path.GetFileName(file.FileName);
						string _path = Path.Combine(Server.MapPath("~/Receipt"), _FileName);
						file.SaveAs(_path);
						tb_expenses.e_Receipt = _FileName;
					}
					db.tb_expenses.Add(tb_expenses);
					db.SaveChanges();
					return RedirectToAction("Index");
				}
				else
				{
					ViewBag.Error = "Amount cannot be negative";
					ViewBag.e_Category = new SelectList(db.tb_category, "cat_id", "cat_desc", tb_expenses.e_Category);
					return View(tb_expenses);
				}
			}

			ViewBag.e_Category = new SelectList(db.tb_category, "cat_id", "cat_desc", tb_expenses.e_Category);
			return View(tb_expenses);
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult CreateExpense([Bind(Include = "e_id,e_Type,e_Date,e_Category,e_Payment,e_Detail,e_Amount,e_Receipt")] tb_expenses tb_expenses, HttpPostedFileBase file)
		{
			if (ModelState.IsValid)
			{

				if (tb_expenses.e_Amount > 0)
				{
					tb_expenses.e_Type = "Expenses"; // Set the e_Type property to "Expenses"
													 // Save upload file in File folder
					if (file != null && file.ContentLength > 0)
					{
						string _FileName = Path.GetFileName(file.FileName);
						string _path = Path.Combine(Server.MapPath("~/Receipt"), _FileName);
						file.SaveAs(_path);
						tb_expenses.e_Receipt = _FileName;
					}
					db.tb_expenses.Add(tb_expenses);
					db.SaveChanges();
					return RedirectToAction("Index");
				}
				else
				{
					ViewBag.Error = "Amount cannot be negative";
					ViewBag.e_Category = new SelectList(db.tb_category, "cat_id", "cat_desc", tb_expenses.e_Category);
					return View(tb_expenses);
				}
			}

			ViewBag.e_Category = new SelectList(db.tb_category, "cat_id", "cat_desc", tb_expenses.e_Category);
			return View(tb_expenses);
		}


	}
}