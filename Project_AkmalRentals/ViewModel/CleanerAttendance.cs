using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project_AkmalRentals.ViewModel
{
	public class CleanerAttendance
	{
		[Key]
		public int att_id { get; set; }
		public string att_cleanerName { get; set; }
		public DateTime? att_date { get; set; }
	}
}