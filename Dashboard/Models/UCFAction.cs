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
    
    public partial class UCFAction
    {
        public int ID { get; set; }
        public Nullable<int> AreaID { get; set; }
        public string Owner { get; set; }
        public Nullable<System.DateTime> DateCreated { get; set; }
        public Nullable<System.DateTime> DateStart { get; set; }
        public Nullable<System.DateTime> DateDue { get; set; }
        public Nullable<System.DateTime> DateComplete { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }
        public Nullable<int> OwnerID { get; set; }
    }
}
