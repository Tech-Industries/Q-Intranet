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
    
    public partial class SalesStage
    {
        public string CompanyCode { get; set; }
        public string PlantCode { get; set; }
        public string PlantName { get; set; }
        public string InvoiceNum { get; set; }
        public string OrderNum { get; set; }
        public string OrderSuffix { get; set; }
        public string OrderLine { get; set; }
        public string DateOrder { get; set; }
        public string DateDue { get; set; }
        public string DateShipped { get; set; }
        public string DateInvoiced { get; set; }
        public string ShipVia { get; set; }
        public string PartNum { get; set; }
        public Nullable<decimal> QtyOrderd { get; set; }
        public Nullable<decimal> QtyShipped { get; set; }
        public Nullable<decimal> QtyBo { get; set; }
        public Nullable<decimal> QtyOriginal { get; set; }
        public string Description { get; set; }
        public string Customer { get; set; }
        public Nullable<decimal> Extension { get; set; }
        public Nullable<decimal> MarginPercent { get; set; }
        public Nullable<decimal> MarginAmt { get; set; }
        public Nullable<decimal> Price { get; set; }
        public Nullable<decimal> Cost { get; set; }
        public Nullable<bool> OnTime { get; set; }
        public int ID { get; set; }
    }
}
