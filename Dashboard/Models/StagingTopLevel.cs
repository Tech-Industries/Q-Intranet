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
    
    public partial class StagingTopLevel
    {
        public string Job { get; set; }
        public string Suffix { get; set; }
        public Nullable<int> Steps { get; set; }
        public Nullable<System.DateTime> DateStart { get; set; }
        public string PartNum { get; set; }
        public string Description { get; set; }
        public Nullable<System.DateTime> DateStageDue { get; set; }
        public int ID { get; set; }
        public Nullable<System.DateTime> DateClosed { get; set; }
        public Nullable<int> PlantID { get; set; }
    }
}
