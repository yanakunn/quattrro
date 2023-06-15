using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Project_AkmalRentals.Models;

namespace AkmalRentals.Controllers
{
    public class RoomController : Controller
    {
        private db_akmalrentalsEntities db = new db_akmalrentalsEntities();

        // GET: Room
        public ActionResult Index()
        {


            var tb_room = db.tb_room.Include(t => t.tb_floor).Include(t => t.tb_status);
            return View(tb_room.ToList());
        }

        // GET: Room/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_room tb_room = db.tb_room.Find(id);
            if (tb_room == null)
            {
                return HttpNotFound();
            }
            return View(tb_room);
        }

        // GET: Room/Create
        public ActionResult Create()
        {
			ViewBag.f_floor = new SelectList(db.tb_floor, "y_id", "y_location");
			ViewBag.f_status = new SelectList(db.tb_status, "s_id", "s_desc");
            return View();
        }

        // POST: Room/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "f_id,f_floor,f_room,f_price,f_status")] tb_room tb_room)
        {
            if (ModelState.IsValid)
            {
                db.tb_room.Add(tb_room);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
			ViewBag.f_floor = new SelectList(db.tb_floor, "y_id", "y_location", tb_room.f_floor);
			ViewBag.f_status = new SelectList(db.tb_status, "s_id", "s_desc", tb_room.f_status);
            return View(tb_room);
        }

        // GET: Room/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_room tb_room = db.tb_room.Find(id);
            if (tb_room == null)
            {
                return HttpNotFound();
            }
            ViewBag.f_floor = new SelectList(db.tb_floor, "y_id", "y_location", tb_room.f_floor);

            ViewBag.f_status = new SelectList(db.tb_status, "s_id", "s_desc", tb_room.f_status);
            return View(tb_room);
        }

        // POST: Room/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "f_id,f_floor,f_room, f_price,f_status")] tb_room tb_room)
        {
            if (ModelState.IsValid)
            {
                
                db.Entry(tb_room).State = EntityState.Modified;

                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.f_floor = new SelectList(db.tb_floor, "y_id", "y_location", tb_room.f_floor);
            ViewBag.f_status = new SelectList(db.tb_status, "s_id", "s_desc", tb_room.f_status);
            return View(tb_room);
        }

        // GET: Room/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_room tb_room = db.tb_room.Find(id);
            if (tb_room == null)
            {
                return HttpNotFound();
            }
            return View(tb_room);
        }

        // POST: Room/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tb_room tb_room = db.tb_room.Find(id);
            db.tb_room.Remove(tb_room);
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
