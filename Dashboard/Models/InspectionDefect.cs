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
    
    public partial class InspectionDefect
    {
        public int ID { get; set; }
        public Nullable<int> InspectionID { get; set; }
        public Nullable<int> InspectorID { get; set; }
        public Nullable<System.DateTime> DateSubmitted { get; set; }
        public string Category { get; set; }
        public string SubCategory { get; set; }
        public Nullable<int> Quantity { get; set; }
    }
}
