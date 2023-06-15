using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Project_AkmalRentals.Models;

namespace Project_AkmalRentals.ViewModel
{
	public class Services 
	{
		public int c_id { get; set; }
		public string c_name { get; set; }
		public string c_company { get; set; }
		public int c_location { get; set; }

		public string l_name { get; set; }
		public string l_contact { get; set; }
		public int l_location { get; set; }
		public System.DateTime l_ctrctStartDate { get; set; }
		public System.DateTime l_ctrctEndDate { get; set; }
		public double l_rentalAmount { get; set; }

		public IEnumerable<tb_cleaner> Cleaners { get; set; }

		public IEnumerable<tb_landlord> Landlord { get; set; }

	}
}