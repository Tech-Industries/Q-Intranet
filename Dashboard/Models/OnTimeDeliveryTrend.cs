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
    
    public partial class OnTimeDeliveryTrend
    {
        public long ID { get; set; }
        public Nullable<int> PlantID { get; set; }
        public string Year { get; set; }
        public string Month { get; set; }
        public Nullable<decimal> OnTime { get; set; }
        public decimal PastDue { get; set; }
        public decimal OnTimePercent { get; set; }
    }
}
