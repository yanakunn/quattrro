using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Project_AkmalRentals.Models;
using Project_AkmalRentals.ViewModel;
using System.Data.Entity;
using System.Web.Security;

namespace Project_AkmalRentals.Controllers
{
    public class DashboardPropertyController : Controller
    { 
        private readonly db_akmalrentalsEntities db = new db_akmalrentalsEntities();


        // GET: DashboardProperty
        public ActionResult DashboardProperty()
        {
            var floors = db.tb_floor.ToList();
            var trimmedRooms = floors.Select(r => r.y_address?.Split(',')[1]?.Trim()).Where(s => s != null);
            var distinctCount = trimmedRooms.Distinct().Count();

            var available = db.tb_room.Count(r => r.f_status == 1);
            var room = db.tb_room.ToList();

            var dashboard = new Dashboard_Property
            {
                Floors = floors,
                Rooms = db.tb_room.ToList(),
                Inventory = db.tb_inventory.ToList(),
                Status = db.tb_status.ToList()
            };

            ViewBag.Property = distinctCount;
            ViewBag.Availability = available;
            ViewBag.Label = room.Select(s => s.tb_status.s_desc);
            
            return View(dashboard);
        }

    }
}