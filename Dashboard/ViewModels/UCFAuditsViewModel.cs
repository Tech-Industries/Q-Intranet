using Dashboard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Dashboard.ViewModels
{
    public class UCFAuditsViewModel
    {
        public long ID { get; set; }
        public int? UserID { get; set; }
        public int? AreaID { get; set; }
        public DateTime? DateCompleted { get; set; }



        public static async Task<UCFAuditsViewModel> MapFromAsync(UCFAudit d)
        {
            return await Task.Run(() => { return MapFrom(d); });
        }

        public static async Task<List<UCFAuditsViewModel>> MapFromAsync(ICollection<UCFAudit> d)
        {
            return await Task.Run(() => { return MapFrom(d); });
        }

        public static UCFAuditsViewModel MapFrom(UCFAudit d)
        {
            return new UCFAuditsViewModel
            {
                ID = d.ID,
                UserID = d.UserID,
                AreaID = d.AreaID,
                DateCompleted = d.DateCompleted
            };
        }

        public static List<UCFAuditsViewModel> MapFrom(ICollection<UCFAudit> d)
        {
            return d.Select(x => MapFrom(x)).ToList();
        }

    }
}