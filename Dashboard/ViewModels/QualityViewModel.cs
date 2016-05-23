using Dashboard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Dashboard.ViewModels
{
    public class QualityViewModel
    {
        public long ID { get; set; }
        public decimal? Amount { get; set; }
        public string Plant { get; set; }
        public string Year { get; set; }
        public string Month { get; set; }
        public string Day { get; set; }

        public string PlantCode { get; set; }
        public string Job { get; set; }
        public string Suffix { get; set; }
        public string ControlNumber { get; set; }
        public string PartNum { get; set; }
        public string Description { get; set; }
        public decimal? QtyDisposed { get; set; }
        public decimal? DisposedValue { get; set; }


        #region SalesYTD
        public static async Task<QualityViewModel> MapFromAsync(ScrapMonthlyByPlant g)
        {
            return await Task.Run(() => { return MapFrom(g); });
        }

        public static async Task<List<QualityViewModel>> MapFromAsync(ICollection<ScrapMonthlyByPlant> g)
        {
            return await Task.Run(() => { return MapFrom(g); });
        }

        public static QualityViewModel MapFrom(ScrapMonthlyByPlant g)
        {
            return new QualityViewModel
            {
                ID = g.ID,
                Amount = g.Amount,
                Plant = g.PlantCode,
                Month = g.Month,
                Year = g.Year
            };
        }

        public static List<QualityViewModel> MapFrom(ICollection<ScrapMonthlyByPlant> g)
        {
            return g.Select(x => MapFrom(x)).ToList();
        }
        #endregion


        public static async Task<QualityViewModel> MapFromAsync(ScrapDailyByPlant g)
        {
            return await Task.Run(() => { return MapFrom(g); });
        }

        public static async Task<List<QualityViewModel>> MapFromAsync(ICollection<ScrapDailyByPlant> g)
        {
            return await Task.Run(() => { return MapFrom(g); });
        }

        public static QualityViewModel MapFrom(ScrapDailyByPlant g)
        {
            return new QualityViewModel
            {
                ID = g.ID,
                Amount = g.Amount,
                Plant = g.PlantCode,
                Year = g.Year,
                Month = g.Month,
                Day = g.Day

            };
        }

        public static List<QualityViewModel> MapFrom(ICollection<ScrapDailyByPlant> g)
        {
            return g.Select(x => MapFrom(x)).ToList();
        }





        public static async Task<QualityViewModel> MapFromAsync(ScrapStage g)
        {
            return await Task.Run(() => { return MapFrom(g); });
        }

        public static async Task<List<QualityViewModel>> MapFromAsync(ICollection<ScrapStage> g)
        {
            return await Task.Run(() => { return MapFrom(g); });
        }

        public static QualityViewModel MapFrom(ScrapStage g)
        {
            return new QualityViewModel
            {
                 PlantCode = g.PlantCode,
                 ControlNumber = g.ControlNumber,
                 Job  = g.Job,
                 Suffix = g.Suffix,
                 PartNum = g.PartNum,
                 Description = g.Description,
                 QtyDisposed = g.QtyDisposed,
                 DisposedValue = g.DisposedValue,
                 Year = g.Year,
                 Month = g.Month,
                 Day = g.Day
            };
        }

        public static List<QualityViewModel> MapFrom(ICollection<ScrapStage> g)
        {
            return g.Select(x => MapFrom(x)).ToList();
        }

    }
}