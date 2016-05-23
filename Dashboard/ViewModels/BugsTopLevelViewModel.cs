using Dashboard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Dashboard.ViewModels
{
    public class BugsTopLevelViewModel
    {
        public long ID { get; set; }
        public string Project { get; set; }
        public string Description { get; set; }
        public string Creator { get; set; }
        public string Assignee { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public string Notes { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateClosed { get; set; }
        public string Tags { get; set; }



        public static async Task<BugsTopLevelViewModel> MapFromAsync(BugsTopLevel d)
        {
            return await Task.Run(() => { return MapFrom(d); });
        }

        public static async Task<List<BugsTopLevelViewModel>> MapFromAsync(ICollection<BugsTopLevel> d)
        {
            return await Task.Run(() => { return MapFrom(d); });
        }

        public static BugsTopLevelViewModel MapFrom(BugsTopLevel d)
        {
            return new BugsTopLevelViewModel
            {
                ID = d.ID,
                Project = d.Project,
                Description = d.Description,
                Creator = d.Creator,
                Assignee = d.Assignee,
                Type = d.Type,
                Notes = d.Notes,
                DateCreated = d.DateCreated,
                DateClosed = d.DateClosed,
                Tags = d.Tags
            };
        }

        public static List<BugsTopLevelViewModel> MapFrom(ICollection<BugsTopLevel> d)
        {
            return d.Select(x => MapFrom(x)).ToList();
        }

    }
}