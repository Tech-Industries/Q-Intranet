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
//using Dashboard.Models;
using Dashboard.ViewModels;
using Orizon.Web.Data;
using System.Web.Http.Results;
using Dashboard.Models;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace Dashboard.APIControllers
{
    public class OnboardingController : ApiController
    {
        private OnBoardingEntities1 onbdb = new OnBoardingEntities1();
        private DashboardEntities db = new DashboardEntities();

        [Route("api/v1/Onboarding/Authorized/{id:int}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetAuthorizedParts(int id)
        {
            var RelPer = db.UserGroups.Where(x => x.UserID == id && x.GroupID == 18);
            List< AuthorizedOnBoardPart> parts = new List<AuthorizedOnBoardPart>();
            if (RelPer.Any())
            {
                parts = await onbdb.AuthorizedOnBoardParts.OrderBy(o => o.DatePODue).ToListAsync();
            }
            else
            {
                parts = await onbdb.AuthorizedOnBoardParts.Where(x => x.DelegatedTo == id).OrderBy(o => o.DatePODue).ToListAsync();
            }
            
            if (!parts.Any())
            {
                return NotFound();
            }
            return (Ok(parts));
        }

        [Route("api/v1/Onboarding")]
        [HttpGet]
        public async Task<IHttpActionResult> GetParts()
        {
            var parts = await onbdb.OnBoardParts.OrderBy(o => o.DatePODue).Where(x => x.Archived != true).ToListAsync();
            if (!parts.Any())
            {
                return NotFound();
            }
            return (Ok(parts));
        }

        [Route("api/v1/Onboarding/{id:int}")]
        [HttpPut]
        public async Task<IHttpActionResult> PutPart(int id, OnBoardPart part)
        {

            var timeSubmitted = DateTime.Now;
            var oldPart = await onbdb.OnBoardParts.FindAsync(id);

            

            if (oldPart == null || part == null)
            {
                return NotFound();
            }
            try
            {

                var entry = onbdb.Entry(oldPart);
                oldPart.Archived = part.Archived;
                oldPart.FirstArticleJobNumber = part.FirstArticleJobNumber;


                //entry.State = EntityState.Modified;

                await onbdb.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return new ResponseMessageResult(new HttpResponseMessage(HttpStatusCode.BadRequest));
            }

            return Ok(oldPart);
        }



        [Route("api/v1/Onboarding/{id:int}")]
        [HttpDelete]
         public async Task<IHttpActionResult> DeletePart(int id)
        {
            try
            {
                onbdb.OnBoardParts.Remove(await onbdb.OnBoardParts.FindAsync(id));
                await onbdb.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return NotFound();
            }
            return Ok();
        }

        [Route("api/v1/Onboarding/Tasks")]
        [HttpGet]
        public async Task<IHttpActionResult> GetPartTasks()
        {
            var tasks = await onbdb.OnBoardTasks.ToListAsync();
            if (!tasks.Any())
            {
                return NotFound();
            }
            return (Ok(tasks));
        }

        [Route("api/v1/Onboarding/{id:int}/Tasks")]
        [HttpGet]
        public async Task<IHttpActionResult> GetPartTasksByID(int id)
        {
            var tasks = await onbdb.OnBoardTasks.Where(x => x.OnBoardPart == id && x.TaskSequence < 100).OrderBy(o => o.TaskSequence).ToListAsync();
            if (!tasks.Any())
            {
                return NotFound();
            }
            return (Ok(tasks));
        }

        [Route("api/v1/Onboarding/Tasks/{id:int}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetTaskByID(int id)
        {
            var tasks = await onbdb.OnBoardTasks.Where(x => x.OnBoardPart == id && x.TaskSequence < 100).OrderBy(o => o.TaskSequence).ToListAsync();
            if (!tasks.Any())
            {
                return NotFound();
            }
            return (Ok(tasks));
        }

        [Route("api/v1/Onboarding/Tasks/{id:int}")]
        [HttpPut]
        public async Task<IHttpActionResult> PutTask(int id, OnBoardTask task)
        {

            var timeSubmitted = DateTime.Now;
            var oldTask = await onbdb.OnBoardTasks.FindAsync(id);

            if (oldTask == null || task == null)
            {
                return NotFound();
            }
            try
            {

                var entry = onbdb.Entry(oldTask);
                if(oldTask.PercentComplete < 100 && task.PercentComplete == 100)
                {
                    oldTask.CompletedOn = timeSubmitted;
                    oldTask.CompletedBy = task.CompletedBy;
                }
                else if(task.PercentComplete < 100)
                {
                    oldTask.CompletedOn = null;
                    oldTask.CompletedBy = null;
                }

                oldTask.DelegatedTo = task.DelegatedTo;
                oldTask.CurrentStatus = task.CurrentStatus;
                oldTask.PercentComplete = Convert.ToDecimal(task.PercentComplete);
                oldTask.DueBy = task.DueBy;
                oldTask.RecoveryDate = task.RecoveryDate;


                //entry.State = EntityState.Modified;

                await onbdb.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return new ResponseMessageResult(new HttpResponseMessage(HttpStatusCode.BadRequest));
            }

            return Ok(oldTask);
        }

        [Route("api/v1/Onboarding/{id:int}/AvailJobs")]
        [HttpGet]
        public async Task<IHttpActionResult> GetAvailJobsByPartID(int id)
        {
            List<AvailPlanJob> Jobs = await onbdb.AvailPlanJobs.Where(x => x.OnBoardPart == id).OrderBy(o => o.Job).ToListAsync();
            if (!Jobs.Any())
            {
                return NotFound();
            }
            return (Ok(Jobs));
        }
    }
}