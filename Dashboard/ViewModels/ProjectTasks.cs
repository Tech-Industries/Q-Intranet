using Dashboard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Dashboard.ViewModels
{
    public class ProjectTasksViewModel
    {
        public long ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateCompleted { get; set; }
        public string Status { get; set; }
        public decimal? EstTime { get; set; }
        public int? CreatorID { get; set; }
        public int? AssigneeID { get; set; }



        public static async Task<ProjectTasksViewModel> MapFromAsync(ProjectTask d)
        {
            return await Task.Run(() => { return MapFrom(d); });
        }

        public static async Task<List<ProjectTasksViewModel>> MapFromAsync(ICollection<ProjectTask> d)
        {
            return await Task.Run(() => { return MapFrom(d); });
        }

        public static ProjectTasksViewModel MapFrom(ProjectTask d)
        {
            return new ProjectTasksViewModel
            {
                ID = d.ID,
                Name = d.Name,
                Description = d.Description,
                DateCreated = d.DateCreated,
                DateCompleted = d.DateCompleted,
                Status = d.Status,
                EstTime = d.EstTime,
                CreatorID = d.CreatorID,
                AssigneeID = d.AssigneeID
            };
        }

        public static List<ProjectTasksViewModel> MapFrom(ICollection<ProjectTask> d)
        {
            return d.Select(x => MapFrom(x)).ToList();
        }

    }
}