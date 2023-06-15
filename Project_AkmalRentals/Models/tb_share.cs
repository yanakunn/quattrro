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
    using System.ComponentModel.DataAnnotations;

    public partial class tb_share
    {
        [DisplayName("Share ID")]
        public int share_id { get; set; }

        [DisplayName("Share Amount")]
        public double share_amount { get; set; }
        public string share
        {
            get { return share_amount.ToString("N2"); }
        }
        [DisplayFormat(DataFormatString = "{0:MMMM yyyy}")]
        [DisplayName("Date")]
        public System.DateTime share_date { get; set; }

        [DisplayName("Investor")]
        public int share_investor { get; set; }
    
        public virtual tb_investor tb_investor { get; set; }
    }
}