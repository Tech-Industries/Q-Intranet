using Dashboard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Dashboard.ViewModels
{
    public class GoalsViewModel
    {
        public long ID { get; set; }
        public string Type { get; set; }
        public decimal? Amount { get; set; }
        public string Company { get; set; }
        public string Month { get; set; }
        public string Year { get; set; }



        public static async Task<GoalsViewModel> MapFromAsync(Goal g)
        {
            return await Task.Run(() => { return MapFrom(g); });
        }

        public static async Task<List<GoalsViewModel>> MapFromAsync(ICollection<Goal> g)
        {
            return await Task.Run(() => { return MapFrom(g); });
        }

        public static GoalsViewModel MapFrom(Goal g)
        {
            return new GoalsViewModel
            {
                ID = g.ID,
                Type = g.Type,
                Amount = g.Amount,
                Company = g.CompanyCode,
                Month = g.Month,
                Year = g.Year
            };
        }

        public static List<GoalsViewModel> MapFrom(ICollection<Goal> g)
        {
            return g.Select(x => MapFrom(x)).ToList();
        }

    }
}