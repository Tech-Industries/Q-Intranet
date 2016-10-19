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
    
    public partial class Calendar
    {
        public int DateKey { get; set; }
        public System.DateTime Date { get; set; }
        public byte Day { get; set; }
        public string DaySuffix { get; set; }
        public byte Weekday { get; set; }
        public string WeekDayName { get; set; }
        public bool IsWeekend { get; set; }
        public bool IsHoliday { get; set; }
        public string HolidayText { get; set; }
        public byte DOWInMonth { get; set; }
        public short DayOfYear { get; set; }
        public byte WeekOfMonth { get; set; }
        public byte WeekOfYear { get; set; }
        public byte ISOWeekOfYear { get; set; }
        public byte Month { get; set; }
        public string MonthName { get; set; }
        public byte Quarter { get; set; }
        public string QuarterName { get; set; }
        public int Year { get; set; }
        public string MMYYYY { get; set; }
        public string MonthYear { get; set; }
        public System.DateTime FirstDayOfMonth { get; set; }
        public System.DateTime LastDayOfMonth { get; set; }
        public System.DateTime FirstDayOfQuarter { get; set; }
        public System.DateTime LastDayOfQuarter { get; set; }
        public System.DateTime FirstDayOfYear { get; set; }
        public System.DateTime LastDayOfYear { get; set; }
        public System.DateTime FirstDayOfNextMonth { get; set; }
        public System.DateTime FirstDayOfNextYear { get; set; }
    }
}
