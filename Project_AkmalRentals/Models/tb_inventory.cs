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
    
    public partial class tb_inventory
    {
        public int v_id { get; set; }
        public Nullable<int> v_location { get; set; }
        public string v_item { get; set; }
        public Nullable<int> v_quantity { get; set; }
    
        public virtual tb_floor tb_floor { get; set; }
    }
}
