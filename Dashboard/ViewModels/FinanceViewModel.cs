using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dashboard.Models;
using System.Threading.Tasks;

namespace Dashboard.ViewModels
{
    public class FinanceViewModel
    {
        public long ID { get; set; }
        public string Company { get; set; }
        public string PlantName { get; set; }
        public string Year { get; set; }
        public string Month { get; set; }
        public string Day { get; set; }
        public decimal? Sales { get; set; }
        public decimal? Amount { get; set; }
        public string DateInvoiced { get; set; }
        public string DateShipped { get; set; }
        public string DateDue { get; set; }
        public decimal? QtyShipped { get; set; }
        public string PartNum { get; set; }
        public string Description { get; set; }
        public string Customer { get; set; }
        public decimal? Extension { get; set; }
        public decimal? Cost { get; set; }
        public decimal? MarginAmt { get; set; }
        public decimal? MarginPercent { get; set; }
        public bool? OnTime { get; set; }



        //Daily Sales
        #region DailySales
        public static async Task<FinanceViewModel> MapFromAsync(SalesDailyByCompany f)
        {
            return await Task.Run(() => { return MapFrom(f); });
        }

        public static async Task<List<FinanceViewModel>> MapFromAsync(ICollection<SalesDailyByCompany> f)
        {
            return await Task.Run(() => { return MapFrom(f); });
        }

        public static FinanceViewModel MapFrom(SalesDailyByCompany f)
        {
            return new FinanceViewModel
            {
                ID = f.ID,
                Company = f.CompanyCode,
                Year = f.Year,
                Month = f.Month,
                Day = f.Day,
                Sales = f.Sales,
                MarginAmt = f.MarginAmt
            };
        }

        public static List<FinanceViewModel> MapFrom(ICollection<SalesDailyByCompany> m)
        {
            return m.Select(x => MapFrom(x)).ToList();
        }
        #endregion

        #region SalesDetail
        //Sales Detail
        public static async Task<FinanceViewModel> MapFromAsync(SalesStage f)
        {
            return await Task.Run(() => { return MapFrom(f); });
        }

        public static async Task<List<FinanceViewModel>> MapFromAsync(ICollection<SalesStage> f)
        {
            return await Task.Run(() => { return MapFrom(f); });
        }

        public static FinanceViewModel MapFrom(SalesStage f)
        {
            return new FinanceViewModel
            {
                OnTime = f.OnTime,
                Customer = f.Customer,
                PartNum = f.PartNum,
                Description = f.Description,
                DateDue = f.DateDue,
                QtyShipped = f.QtyShipped,
                Extension = f.Extension,
                Cost = f.Cost,
                MarginAmt = f.MarginAmt,
                MarginPercent = f.MarginPercent,
                PlantName = f.PlantName
                
                
                
                

            };
        }

        public static List<FinanceViewModel> MapFrom(ICollection<SalesStage> m)
        {
            return m.Select(x => MapFrom(x)).ToList();
        }
        #endregion

        #region MonthlyOvertime
        public static async Task<FinanceViewModel> MapFromAsync(OTMonthlyByCompany f)
        {
            return await Task.Run(() => { return MapFrom(f); });
        }

        public static async Task<List<FinanceViewModel>> MapFromAsync(ICollection<OTMonthlyByCompany> f)
        {
            return await Task.Run(() => { return MapFrom(f); });
        }

        public static FinanceViewModel MapFrom(OTMonthlyByCompany f)
        {
            return new FinanceViewModel
            {
                ID = f.ID,
                Company = f.CompanyCode,
                Year = f.Year,
                Month = f.Month,
                Amount = f.Amount
            };
        }

        public static List<FinanceViewModel> MapFrom(ICollection<OTMonthlyByCompany> m)
        {
            return m.Select(x => MapFrom(x)).ToList();
        }
        #endregion

        #region DailyCWO
        public static async Task<FinanceViewModel> MapFromAsync(CWODailyByPlant f)
        {
            return await Task.Run(() => { return MapFrom(f); });
        }

        public static async Task<List<FinanceViewModel>> MapFromAsync(ICollection<CWODailyByPlant> f)
        {
            return await Task.Run(() => { return MapFrom(f); });
        }

        public static FinanceViewModel MapFrom(CWODailyByPlant f)
        {
            return new FinanceViewModel
            {
                ID = f.ID,
                Company = f.CompanyCode,
                Year = f.Year,
                Month = f.Month,
                Day = f.Day,
                Amount = f.Amount
            };
        }

        public static List<FinanceViewModel> MapFrom(ICollection<CWODailyByPlant> m)
        {
            return m.Select(x => MapFrom(x)).ToList();
        }
        #endregion

    }

}