using Dashboard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Dashboard.ViewModels
{
    public class DeliverableReviewsViewModel
    {
        public long ID { get; set; }
        public int? DelDetID { get; set; }
        public int? UserID { get; set; }        
        public DateTime? TimeReviewed { get; set; }




        public static async Task<DeliverableReviewsViewModel> MapFromAsync(DeliverableReview d)
        {
            return await Task.Run(() => { return MapFrom(d); });
        }

        public static async Task<List<DeliverableReviewsViewModel>> MapFromAsync(ICollection<DeliverableReview> d)
        {
            return await Task.Run(() => { return MapFrom(d); });
        }

        public static DeliverableReviewsViewModel MapFrom(DeliverableReview d)
        {
            return new DeliverableReviewsViewModel
            {
                ID = d.ID,
                DelDetID = d.DelDetID,
                UserID = d.UserID,
                TimeReviewed = d.TimeReviewed

            };
        }

        public static List<DeliverableReviewsViewModel> MapFrom(ICollection<DeliverableReview> d)
        {
            return d.Select(x => MapFrom(x)).ToList();
        }

    }
}