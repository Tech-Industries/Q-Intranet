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
    
    public partial class ProjectsTopLevel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public Nullable<System.DateTime> DateCreated { get; set; }
        public string Priority { get; set; }
        public Nullable<int> TasksComplete { get; set; }
        public Nullable<int> TotalTasks { get; set; }
        public Nullable<decimal> PercComplete { get; set; }
        public Nullable<decimal> EstTimeRemaining { get; set; }
        public string Assignees { get; set; }
    }
}