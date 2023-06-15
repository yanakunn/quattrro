using Project_AkmalRentals.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Project_AkmalRentals.ViewModel
{
	public class Dashboard
	{
		[Key]
		public int y_id { get; set; }

		[DisplayName("Location Code")]
		public string y_location { get; set; }

		[DisplayName("Address")]
		public string y_address { get; set; }

		[DisplayName("Floor Layout")]
		public string y_floor { get; set; }

		[DisplayName("Description")]
		public string y_desc { get; set; }

		[DisplayName("CCTV QR")]
		public string y_cctvqr { get; set; }

		public int f_id { get; set; }

		[DisplayName("Location")]
		public Nullable<int> f_floor { get; set; }

		[DisplayName("Room No")]
		public Nullable<int> f_room { get; set; }

		[DisplayName("Price")]
		public Nullable<double> f_price { get; set; }

		[DisplayName("Status")]
		public Nullable<int> f_status { get; set; }

		public string t_ic { get; set; }
		public string t_name { get; set; }
		public string t_contact { get; set; }
		public string t_emergencyContact { get; set; }
		public System.DateTime t_chkinDate { get; set; }
		public System.DateTime t_chkoutDate { get; set; }
		public int t_room { get; set; }
		public string t_ICdoc { get; set; }
		public int t_pax { get; set; }

		public string l_name { get; set; }
		public string l_contact { get; set; }
		public int l_location { get; set; }
		public System.DateTime l_ctrctStartDate { get; set; }
		public System.DateTime l_ctrctEndDate { get; set; }
		public double l_rentalAmount { get; set; }

		public int i_id { get; set; }
		public double i_investment { get; set; }
		public double i_percentage { get; set; }
		public int i_lot { get; set; }


		public IEnumerable<tb_floor> Floors { get; set; }

		public IEnumerable<tb_room> Rooms { get; set; }


		public IEnumerable<tb_status> Status { get; set; }

		public IEnumerable<tb_tenant> Tenant { get; set; }

		public IEnumerable<tb_landlord> Landlord { get; set; }

		public IEnumerable<tb_investor> Investor { get; set; }

	}
}