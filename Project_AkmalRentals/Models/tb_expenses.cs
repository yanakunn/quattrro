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

    public partial class tb_expenses
    {
        public int e_id { get; set; }
        public string e_Type { get; set; }
        [Required(ErrorMessage = "The Date field is required.")]
        public System.DateTime e_Date { get; set; }
        [Required(ErrorMessage = "The Category field is required.")]
        public int e_Category { get; set; }
        [Required(ErrorMessage = "The Payment field is required.")]
        public string e_Payment { get; set; }
        public string e_Detail { get; set; }
        [Required(ErrorMessage = "The Amount field is required.")]
        public double e_Amount { get; set; }
        public string e_Receipt { get; set; }
    
        public virtual tb_category tb_category { get; set; }
    }
}
