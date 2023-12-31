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
    using System.ComponentModel.DataAnnotations;

    public partial class tb_payment
    {
        public int p_id { get; set; }
        [Required(ErrorMessage = "The Date field is required.")]
        public System.DateTime p_Date { get; set; }
        [Required(ErrorMessage = "The Category field is required.")]
        public int p_Category { get; set; }
        [Required(ErrorMessage = "The Payment field is required.")]
        public string p_Payment { get; set; }
        [Required(ErrorMessage = "The Tenant Name field is required.")]
        public string p_TenantId { get; set; }
        [Required(ErrorMessage = "The Amount field is required.")]
        public double p_Amount { get; set; }
        public string p_Receipt { get; set; }
    
        public virtual tb_tenant tb_tenant { get; set; }
        public virtual tb_category tb_category { get; set; }
    }
}
