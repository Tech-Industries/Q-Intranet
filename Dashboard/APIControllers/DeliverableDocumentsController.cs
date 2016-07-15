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
using System.Web;
using System.Web.Http.Results;
using System.Web.Mvc;
using System.Net.Http.Headers;
using System.IO;

namespace Dashboard.APIControllers
{
    public class DeliverableDocumentsController : ApiController
    {
        private DashboardEntities db = new DashboardEntities();
        // GET: api/Organizations


        [ResponseType(typeof(DeliverableDocumentsViewModel))]
        public async Task<IHttpActionResult> Get(int DelDetID)
        {

            return Ok(await DeliverableDocumentsViewModel.MapFromAsync(db.DeliverableDocuments.Where(x => x.DelDetID == DelDetID).ToList()));
        }



        // POST: Deliverable Document
        [ResponseType(typeof(DeliverableDocumentsViewModel))]
        public async Task<IHttpActionResult> Post(DeliverableDocument delr)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            db.DeliverableDocuments.Add(delr);
            await db.SaveChangesAsync();

            return Ok(DeliverableDocumentsViewModel.MapFrom(delr));
        }


        [ResponseType(typeof(DeliverableDocumentsViewModel))]
        public async Task<IHttpActionResult> Delete(long id)
        {
            DeliverableDocument c = await db.DeliverableDocuments.FindAsync(id);
            if (c == null)
            {
                return NotFound();
            }
            else
            {
                db.DeliverableDocuments.Remove(c);
                await db.SaveChangesAsync();
                return Ok(DeliverableDocumentsViewModel.MapFrom(c));
            }
    }
}
}