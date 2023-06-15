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
	public class AttendanceController : Controller
	{
		private db_akmalrentalsEntities db = new db_akmalrentalsEntities();

		// GET: Attendance
		public ActionResult Index()
		{
			var tb_attendance = db.tb_attendance.Include(t => t.tb_attendanceStatus).Include(t => t.tb_cleaner);
			return View(tb_attendance.ToList());
		}

		// GET: Attendance/Details/5
		public ActionResult Details(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			tb_attendance tb_attendance = db.tb_attendance.Find(id);
			if (tb_attendance == null)
			{
				return HttpNotFound();
			}
			return View(tb_attendance);
		}

		// GET: Attendance/Create
		public ActionResult Create()
		{
			ViewBag.att_status = new SelectList(db.tb_attendanceStatus, "atts_id", "atts_desc");
			ViewBag.att_cleanerID = new SelectList(db.tb_cleaner, "c_id", "c_name");
			return View();
		}

		// POST: Attendance/Create
		// To protect from overposting attacks, enable the specific properties you want to bind to, for 
		// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create([Bind(Include = "att_id,att_cleanerID,att_date,att_status")] tb_attendance tb_attendance)
		{
			if (ModelState.IsValid)
			{
				db.tb_attendance.Add(tb_attendance);
				db.SaveChanges();
				return RedirectToAction("Index");
			}

			ViewBag.att_status = new SelectList(db.tb_attendanceStatus, "atts_id", "atts_desc", tb_attendance.att_status);
			ViewBag.att_cleanerID = new SelectList(db.tb_cleaner, "c_id", "c_name", tb_attendance.att_cleanerID);
			return View(tb_attendance);
		}

		// GET: Attendance/Edit/5
		public ActionResult Edit(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			tb_attendance tb_attendance = db.tb_attendance.Find(id);
			if (tb_attendance == null)
			{
				return HttpNotFound();
			}
			ViewBag.att_status = new SelectList(db.tb_attendanceStatus, "atts_id", "atts_desc", tb_attendance.att_status);
			ViewBag.att_cleanerID = new SelectList(db.tb_cleaner, "c_id", "c_name", tb_attendance.att_cleanerID);
			return View(tb_attendance);
		}

		// POST: Attendance/Edit/5
		// To protect from overposting attacks, enable the specific properties you want to bind to, for 
		// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit([Bind(Include = "att_id,att_cleanerID,att_date,att_status")] tb_attendance tb_attendance)
		{
			if (ModelState.IsValid)
			{
				db.Entry(tb_attendance).State = EntityState.Modified;
				db.SaveChanges();
				return RedirectToAction("Index");
			}
			ViewBag.att_status = new SelectList(db.tb_attendanceStatus, "atts_id", "atts_desc", tb_attendance.att_status);
			ViewBag.att_cleanerID = new SelectList(db.tb_cleaner, "c_id", "c_name", tb_attendance.att_cleanerID);
			return View(tb_attendance);
		}

		// GET: Attendance/Delete/5
		public ActionResult Delete(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			tb_attendance tb_attendance = db.tb_attendance.Find(id);
			if (tb_attendance == null)
			{
				return HttpNotFound();
			}
			return View(tb_attendance);
		}

		// POST: Attendance/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public ActionResult DeleteConfirmed(int id)
		{
			tb_attendance tb_attendance = db.tb_attendance.Find(id);
			db.tb_attendance.Remove(tb_attendance);
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
