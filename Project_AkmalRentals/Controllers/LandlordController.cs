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
    public class LandlordController : Controller
    {
        private db_akmalrentalsEntities db = new db_akmalrentalsEntities();

        // GET: Landlord
        public ActionResult Index()
        {
            var tb_landlord = db.tb_landlord.Include(t => t.tb_floor);
            return View(tb_landlord.ToList());
        }

        // GET: Landlord/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_landlord tb_landlord = db.tb_landlord.Find(id);
            if (tb_landlord == null)
            {
                return HttpNotFound();
            }
            return View(tb_landlord);
        }

        // GET: Landlord/Create
        public ActionResult Create()
        {
            ViewBag.l_location = new SelectList(db.tb_floor, "y_id", "y_location");
            return View();
        }

        // POST: Landlord/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "l_id,l_name,l_contact,l_location,l_ctrctStartDate,l_ctrctEndDate,l_rentalAmount")] tb_landlord tb_landlord)
        {
            if (ModelState.IsValid)
            {
                db.tb_landlord.Add(tb_landlord);
                db.SaveChanges();
                return RedirectToAction("Service", "Services");
            }

            ViewBag.l_location = new SelectList(db.tb_floor, "y_id", "y_location", tb_landlord.l_location);
            return View(tb_landlord);
        }

        // GET: Landlord/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_landlord tb_landlord = db.tb_landlord.Find(id);
            if (tb_landlord == null)
            {
                return HttpNotFound();
            }
            ViewBag.l_location = new SelectList(db.tb_floor, "y_id", "y_location", tb_landlord.l_location);
            return View(tb_landlord);
        }

        // POST: Landlord/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "l_id,l_name,l_contact,l_location,l_ctrctStartDate,l_ctrctEndDate,l_rentalAmount")] tb_landlord tb_landlord)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tb_landlord).State = EntityState.Modified;
                db.SaveChanges();
				return RedirectToAction("Service", "Services");
			}
            ViewBag.l_location = new SelectList(db.tb_floor, "y_id", "y_location", tb_landlord.l_location);
            return View(tb_landlord);
        }

        // GET: Landlord/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_landlord tb_landlord = db.tb_landlord.Find(id);
            if (tb_landlord == null)
            {
                return HttpNotFound();
            }
            return View(tb_landlord);
        }

        // POST: Landlord/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tb_landlord tb_landlord = db.tb_landlord.Find(id);
            db.tb_landlord.Remove(tb_landlord);
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
