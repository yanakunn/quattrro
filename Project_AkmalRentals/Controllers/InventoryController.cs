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
    public class InventoryController : Controller
    {
        private db_akmalrentalsEntities db = new db_akmalrentalsEntities();

        // GET: Inventory
        public ActionResult Index()
        {
            var tb_inventory = db.tb_inventory.Include(t => t.tb_floor);
            return View(tb_inventory.ToList());
        }
       

        // GET: Inventory/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_inventory tb_inventory = db.tb_inventory.Find(id);
            if (tb_inventory == null)
            {
                return HttpNotFound();
            }
            return View(tb_inventory);
        }

        // GET: Inventory/Create
        public ActionResult Create()
        {
            ViewBag.v_location = new SelectList(db.tb_floor, "y_id", "y_location");
            return View();
        }

        // POST: Inventory/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "v_id,v_location,v_item,v_quantity")] tb_inventory tb_inventory)
        {
            if (ModelState.IsValid)
            {
                db.tb_inventory.Add(tb_inventory);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.v_location = new SelectList(db.tb_floor, "y_id", "y_location", tb_inventory.v_location);
            return View(tb_inventory);
        }

        // GET: Inventory/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_inventory tb_inventory = db.tb_inventory.Find(id);
            if (tb_inventory == null)
            {
                return HttpNotFound();
            }
            ViewBag.v_location = new SelectList(db.tb_floor, "y_id", "y_location", tb_inventory.v_location);
            return View(tb_inventory);
        }

        // POST: Inventory/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "v_id,v_location,v_item,v_quantity")] tb_inventory tb_inventory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tb_inventory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.v_location = new SelectList(db.tb_floor, "y_id", "y_location", tb_inventory.v_location);
            return View(tb_inventory);
        }

        // GET: Inventory/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_inventory tb_inventory = db.tb_inventory.Find(id);
            if (tb_inventory == null)
            {
                return HttpNotFound();
            }
            return View(tb_inventory);
        }

        // POST: Inventory/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tb_inventory tb_inventory = db.tb_inventory.Find(id);
            db.tb_inventory.Remove(tb_inventory);
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
