using Project_AkmalRentals.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Project_AkmalRentals.ViewModel
{
    public class Dashboard_Property
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

        public int v_id { get; set; }

        [DisplayName("Location")]
        public Nullable<int> v_location { get; set; }

        [DisplayName("Item Name")]
        public string v_item { get; set; }

        [DisplayName("Quantity")]
        public Nullable<int> v_quantity { get; set; }

        public IEnumerable<tb_floor> Floors { get; set; }

        public IEnumerable<tb_room> Rooms { get; set; }

        public IEnumerable<tb_inventory> Inventory { get; set; }

        public IEnumerable<tb_status> Status { get; set; }
    }
}