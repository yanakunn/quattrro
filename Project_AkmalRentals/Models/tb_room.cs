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

    public partial class tb_room
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tb_room()
        {
            this.tb_tenant = new HashSet<tb_tenant>();
        }
        [DisplayName("Room ID")]
        public int f_id { get; set; }

        [DisplayName("Floor")]
        public Nullable<int> f_floor { get; set; }

        [DisplayName("Room")]
        public Nullable<int> f_room { get; set; }

        [DisplayName("Price (in RM)")]
        public Nullable<double> f_price { get; set; }

        [DisplayName("Status")]
        public Nullable<int> f_status { get; set; }
    
        public virtual tb_floor tb_floor { get; set; }
        public virtual tb_status tb_status { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tb_tenant> tb_tenant { get; set; }
    }
}