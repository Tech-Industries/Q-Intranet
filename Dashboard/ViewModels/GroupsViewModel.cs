using Dashboard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Dashboard.ViewModels
{
    public class GroupsViewModel
    {
        public int ID { get; set; }
        public string Category { get; set; }
        public string Access { get; set; }
        public string Description { get; set; }


        public static async Task<GroupsViewModel> MapFromAsync(Group g)
        {
            return await Task.Run(() => { return MapFrom(g); });
        }

        public static async Task<List<GroupsViewModel>> MapFromAsync(ICollection<Group> g)
        {
            return await Task.Run(() => { return MapFrom(g); });
        }
        public static GroupsViewModel MapFrom(Group g)
        {
            return new GroupsViewModel
            {
                ID = g.ID,
                Category = g.Category,
                Access = g.Access,
                Description = g.Description
            };
        }

        public static List<GroupsViewModel> MapFrom(ICollection<Group> g)
        {
            return g.Select(x => MapFrom(x)).ToList();
        }
        
        

    }

}
