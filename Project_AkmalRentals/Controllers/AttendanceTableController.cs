using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Project_AkmalRentals.Models;
using Project_AkmalRentals.ViewModel;

namespace Project_AkmalRentals.Controllers
{
	public class AttendanceTableController : Controller
	{
		// GET: AttendanceTable
		public ActionResult AttendanceTable()
		{
			//Get current month number, you can pass this value to controller ActionMethod also
			int month = DateTime.Now.Month;

			//Create List
			List<CleanerAttendance> cleanerWithDate = new List<CleanerAttendance>();
			using (var context = new db_akmalrentalsEntities())
			{
				// get emp Name, Id, Date time and order it in ascending order of date
				cleanerWithDate = context.tb_attendance.Where(a => a.att_date != null && a.att_date.Month == month)
					.Select(a =>
					new CleanerAttendance
					{
						att_id = a.att_id,
						att_date = a.att_date,
						att_cleanerName = a.tb_cleaner.c_name
					}).OrderBy(a => a.att_date).ToList();

			}

			return View(cleanerWithDate);
		}
	}
}
