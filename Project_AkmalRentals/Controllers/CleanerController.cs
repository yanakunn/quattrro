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
    public class CleanerController : Controller
    {
        private db_akmalrentalsEntities db = new db_akmalrentalsEntities();

        // GET: Cleaner
        public ActionResult Index()
        {
            var tb_cleaner = db.tb_cleaner.Include(t => t.tb_floor);

            return View(tb_cleaner);
        }

        // GET: Cleaner/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_cleaner tb_cleaner = db.tb_cleaner.Find(id);
            if (tb_cleaner == null)
            {
                return HttpNotFound();
            }
            return View(tb_cleaner);
        }

        // GET: Cleaner/Create
        public ActionResult Create()
        {
            ViewBag.c_location = new SelectList(db.tb_floor, "y_id", "y_location");
            return View();
        }

        // POST: Cleaner/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "c_id,c_name,c_company,c_location")] tb_cleaner tb_cleaner)
        {
            if (ModelState.IsValid)
            {
                db.tb_cleaner.Add(tb_cleaner);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.c_location = new SelectList(db.tb_floor, "y_id", "y_location", tb_cleaner.c_location);
            return View(tb_cleaner);
        }

        // GET: Cleaner/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_cleaner tb_cleaner = db.tb_cleaner.Find(id);
            if (tb_cleaner == null)
            {
                return HttpNotFound();
            }
            ViewBag.c_location = new SelectList(db.tb_floor, "y_id", "y_location", tb_cleaner.c_location);
            return View(tb_cleaner);
        }

        // POST: Cleaner/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "c_id,c_name,c_company,c_location")] tb_cleaner tb_cleaner)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tb_cleaner).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Service", "Services");
            }
            ViewBag.c_location = new SelectList(db.tb_floor, "y_id", "y_location", tb_cleaner.c_location);
            return View(tb_cleaner);
        }

        // GET: Cleaner/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_cleaner tb_cleaner = db.tb_cleaner.Find(id);
            if (tb_cleaner == null)
            {
                return HttpNotFound();
            }
            return View(tb_cleaner);
        }

        // POST: Cleaner/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tb_cleaner tb_cleaner = db.tb_cleaner.Find(id);

			// Delete all attendance records for the cleaner
			var attendanceRecords = db.tb_attendance
				.Where(a => a.att_cleanerID == id)
				.ToList();

			foreach (var attendanceRecord in attendanceRecords)
			{
				db.tb_attendance.Remove(attendanceRecord);
			}
			db.tb_cleaner.Remove(tb_cleaner);

			db.SaveChanges();
			return RedirectToAction("Service", "Services");
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
