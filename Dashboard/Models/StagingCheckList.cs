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
    
    public partial class StagingCheckList
    {
        public int ID { get; set; }
        public Nullable<int> StagingAuditID { get; set; }
        public Nullable<int> StagingDetailID { get; set; }
        public string Name { get; set; }
        public string Owner { get; set; }
        public Nullable<int> OwnerID { get; set; }
        public Nullable<bool> Important { get; set; }
        public string Status { get; set; }
        public Nullable<System.DateTime> DateDue { get; set; }
        public Nullable<System.DateTime> DateCompleted { get; set; }
    }
}