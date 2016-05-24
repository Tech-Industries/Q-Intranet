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
    
    public partial class TimeInStateByMachine
    {
        public long ID { get; set; }
        public string Mode { get; set; }
        public string Machine { get; set; }
        public string Description { get; set; }
        public string CurrentMode { get; set; }
        public Nullable<decimal> TotalTimeInMode { get; set; }
        public Nullable<decimal> StatePercentage { get; set; }
        public Nullable<int> MinutesCaptured { get; set; }
        public Nullable<decimal> Perc1 { get; set; }
        public Nullable<decimal> Perc2 { get; set; }
        public Nullable<decimal> Perc3 { get; set; }
        public Nullable<decimal> Perc4 { get; set; }
        public Nullable<System.DateTime> Date1 { get; set; }
        public Nullable<System.DateTime> Date2 { get; set; }
        public Nullable<System.DateTime> Date3 { get; set; }
        public Nullable<System.DateTime> Date4 { get; set; }
    }
}