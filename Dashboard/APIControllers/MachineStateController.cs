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
    public class MachineStateController : ApiController
    {
        private DashboardEntities db = new DashboardEntities();
        private NKCCONNECTEntities nkdb = new NKCCONNECTEntities();
        private PMMCONNECTEntities pmmdb = new PMMCONNECTEntities();

        // GET: api/Organizations
        //[ResponseType(typeof(List<CURRENT_MACHINE_INFORMATION>))]
        public List<object> Get(string location)
        {
            if (location == "BEST")
            {
                return nkdb.CurrentMachineStates.OrderBy(o => o.RESOURCE_ID).ToList<object>();
            }
            else
            {
                return pmmdb.JP_CurrentMachineState.OrderBy(o => o.RESOURCE_ID).ToList<object>();
            }
        }
        //{
        //        return Ok(await MachineStateCurrentViewModel.MapFromAsync(db.MachineStateCurrents.OrderBy(o => o.Machine).ToList()));
        //}

        [ResponseType(typeof(Object))]
        public List<object> Get(string location, string machine)
        {
            if (machine == "All") {
                if (location == "BEST")
                {
                    return nkdb.TimeInStateByMachines.ToList<object>();
                }
                else
                {
                    return pmmdb.JP_TimeInStateByMachine.ToList<object>();
                }
            }
            else
            {
                if (location == "BEST")
                {
                    return nkdb.TimeInStateByMachines.Where(x => x.Machine == machine).ToList<object>();
                }
                else
                {
                    return pmmdb.JP_TimeInStateByMachine.Where(x => x.Machine == machine).ToList<object>();
                }
            }
        }
    }
}