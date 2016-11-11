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
using System.Diagnostics;

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
            var Mod = db.UserGroups.Where(x => x.UserID == id && x.GroupID == 19);
            string sid = id.ToString();

            List<AuthorizedOnBoardPart> authParts = new List<AuthorizedOnBoardPart>();
            List<OnBoardPart> parts = new List<OnBoardPart>();


            //authParts = await onbdb.AuthorizedOnBoardParts.Where(x => x.DelegatedTo == "90").OrderBy(o => o.DatePODue).ToListAsync();
            //if (!authParts.Any())
            //{
            //    return NotFound();
            //}
            //return (Ok(authParts));


            if (RelPer.Any())
            {
                parts = await onbdb.OnBoardParts.OrderBy(o => o.DatePODue).Where(x => x.Archived != true).ToListAsync();
                if (!parts.Any())
                {
                    return NotFound();
                }
                return (Ok(parts));
            }
            else if (Mod.Any())
            {
                parts = await onbdb.OnBoardParts.OrderBy(o => o.DatePODue).Where(x => x.Archived != true).ToListAsync();
                if (!parts.Any())
                {
                    return NotFound();
                }
                return (Ok(parts));
            }
            else
            {
                authParts = await onbdb.AuthorizedOnBoardParts.Where(x => x.DelegatedTo == sid && x.Archived != true).OrderBy(o => o.DatePODue).ToListAsync();
                if (!authParts.Any())
                {
                    return NotFound();
                }
                return (Ok(authParts));
            }


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
                if (oldTask.PercentComplete < 100 && task.PercentComplete == 100)
                {
                    oldTask.CompletedOn = timeSubmitted;
                    oldTask.CompletedBy = task.CompletedBy;
                }
                else if (task.PercentComplete < 100)
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

        [Route("api/v1/Onboarding/metrics/burndown")]
        [HttpGet]
        public async Task<IHttpActionResult> GetOnboardingBurndown(string Program = null, int Task = 0)
        {
            if (Task == 0)
            {
                return BadRequest();
            }

            var expec = await onbdb.ExpectedDueCounts.Where(x => x.PackageName == Program && x.Task == Task).OrderBy(o => o.DueBy).ToListAsync();
            var actual = await onbdb.ActualCompCounts.Where(x => x.PackageName == Program && x.Task == Task).OrderBy(o => o.CompletedOn).ToListAsync();
            if (!expec.Any())
            {
                return NotFound();
            }

            DateTime? startDate = expec.First().DueBy;
            DateTime? endDate = expec.Last().DueBy;
            int? totalDue = expec.Sum(item => item.Total);
            int? totalComp = actual.Sum(item => item.Total);

            if (endDate < actual.Last().CompletedOn)
            {
                endDate = actual.Last().CompletedOn;
            }
            if (totalDue > totalComp)
            {

                if (endDate < DateTime.Now.Date)
                {
                    endDate = DateTime.Now.Date;
                }
            }

            var dates = await db.Calendar.Where(x => x.Date >= startDate && x.Date <= endDate).ToListAsync();
            var sendData = new List<BurnItem>();

            foreach (var d in dates)
            {
                int? sumDue = 0;
                int? sumAct = 0;

                foreach (var e in expec)
                {
                    if (e.DueBy >= d.Date)
                    {
                        Debug.WriteLine(e.Total.ToString());
                        sumDue += e.Total;
                    }
                }

                foreach (var a in actual) { if (a.CompletedOn <= d.Date) { sumAct += a.Total; } }
                sendData.Add(new BurnItem { Date = d.Date, Expected = sumDue, Actual = totalDue - sumAct });
            }




            return Ok(sendData);
        }
        public class BurnItem
        {
            public DateTime Date { get; set; }
            public int? Expected { get; set; }
            public int? Actual { get; set; }
        }
    }
}