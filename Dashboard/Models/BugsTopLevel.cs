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
    
    public partial class BugsTopLevel
    {
        public int ID { get; set; }
        public string Project { get; set; }
        public string Description { get; set; }
        public string Creator { get; set; }
        public string Assignee { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public string Notes { get; set; }
        public Nullable<System.DateTime> DateCreated { get; set; }
        public Nullable<System.DateTime> DateClosed { get; set; }
        public string Tags { get; set; }
    }
}
