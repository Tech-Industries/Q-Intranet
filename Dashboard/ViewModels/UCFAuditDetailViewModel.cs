using Dashboard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Dashboard.ViewModels
{
    public class UCFAuditDetailViewModel
    {
        public long ID { get; set; }
        public int? AuditID { get; set; }
        public int? CriteriaID { get; set; }
        public decimal? Score { get; set; }
        public bool? Selected { get; set; }



        public static async Task<UCFAuditDetailViewModel> MapFromAsync(UCFAuditDetail d)
        {
            return await Task.Run(() => { return MapFrom(d); });
        }

        public static async Task<List<UCFAuditDetailViewModel>> MapFromAsync(ICollection<UCFAuditDetail> d)
        {
            return await Task.Run(() => { return MapFrom(d); });
        }

        public static UCFAuditDetailViewModel MapFrom(UCFAuditDetail d)
        {
            return new UCFAuditDetailViewModel
            {
                ID = d.ID,
                AuditID = d.AuditID,
                CriteriaID = d.CriteriaID,
                Score = d.Score,
                Selected = d.Selected
            };
        }

        public static List<UCFAuditDetailViewModel> MapFrom(ICollection<UCFAuditDetail> d)
        {
            return d.Select(x => MapFrom(x)).ToList();
        }

    }
}