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
    
    public partial class StagingSnapshotTrend
    {
        public int Complete { get; set; }
        public int Total { get; set; }
        public int Incomplete { get; set; }
        public int Missing { get; set; }
        public int Hold { get; set; }
        public int UnReleased { get; set; }
        public Nullable<System.DateTime> DatePulled { get; set; }
        public Nullable<int> Year { get; set; }
        public Nullable<int> Month { get; set; }
        public Nullable<int> PlantID { get; set; }
        public int ID { get; set; }
    }
}