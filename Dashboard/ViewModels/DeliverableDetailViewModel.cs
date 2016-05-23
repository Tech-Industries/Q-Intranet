using Dashboard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Dashboard.ViewModels
{
    public class DeliverableDetailViewModel
    {
        public long ID { get; set; }
        public int DelID { get; set; }
        public DateTime? DateDue { get; set; }
        public DateTime? DateCompleted { get; set; }
        public string Notes { get; set; }



        public static async Task<DeliverableDetailViewModel> MapFromAsync(DeliverableDetail d)
        {
            return await Task.Run(() => { return MapFrom(d); });
        }

        public static async Task<List<DeliverableDetailViewModel>> MapFromAsync(ICollection<DeliverableDetail> d)
        {
            return await Task.Run(() => { return MapFrom(d); });
        }

        public static DeliverableDetailViewModel MapFrom(DeliverableDetail d)
        {
            return new DeliverableDetailViewModel
            {
                ID = d.ID,
                DelID = d.DelID,
                DateDue = d.DateDue,
                DateCompleted = d.DateCompleted,
                Notes = d.Notes

            };
        }

        public static List<DeliverableDetailViewModel> MapFrom(ICollection<DeliverableDetail> d)
        {
            return d.Select(x => MapFrom(x)).ToList();
        }
        

    }
}