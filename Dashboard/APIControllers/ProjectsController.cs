using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Dashboard.Models;
using Dashboard.ViewModels;

namespace Dashboard.APIControllers
{
    public class ProjectsController : ApiController
    {
        private DashboardEntities db = new DashboardEntities();


        [ResponseType(typeof(List<object>))]
        public List<object> Get()
        {
            return db.ProjectsTopLevels.OrderBy(o => o.ID).ToList<object>();
        }

        public List<object> Get(string Status)
        {
            return db.ProjectsTopLevels.Where(x => x.Status == Status).OrderBy(o => o.ID).ToList<object>();
        }

        [ResponseType(typeof(ProjectsTopLevelViewModel))]
        public async Task<IHttpActionResult> Post(Project proj)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            db.Projects.Add(proj);
            await db.SaveChangesAsync();
            

            return Ok(await ProjectsTopLevelViewModel.MapFromAsync(db.ProjectsTopLevels.Where(x => x.ID == proj.ID).ToList()));
        }
        
    }
}