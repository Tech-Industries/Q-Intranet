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
    
    public partial class OnBoardPart
    {
        public long RecordNumber { get; set; }
        public string Company { get; set; }
        public string PartNumber { get; set; }
        public string PartDescription { get; set; }
        public Nullable<System.DateTime> DateEntered { get; set; }
        public string EnteredBy { get; set; }
        public Nullable<System.DateTime> DateUpdated { get; set; }
        public string UpdatedBy { get; set; }
        public string PartType { get; set; }
        public string OnBoardClass { get; set; }
        public string Customer { get; set; }
        public string PackageName { get; set; }
        public string Priority { get; set; }
        public Nullable<decimal> QtyPerShipSet { get; set; }
        public Nullable<decimal> PriceEach { get; set; }
        public Nullable<decimal> EstimatedMachineHours { get; set; }
        public Nullable<decimal> EstimatedMaterial { get; set; }
        public Nullable<decimal> EstimatedLaborDollars { get; set; }
        public Nullable<decimal> EstimatedOutsideCost { get; set; }
        public string FirstArticleJobNumber { get; set; }
        public Nullable<System.DateTime> FirstArticleJobScheduleDate { get; set; }
        public string ProductionJobNumber { get; set; }
        public Nullable<System.DateTime> ProductionJobScheduleDate { get; set; }
        public Nullable<System.DateTime> DatePOReceived { get; set; }
        public Nullable<System.DateTime> DatePODue { get; set; }
        public Nullable<System.DateTime> DatePOCommitted { get; set; }
        public Nullable<System.DateTime> DateScheduledRun { get; set; }
        public Nullable<decimal> PercentComplete { get; set; }
        public Nullable<System.DateTime> DateComplete { get; set; }
        public string SalesOrderNumber { get; set; }
        public string SalesOrderLine { get; set; }
    }
}
