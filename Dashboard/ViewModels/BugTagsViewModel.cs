using Dashboard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Dashboard.ViewModels
{
    public class BugTagsViewModel
    {
        public long ID { get; set; }
        public int? BugID { get; set; }
        public string Tag { get; set; }



        public static async Task<BugTagsViewModel> MapFromAsync(BugTag d)
        {
            return await Task.Run(() => { return MapFrom(d); });
        }

        public static async Task<List<BugTagsViewModel>> MapFromAsync(ICollection<BugTag> d)
        {
            return await Task.Run(() => { return MapFrom(d); });
        }

        public static BugTagsViewModel MapFrom(BugTag d)
        {
            return new BugTagsViewModel
            {
                ID = d.ID,
                BugID = d.BugID,
                Tag = d.Tag
            };
        }

        public static List<BugTagsViewModel> MapFrom(ICollection<BugTag> d)
        {
            return d.Select(x => MapFrom(x)).ToList();
        }

    }
}