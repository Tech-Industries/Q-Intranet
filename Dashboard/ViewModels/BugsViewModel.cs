using Dashboard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Dashboard.ViewModels
{
    public class BugsViewModel
    {
        public long ID { get; set; }
        public string Project { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public int? UserID { get; set; }
        public int? AssigneeID { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateClosed { get; set; }
        public string Status { get; set; }
        public string Notes { get; set; }



        public static async Task<BugsViewModel> MapFromAsync(Bug d)
        {
            return await Task.Run(() => { return MapFrom(d); });
        }

        public static async Task<List<BugsViewModel>> MapFromAsync(ICollection<Bug> d)
        {
            return await Task.Run(() => { return MapFrom(d); });
        }

        public static BugsViewModel MapFrom(Bug d)
        {
            return new BugsViewModel
            {
                ID = d.ID,
                Project = d.Project,
                Description = d.Description,
                Type = d.Type,
                UserID = d.UserID,
                AssigneeID = d.AssigneeID,
                DateCreated = d.DateCreated,
                DateClosed = d.DateClosed,
                Status = d.Status,
                Notes = d.Notes
            };
        }

        public static List<BugsViewModel> MapFrom(ICollection<Bug> d)
        {
            return d.Select(x => MapFrom(x)).ToList();
        }

    }
}