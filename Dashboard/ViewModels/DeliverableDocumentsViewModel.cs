using Dashboard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Dashboard.ViewModels
{
    public class DeliverableDocumentsViewModel
    {
        public long ID { get; set; }
        public int? DelDetID { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        public int? UserID { get; set; }
        public string Location { get; set; }
        public string Path { get; set; }
        public DateTime? TimeSubmitted { get; set; }




        public static async Task<DeliverableDocumentsViewModel> MapFromAsync(DeliverableDocument d)
        {
            return await Task.Run(() => { return MapFrom(d); });
        }

        public static async Task<List<DeliverableDocumentsViewModel>> MapFromAsync(ICollection<DeliverableDocument> d)
        {
            return await Task.Run(() => { return MapFrom(d); });
        }

        public static DeliverableDocumentsViewModel MapFrom(DeliverableDocument d)
        {
            return new DeliverableDocumentsViewModel
            {
                ID = d.ID,
                DelDetID = d.DelDetID,
                Title = d.Title,
                Type = d.Type,
                UserID = d.UserID,
                Location = d.Location,
                Path = d.Path,
                TimeSubmitted = d.TimeSubmitted,

            };
        }

        public static List<DeliverableDocumentsViewModel> MapFrom(ICollection<DeliverableDocument> d)
        {
            return d.Select(x => MapFrom(x)).ToList();
        }

    }
}