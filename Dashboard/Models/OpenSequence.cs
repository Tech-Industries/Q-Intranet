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
    
    public partial class OpenSequence
    {
        public int ID { get; set; }
        public string Job { get; set; }
        public string Suffix { get; set; }
        public Nullable<int> Seq { get; set; }
        public string Description { get; set; }
        public string WC { get; set; }
        public Nullable<System.DateTime> DateDue { get; set; }
        public string Router { get; set; }
        public string PartNumber { get; set; }
        public Nullable<int> PlantID { get; set; }
    }
}
