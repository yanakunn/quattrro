using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.WebPages;
using System.Net.Http;
using System.Threading.Tasks;
using Project_AkmalRentals.Models;



namespace Project_AkmalRentals.Controllers
{
    public class TenantController : Controller
    {
        private db_akmalrentalsEntities db = new db_akmalrentalsEntities();

        // GET: Tenant
        public ActionResult Index()
        {
			
			string successMessage = TempData["SuccessMessage"] as string;
            ViewBag.SuccessMessage = successMessage;
            var tb_tenant = db.tb_tenant.Include(t => t.tb_room);
            return View(tb_tenant.ToList());
        }

        public ActionResult Reminder()
        {
            var currentDate = DateTime.Now;
            var targetDate = currentDate.AddDays(1);
            var targetDay = targetDate.Day;

            var tenants = db.tb_tenant
                .Include(t => t.tb_room.tb_floor)
                .Where(t => t.t_chkinDate.Day == targetDay)
                .ToList();

            return View(tenants);
        }

        // GET: Tenant/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_tenant tb_tenant = db.tb_tenant.Find(id);
            if (tb_tenant == null)
            {
                return HttpNotFound();
            }
            return View(tb_tenant);
        }

        // GET: Tenant/Create
        public ActionResult Create()
        {
            // Retrieve the available rooms from the database
            var availableRooms = db.tb_room.Where(r => r.f_status == 1).ToList();

            // Create a SelectList using the available rooms
            //ViewBag.Rooms = new SelectList(availableRooms, "f_id", "f_room");
            // Create a SelectList using the available rooms with multiple columns            
            var roomList = availableRooms.Select(r => new SelectListItem
            {
                Value = r.f_id.ToString(),
                Text = $"{r.tb_floor.y_location}, Room{r.f_room}, RM{r.f_price}" // Concatenate multiple columns with a comma
            });

            // Pass the roomList to the view
            ViewBag.Rooms = roomList;

            ViewBag.t_room = new SelectList(roomList, "Value", "Text");


            return View();
        }

        // POST: Tenant/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        // POST: Tenant/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "t_ic,t_name,t_contact,t_emergencyContact,t_chkinDate,t_chkoutDate,t_room,t_ICdoc,t_pax")] tb_tenant tb_tenant, HttpPostedFileBase file)
        {

            var availableRooms = db.tb_room.Where(r => r.f_status == 1).ToList();
            var roomList = availableRooms.Select(r => new SelectListItem
            {
                Value = r.f_id.ToString(),
                Text = $"{r.tb_floor.y_location}, Room{r.f_room}, RM{r.f_price}" // Concatenate multiple columns with a comma
            });
            ViewBag.Rooms = roomList;
            ViewBag.t_room = new SelectList(roomList, "Value", "Text", tb_tenant.t_room);

            if (ModelState.IsValid)
            {
                // Check if the t_ic value already exists in the database
                bool isExistingIC = db.tb_tenant.Any(t => t.t_ic == tb_tenant.t_ic);
                if (isExistingIC)
                {
                    ModelState.AddModelError(string.Empty, "Tenant already exists.");
                    return View(tb_tenant);
                }

                else
                {
                    // Check for date validation
                    if (tb_tenant.t_chkoutDate >= tb_tenant.t_chkinDate)
                    {
                        // Save upload file in File folder
                        if (file != null && file.ContentLength > 0)
                        {
                            string _FileName = Path.GetFileName(file.FileName);
                            string _path = Path.Combine(Server.MapPath("~/File"), _FileName);
                            file.SaveAs(_path);
                            tb_tenant.t_ICdoc = _FileName;
                        }
                        db.tb_tenant.Add(tb_tenant);
                        db.SaveChanges();

                        // Update the room status to 2 (Unavailable)
                        var room = db.tb_room.FirstOrDefault(r => r.f_id == tb_tenant.t_room);
                        if (room != null)
                        {
                            room.f_status = 2; // Update the status to 2 (Unavailable)
                            db.SaveChanges();
                        }
                        TempData["SuccessMessage"] = "Tenant has been created successfully!";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewBag.Message = "Check-out date must be later than or equal to check-in date.";
                    }
                }
            }
            //ViewBag.t_location = new SelectList(db.tb_location, "l_id", "l_code");
            //ViewBag.t_room = new SelectList(db.tb_room, "f_id", "f_room", tb_tenant.t_room);
            //ViewBag.t_room = new SelectList(db.tb_room, "f_id", "f_room", tb_tenant.t_room.ToString(), "f_id", "f_room", "f_floor");
            ViewBag.t_room = new SelectList(roomList, "Value", "Text", tb_tenant.t_room);            
            return View(tb_tenant);
        }


        // GET: Tenant/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_tenant tb_tenant = db.tb_tenant.Find(id);
            if (tb_tenant == null)
            {
                return HttpNotFound();
            }
            
            return View(tb_tenant);
        }

        // POST: Tenant/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(tb_tenant tb_tenant)
        {
            if (ModelState.IsValid)
            {
                if (tb_tenant.t_chkoutDate.Date >= tb_tenant.t_chkinDate.Date)
                {
                    // Retrieve the existing tenant record from the database
                    var existingTenant = db.tb_tenant.Find(tb_tenant.t_ic);
                    if (existingTenant != null)
                    {
                        // Update the properties of the existing tenant
                        existingTenant.t_name = tb_tenant.t_name;
                        existingTenant.t_contact = tb_tenant.t_contact;
                        existingTenant.t_emergencyContact = tb_tenant.t_emergencyContact;
                        existingTenant.t_chkinDate = tb_tenant.t_chkinDate.ToLocalTime();
                        existingTenant.t_chkoutDate = tb_tenant.t_chkoutDate.ToLocalTime();
                        existingTenant.t_pax = tb_tenant.t_pax;

                        db.Entry(existingTenant).State = EntityState.Modified;
                    }
                    else
                    {
                        db.tb_tenant.Add(tb_tenant);
                    }

                    db.SaveChanges();                    
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Check-out date must be later than or equal to check-in date.");
                }
            }

            return View(tb_tenant);
        }


        // GET: Tenant/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_tenant tb_tenant = db.tb_tenant.Find(id);
            if (tb_tenant == null)
            {
                return HttpNotFound();
            }
            return View(tb_tenant);
        }
        // POST: Tenant/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            tb_tenant tb_tenant = db.tb_tenant.Find(id);
            if (tb_tenant == null)
            {
                return HttpNotFound();
            }

            // Update the room status to 2 (Unavailable)
            var room = db.tb_room.FirstOrDefault(r => r.f_id == tb_tenant.t_room);
            if (room != null)
            {
                room.f_status = 1; // Update the status to 2 (Unavailable)
                db.SaveChanges();
            }

            db.tb_tenant.Remove(tb_tenant);
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
