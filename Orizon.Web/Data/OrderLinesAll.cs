//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Orizon.Web.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class OrderLinesAll
    {
        public int PlantID { get; set; }
        public string Status { get; set; }
        public Nullable<System.DateTime> DATE_DUE { get; set; }
        public string PART { get; set; }
        public string DESCRIPTION { get; set; }
        public string NAME_CUSTOMER { get; set; }
        public string CUSTOMER { get; set; }
        public string PRODUCT_LINE { get; set; }
        public Nullable<decimal> ORDER_QTY { get; set; }
        public Nullable<decimal> EXT { get; set; }
        public string ORDER_NO { get; set; }
        public string RECORD_NO { get; set; }
        public string CUSTOMER_PO { get; set; }
        public string ORDER_TYPE { get; set; }
    }
}