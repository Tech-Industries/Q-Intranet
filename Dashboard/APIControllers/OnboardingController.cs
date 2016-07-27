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

namespace Dashboard.APIControllers
{
    public class OnboardingController : ApiController
    {
        private OnBoardingEntities1 onbdb = new OnBoardingEntities1();


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
    }
}