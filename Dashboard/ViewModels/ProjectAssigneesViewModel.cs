using Dashboard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Dashboard.ViewModels
{
    public class ProjectAssigneesViewModel
    {
        public long ID { get; set; }
        public int? ProjID { get; set; }
        public int? AssigneeID { get; set; }







        public static async Task<ProjectAssigneesViewModel> MapFromAsync(ProjectAssignee d)
        {
            return await Task.Run(() => { return MapFrom(d); });
        }

        public static async Task<List<ProjectAssigneesViewModel>> MapFromAsync(ICollection<ProjectAssignee> d)
        {
            return await Task.Run(() => { return MapFrom(d); });
        }

        public static ProjectAssigneesViewModel MapFrom(ProjectAssignee d)
        {
            return new ProjectAssigneesViewModel
            {
                ID = d.ID,
                ProjID = d.ProjID,
                AssigneeID = d.AssigneeID
            };
        }

        public static List<ProjectAssigneesViewModel> MapFrom(ICollection<ProjectAssignee> d)
        {
            return d.Select(x => MapFrom(x)).ToList();
        }

    }
}