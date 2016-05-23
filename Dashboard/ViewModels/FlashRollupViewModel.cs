using Dashboard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Dashboard.ViewModels
{
    public class FlashRollupViewModel
    {
        public long ID { get; set; }
        public string Year { get; set; }
        public string Month { get; set; }
        public int? DaysInMonth { get; set; }
        public decimal? Cwo { get; set; }
        public decimal? CWOGoal { get; set; }
        public decimal? Scrap { get; set; }
        public decimal? ScrapGoal { get; set; }
        public decimal? ScrapAsPercent { get; set; }
        public string PlantCode { get; set; }
        public decimal? UCFScore { get; set; }
        public decimal? UCFGoal { get; set; }
        public int? PlantID { get; set; }
        public decimal? PDSales { get; set; }
        public decimal? Sales { get; set; }
        public decimal? Margins { get; set; }
        public decimal? MarginPct { get; set; }



        public static async Task<FlashRollupViewModel> MapFromAsync(FlashReportRollUp f)
        {
            return await Task.Run(() => { return MapFrom(f); });
        }

        public static async Task<List<FlashRollupViewModel>> MapFromAsync(ICollection<FlashReportRollUp> f)
        {
            return await Task.Run(() => { return MapFrom(f); });
        }

        public static FlashRollupViewModel MapFrom(FlashReportRollUp f)
        {
            return new FlashRollupViewModel
            {
                ID = f.ID,
                Year = f.Year,
                Month = f.Month,
                DaysInMonth = f.DaysInMonth,
                Cwo = f.Cwo,
                CWOGoal = f.CWOGoal,
                Scrap = f.Scrap,
                ScrapGoal = f.ScrapGoal,
                ScrapAsPercent = f.ScrapAsPercent,
                PlantCode = f.PlantCode,
                UCFScore = f.UCFScore,
                UCFGoal = f.UCFGoal,
                PlantID = f.PlantID,
                PDSales = f.PDSales,
                Sales = f.Sales,
                Margins = f.Margins,
                MarginPct = f.MarginPct

            };
        }

        public static List<FlashRollupViewModel> MapFrom(ICollection<FlashReportRollUp> f)
        {
            return f.Select(x => MapFrom(x)).ToList();
        }

    }
}