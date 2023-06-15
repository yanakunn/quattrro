using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Project_AkmalRentals.Models;
using Project_AkmalRentals.ViewModel;

namespace Project_AkmalRentals.Controllers
{
	public class ServicesController : Controller
	{
		private readonly db_akmalrentalsEntities db = new db_akmalrentalsEntities();

		public ActionResult Service()
		{

			var tables = new Services
			{
				Cleaners = db.tb_cleaner.ToList(),
				Landlord = db.tb_landlord.ToList()
			};

			return View(tables);
		}

		public ActionResult Details(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			tb_cleaner Cleaners = db.tb_cleaner.Find(id);
			if (Cleaners == null)
			{
				return HttpNotFound();
			}
			return View(Cleaners);
		}

		// GET: Services/Create
		public ActionResult Create()
		{
			ViewBag.c_location = new SelectList(db.tb_floor, "y_id", "y_location");
			return View();
		}

		// POST: Services/Create
		// To protect from overposting attacks, enable the specific properties you want to bind to, for 
		// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create([Bind(Include = "c_id,c_name,c_company,c_location")] tb_cleaner Cleaners)
		{
			if (ModelState.IsValid)
			{
				db.tb_cleaner.Add(Cleaners);
				db.SaveChanges();
				return RedirectToAction("Service");
			}

			ViewBag.c_location = new SelectList(db.tb_floor, "y_id", "y_location", Cleaners.c_location);
			return View(Cleaners);
		}

		// GET: Cleaner/Edit/5
		public ActionResult Edit(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			tb_cleaner Cleaners = db.tb_cleaner.Find(id);
			if (Cleaners == null)
			{
				return HttpNotFound();
			}
			ViewBag.c_location = new SelectList(db.tb_floor, "y_id", "y_location", Cleaners.c_location);
			return View(Cleaners);
		}

		// POST: Cleaner/Edit/5
		// To protect from overposting attacks, enable the specific properties you want to bind to, for 
		// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit([Bind(Include = "c_id,c_name,c_company,c_location")] tb_cleaner Cleaners)
		{
			if (ModelState.IsValid)
			{
				db.Entry(Cleaners).State = EntityState.Modified;
				db.SaveChanges();
				return RedirectToAction("Service");
			}
			ViewBag.c_location = new SelectList(db.tb_floor, "y_id", "y_location", Cleaners.c_location);
			return View(Cleaners);
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