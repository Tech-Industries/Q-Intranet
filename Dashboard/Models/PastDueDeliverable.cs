//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Dashboard.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class PastDueDeliverable
    {
        public long ID { get; set; }
        public Nullable<int> DelID { get; set; }
        public Nullable<int> DelDetID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Frequency { get; set; }
        public Nullable<System.DateTime> DateDue { get; set; }
        public Nullable<int> DaysPast { get; set; }
        public Nullable<System.DateTime> DateCompleted { get; set; }
        public Nullable<int> UserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
    }
}
