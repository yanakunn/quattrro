//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Project_AkmalRentals.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    public partial class tb_landlord
    {
        [DisplayName("ID")]
        public int l_id { get; set; }

        [DisplayName("Name")]
        public string l_name { get; set; }

        [DisplayName("Contact")]
        public string l_contact { get; set; }

        [DisplayName("Location")]
        public int l_location { get; set; }

        [DisplayName("Contract Start Date")]
        public System.DateTime l_ctrctStartDate { get; set; }

        [DisplayName("Contract End Date")]
        public System.DateTime l_ctrctEndDate { get; set; }

        [DisplayName("Rental Amount (in RM)")]
        public double l_rentalAmount { get; set; }
    
        public virtual tb_floor tb_floor { get; set; }
    }
}