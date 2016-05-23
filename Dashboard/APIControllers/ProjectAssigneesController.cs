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
    public class ProjectAssigneesController : ApiController
    {
        private DashboardEntities db = new DashboardEntities();


        //[ResponseType(typeof(List<object>))]
        //public List<object> Get()
        //{
        //    return db.ProjectsTopLevels.OrderBy(o => o.ID).ToList<object>();
        //}
        
        [ResponseType(typeof(ProjectAssigneesViewModel))]
        public async Task<IHttpActionResult> Post(ProjectAssignee projA)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }
            db.ProjectAssignees.Add(projA);
            await db.SaveChangesAsync();

            return Ok(ProjectAssigneesViewModel.MapFrom(projA));
        }
    }
}