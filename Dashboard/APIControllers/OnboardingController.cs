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

        [Route("api/v1/Onboarding/Authorized/{id:int}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetAuthorizedParts(int id)
        {
            var parts = await onbdb.AuthorizedOnBoardParts.Where(x => x.DelegatedTo == id).ToListAsync();
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
            var parts = await onbdb.OnBoardParts.ToListAsync();
            if (!parts.Any())
            {
                return NotFound();
            }
            return (Ok(parts));
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
        public async Task<IHttpActionResult> PutProject(int id, OnBoardTask task)
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

                oldTask.DelegatedTo = task.DelegatedTo;
                oldTask.CurrentStatus = task.CurrentStatus;
                oldTask.PercentComplete = Convert.ToDecimal(task.PercentComplete);
                if(task.PercentComplete == 100)
                {
                    if (oldTask.CompletedOn == null)
                    {
                        oldTask.CompletedBy = task.CompletedBy;
                        if (oldTask.DueBy >= timeSubmitted)
                        {
                            oldTask.CurrentStatus = "Complete";
                        }
                        else
                        {
                            oldTask.CurrentStatus = "Completed Late";
                        }
                        oldTask.CompletedOn = timeSubmitted;
                    }
                }
                else
                {
                    if (oldTask.DueBy >= timeSubmitted)
                    {
                        oldTask.CurrentStatus = "Future";
                    }
                    else
                    {
                        oldTask.CurrentStatus = "Late";
                    }

                    oldTask.CompletedOn = null;
                    oldTask.CompletedBy = null;
                }

                //entry.State = EntityState.Modified;

                await onbdb.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return new ResponseMessageResult(new HttpResponseMessage(HttpStatusCode.BadRequest));
            }

            return Ok(oldTask);
        }
    }
}