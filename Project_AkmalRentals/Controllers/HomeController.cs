using Project_AkmalRentals.Models;
using Project_AkmalRentals.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI.WebControls;

namespace Project_AkmalRentals.Controllers
{
    public class HomeController : Controller
    {
		private readonly db_akmalrentalsEntities db = new db_akmalrentalsEntities();

		//[Authorize(Roles = "Admin")]
		public ActionResult Index()
        {
            if (Session["uid"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            else
            {
				var tables = new Dashboard
				{
					Floors = db.tb_floor.ToList(),
					Rooms = db.tb_room.ToList(),
					Status = db.tb_status.ToList(),
					Tenant = db.tb_tenant.ToList(),
					Landlord = db.tb_landlord.ToList(),
					Investor = db.tb_investor.ToList()
				};

				int tenant =db.tb_tenant.Count();

				int currYear = DateTime.Now.Year;
				int currMonth = DateTime.Now.Month;
				var income = db.tb_expenses
					.Where(e => e.e_Date.Year == currYear && e.e_Date.Month == currMonth && e.e_Type == "Income")
					.ToList();

				var expenses = db.tb_expenses
					.Where(e => e.e_Date.Year == currYear && e.e_Date.Month == currMonth && e.e_Type == "Expenses")
					.ToList();

				double totalincome = income.Sum(e => e.e_Amount);
				double totalexpense = expenses.Sum(e => e.e_Amount);

				double totalProfit = totalincome - totalexpense;

				ViewBag.profit = totalProfit;
				ViewBag.t = tenant;
				ViewBag.i = totalincome;
				return View(tables);
            }

	    }

	}

}
