using Dashboard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Dashboard.ViewModels
{
    public class DeliverableReviewersViewModel
    {
        public long ID { get; set; }
        public int? DelID { get; set; }
        public int? UserID { get; set; }







        public static async Task<DeliverableReviewersViewModel> MapFromAsync(DeliverableReviewer d)
        {
            return await Task.Run(() => { return MapFrom(d); });
        }

        public static async Task<List<DeliverableReviewersViewModel>> MapFromAsync(ICollection<DeliverableReviewer> d)
        {
            return await Task.Run(() => { return MapFrom(d); });
        }

        public static DeliverableReviewersViewModel MapFrom(DeliverableReviewer d)
        {
            return new DeliverableReviewersViewModel
            {
                ID = d.ID,
                UserID = d.UserID,
                DelID = d.DelID
            };
        }

        public static List<DeliverableReviewersViewModel> MapFrom(ICollection<DeliverableReviewer> d)
        {
            return d.Select(x => MapFrom(x)).ToList();
        }

    }
}