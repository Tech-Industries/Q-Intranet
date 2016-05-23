using Dashboard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Dashboard.ViewModels
{
    public class ProjectsViewModel
    {
        public long ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public DateTime DateCreated { get; set; }
        public int? CreatorID { get; set; }
        public string Priority { get; set; }



        public static async Task<ProjectsViewModel> MapFromAsync(Project d)
        {
            return await Task.Run(() => { return MapFrom(d); });
        }

        public static async Task<List<ProjectsViewModel>> MapFromAsync(ICollection<Project> d)
        {
            return await Task.Run(() => { return MapFrom(d); });
        }

        public static ProjectsViewModel MapFrom(Project d)
        {
            return new ProjectsViewModel
            {
                ID = d.ID,
                Name = d.Name,
                Description = d.Description,
                Status = d.Status,
                CreatorID = d.CreatorID,
                Priority = d.Priority
            };
        }

        public static List<ProjectsViewModel> MapFrom(ICollection<Project> d)
        {
            return d.Select(x => MapFrom(x)).ToList();
        }

    }
}