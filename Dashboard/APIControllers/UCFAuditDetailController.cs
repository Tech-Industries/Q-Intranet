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
    public class UCFAuditDetailController : ApiController
    {
        private DashboardEntities db = new DashboardEntities();

        [ResponseType(typeof(List<object>))]
        public List<object> Get(int ID)
        {
            UCFAudit audit = db.UCFAudits.Where(x => x.AreaID == ID).OrderByDescending(o => o.DateCompleted).ToList<UCFAudit>()[0];
            return db.UCFAuditDetails.Where(x => x.AuditID == audit.ID).OrderBy(o => o.ID).ToList<object>();

        }



        [ResponseType(typeof(UCFAuditDetailViewModel))]
        public async Task<IHttpActionResult> Post(UCFAuditDetail audit)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); 
            }
            db.UCFAuditDetails.Add(audit);
            await db.SaveChangesAsync();

            return Ok(UCFAuditDetailViewModel.MapFrom(audit));

        }
    }
}

