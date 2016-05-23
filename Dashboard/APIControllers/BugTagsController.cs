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
    public class BugTagsController : ApiController
    {
        private DashboardEntities db = new DashboardEntities();

        [ResponseType(typeof(BugTagsViewModel))]
        public async Task<IHttpActionResult> Post(BugTag bTag)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }
            db.BugTags.Add(bTag);
            await db.SaveChangesAsync();

            return Ok(BugTagsViewModel.MapFrom(bTag));
        }
    }
}