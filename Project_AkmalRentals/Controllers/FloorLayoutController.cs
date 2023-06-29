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
using static System.Net.WebRequestMethods;

namespace AkmalRentals.Controllers
{
    public class FloorLayoutController : Controller
    {
        private db_akmalrentalsEntities db = new db_akmalrentalsEntities();

        // GET: FloorLayout
        public ActionResult Index()
        {
            var tb_floor = db.tb_floor;
            return View(tb_floor.ToList());
        }

        // GET: FloorLayout/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }


            tb_floor tb_floor = db.tb_floor.Find(id);
            if (tb_floor == null)
            {
                return HttpNotFound();
            }

           
            return View(tb_floor);
        }

        // GET: FloorLayout/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: FloorLayout/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( tb_floor tb_floor, HttpPostedFileBase file1)
        {
            if (ModelState.IsValid)
            {
                if (file1 != null && file1.ContentLength > 0)
                {
                    
                        string _FileName1 = Path.GetFileName(file1.FileName);
                        string _path1 = Path.Combine(Server.MapPath("~/Layout"), _FileName1);
                        file1.SaveAs(_path1);
                        tb_floor.y_floor = _FileName1;
                   
                       


                }
                db.tb_floor.Add(tb_floor);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tb_floor);
        }

        // GET: FloorLayout/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_floor tb_floor = db.tb_floor.Find(id);
            if (tb_floor == null)
            {
                return HttpNotFound();
            }
            return View(tb_floor);
        }

        // POST: FloorLayout/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(tb_floor tb_floor)
        {
            if (ModelState.IsValid)
            {
                
                db.Entry(tb_floor).State = EntityState.Modified;

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tb_floor);
        }

        // GET: FloorLayout/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_floor tb_floor = db.tb_floor.Find(id);
            if (tb_floor == null)
            {
                return HttpNotFound();
            }
            return View(tb_floor);
        }

        // POST: FloorLayout/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tb_floor tb_floor = db.tb_floor.Find(id);
            var floorCheck = db.tb_room.Where(f => f.f_floor == id).FirstOrDefault();
            if (floorCheck == null)
            {
                db.tb_floor.Remove(tb_floor);
                db.SaveChanges();
            }
            else
            {
                ViewBag.Error = "sorry, but you cannot delete a property that has rooms.";
                return View(tb_floor);
            }
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

