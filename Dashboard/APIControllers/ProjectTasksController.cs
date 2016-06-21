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
    public class ProjectTasksController : ApiController
    {
        private DashboardEntities db = new DashboardEntities();



        public List<object> Get(int ProjID)
        {
            return db.ProjectTasks.Where(x => x.ProjID == ProjID).OrderBy(o => o.ID).ToList<object>();
        }

        [ResponseType(typeof(ProjectTasksViewModel))]
        public async Task<IHttpActionResult> Post(ProjectTask proj)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            db.ProjectTasks.Add(proj);
            await db.SaveChangesAsync();


            return Ok(await ProjectTasksViewModel.MapFromAsync(proj));
        }


        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> Put([FromBody]ProjectTask ptask)
        {
            int ID = ptask.ID;
            System.Diagnostics.Debug.WriteLine(ptask.Status);

            if (!ModelState.IsValid)
            {
                System.Diagnostics.Debug.WriteLine("BadModelState");
                return BadRequest(ModelState);
            }

            if (ID != ptask.ID)
            {
                System.Diagnostics.Debug.WriteLine("BadReq");
                return BadRequest();
            }

            ProjectTask uptask = (ProjectTask)db.ProjectTasks.Where(x => x.ID == ID).First();
            uptask.Status = ptask.Status;
            if(uptask.Status == "Complete")
            {
                uptask.DateCompleted = DateTime.Now;
            }
            else
            {
                uptask.DateCompleted = null;
            }


            db.ProjectTasks.Attach(uptask);
            var entry = db.Entry(uptask);
            entry.Property(e => e.Status).IsModified = true;
            entry.Property(e => e.DateCompleted).IsModified = true;
            // other changed properties
            db.SaveChanges();

            //db.Entry(deldet).State = EntityState.Modified;
            //System.Diagnostics.Debug.WriteLine("Modified");
            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PTasksExists(ID))
                {
                    System.Diagnostics.Debug.WriteLine("Catch If");
                    return NotFound();
                }
                else {
                    System.Diagnostics.Debug.WriteLine("Catch If Else");
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }



        private bool PTasksExists(long ID)
        {
            return db.DeliverableComments.Count(e => e.ID == ID) > 0;
        }
    }
}