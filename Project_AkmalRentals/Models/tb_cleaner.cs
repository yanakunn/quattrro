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

    public partial class tb_cleaner
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tb_cleaner()
        {
            this.tb_attendance = new HashSet<tb_attendance>();
        }

        [DisplayName("Cleaner ID")]
        public int c_id { get; set; }
        [DisplayName("Name")]
        public string c_name { get; set; }
        [DisplayName("Company")]
        public string c_company { get; set; }
        [DisplayName("Location")]
        public int c_location { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tb_attendance> tb_attendance { get; set; }
        public virtual tb_floor tb_floor { get; set; }
    }
}