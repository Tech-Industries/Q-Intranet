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

namespace Dashboard.APIControllers
{
    public class UCFActionsController : ApiController
    {
        private DashboardEntities db = new DashboardEntities();

        [ResponseType(typeof(List<object>))]
        public List<object> Get(int ID)
        {
            DateTime now = new DateTime();
            now = DateTime.Now;
            return db.UCFActions.Where(x => x.AreaID == ID && x.DateStart <= now && x.DateDue >= now).OrderBy(o => o.DateDue).ToList<object>();
            
        }

        [ResponseType(typeof(List<object>))]
        public void Post(UCFAction action)
        {
            db.UCFActions.Add(action);
            db.SaveChangesAsync();
        }

        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> Put([FromBody]UCFAction action)
        {
            int ID = action.ID;
            System.Diagnostics.Debug.WriteLine(action.Status);

            if (!ModelState.IsValid)
            {
                System.Diagnostics.Debug.WriteLine("BadModelState");
                return BadRequest(ModelState);
            }

            if (ID != action.ID)
            {
                System.Diagnostics.Debug.WriteLine("BadReq");
                return BadRequest();
            }

            UCFAction uaction = (UCFAction)db.UCFActions.Where(x => x.ID == ID).First();
            uaction.Status = action.Status;
            uaction.DateComplete = action.DateComplete;


            db.UCFActions.Attach(uaction);
            var entry = db.Entry(uaction);

            entry.Property(e => e.Status).IsModified = true;
            entry.Property(e => e.DateComplete).IsModified = true;

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
                if (!ActionExists(ID))
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

        public  void Delete(long id)
        {
            UCFAction a =  db.UCFActions.Find(id);
            if (a == null)
            {
            }
            else
            {
                db.UCFActions.Remove(a);
                 db.SaveChangesAsync();
            }


        }

        private bool ActionExists(long ID)
        {
            return db.UCFActions.Count(e => e.ID == ID) > 0;
        }
    }
}