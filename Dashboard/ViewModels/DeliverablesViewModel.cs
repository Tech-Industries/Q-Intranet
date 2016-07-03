
using Orizon.Web.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Dashboard.ViewModels
{
    public class DeliverablesViewModel
    {
        public long ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Frequency { get; set; }
        public int? UserID { get; set; }
        public DateTime? FirstDueDate { get; set; }

        public int? DelID { get; set; }



        public static async Task<DeliverablesViewModel> MapFromAsync(Deliverable d)
        {
            return await Task.Run(() => { return MapFrom(d); });
        }

        public static async Task<List<DeliverablesViewModel>> MapFromAsync(ICollection<Deliverable> d)
        {
            return await Task.Run(() => { return MapFrom(d); });
        }

        public static DeliverablesViewModel MapFrom(Deliverable d)
        {
            return new DeliverablesViewModel
            {
                ID = d.ID,
                Name = d.Name,
                Description = d.Description,
                Frequency = d.Frequency,
                UserID = d.UserID,
                FirstDueDate = d.FirstDueDate
            };
        }

        public static List<DeliverablesViewModel> MapFrom(ICollection<Deliverable> d)
        {
            return d.Select(x => MapFrom(x)).ToList();
        }

    }
}