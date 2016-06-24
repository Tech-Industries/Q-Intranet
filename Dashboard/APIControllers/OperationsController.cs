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
    public class OperationsController : ApiController
    {
        private DashboardEntities db = new DashboardEntities();

        [ResponseType(typeof(List<object>))]
        public List<object> Get(string Type)
        {
            return db.ShipAdjDates.OrderBy(o => o.DateDue).ToList<object>();
        }


        [ResponseType(typeof(List<object>))]
        public async Task<IHttpActionResult> Post(ShippingPlanAdjShipDate s)
        {
            db.ShippingPlanAdjShipDates.Add(s);
            await db.SaveChangesAsync();


            return Ok(await ShipAdjViewModel.MapFromAsync(s));
        }

        [ResponseType(typeof(void))]
        public void Put([FromBody]ShippingPlanAdjShipDate s)
        {
            int ID = s.ID;

            if (!ModelState.IsValid)
            {
                System.Diagnostics.Debug.WriteLine("BadModelState");
            }

            if (ID != s.ID)
            {
                System.Diagnostics.Debug.WriteLine("BadReq");
            }

            ShippingPlanAdjShipDate ups = (ShippingPlanAdjShipDate)db.ShippingPlanAdjShipDates.Where(x => x.ID == ID).First();
            ups.AdjShipDate= s.AdjShipDate;

            db.ShippingPlanAdjShipDates.Attach(ups);
            var entry = db.Entry(ups);
            entry.Property(e => e.AdjShipDate).IsModified = true;
            // other changed properties
            db.SaveChanges();

            //db.Entry(deldet).State = EntityState.Modified;
            //System.Diagnostics.Debug.WriteLine("Modified");
            //try
            //{
            //    await db.SaveChangesAsync();
            //}
            //catch (DbUpdateConcurrencyException)
            //{
            //    if (!PTasksExists(ID))
            //    {
            //        System.Diagnostics.Debug.WriteLine("Catch If");
            //        return NotFound();
            //    }
            //    else {
            //        System.Diagnostics.Debug.WriteLine("Catch If Else");
            //        throw;
            //    }
            //}
            
        }
    }
}