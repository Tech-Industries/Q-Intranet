//using Dashboard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Orizon.Web.Data;

namespace Dashboard.ViewModels
{
    public class ProjectsTopLevelViewModel
    {
        public long ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public DateTime DateCreated { get; set; }
        public string Priority { get; set; }
        public int? TasksComplete { get; set; }
        public int? TotalTasks { get; set; }
        public decimal? PercComplete { get; set; }
        public decimal? EstTimeRemaining { get; set; }
        public string Assignees { get; set; }



        public static async Task<ProjectsTopLevelViewModel> MapFromAsync(ProjectsTopLevel d)
        {
            return await Task.Run(() => { return MapFrom(d); });
        }

        public static async Task<List<ProjectsTopLevelViewModel>> MapFromAsync(ICollection<ProjectsTopLevel> d)
        {
            return await Task.Run(() => { return MapFrom(d); });
        }

        public static ProjectsTopLevelViewModel MapFrom(ProjectsTopLevel d)
        {
            return new ProjectsTopLevelViewModel
            {
                ID = d.ID,
                Name = d.Name,
                Description = d.Description,
                Status = d.Status,
                Priority = d.Priority,
                TasksComplete = d.TasksComplete,
                TotalTasks = d.TotalTasks,
                PercComplete = d.PercComplete,
                EstTimeRemaining = d.EstTimeRemaining,
                Assignees = d.Assignees
            };
        }

        public static List<ProjectsTopLevelViewModel> MapFrom(ICollection<ProjectsTopLevel> d)
        {
            return d.Select(x => MapFrom(x)).ToList();
        }

    }
}