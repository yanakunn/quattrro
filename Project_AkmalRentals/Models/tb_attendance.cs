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

    public partial class tb_attendance
    {
        [DisplayName("Attendance ID")]
        public int att_id { get; set; }
        [DisplayName("Staff ID")]
        public int att_cleanerID { get; set; }
        [DisplayName("Attendance Date")]
        public System.DateTime att_date { get; set; }
        [DisplayName("Attendance Status")]
        public int att_status { get; set; }
    
        public virtual tb_attendanceStatus tb_attendanceStatus { get; set; }
        public virtual tb_cleaner tb_cleaner { get; set; }
    }
}
