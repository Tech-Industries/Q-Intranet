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
    
    public partial class ProjectTask
    {
        public int ID { get; set; }
        public Nullable<int> ProjID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Nullable<System.DateTime> DateCreated { get; set; }
        public Nullable<System.DateTime> DateCompleted { get; set; }
        public string Status { get; set; }
        public Nullable<decimal> EstTime { get; set; }
        public Nullable<int> CreatorID { get; set; }
        public Nullable<int> AssigneeID { get; set; }
    }
}
