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

    public partial class tb_user
    {
        [DisplayName("User ID")]
        public int u_id { get; set; }
        [DisplayName("Email")]
        
        public string u_email { get; set; }
        [DisplayName("Password")]
        [DataType(DataType.Password)]
        public string u_pwd { get; set; }
        [DisplayName("Name")]
        public string u_name { get; set; }
        [DisplayName("Role")]
        public Nullable<int> u_role { get; set; }
    
        public virtual tb_investor tb_investor { get; set; }
        public virtual tb_role tb_role { get; set; }
    }
}