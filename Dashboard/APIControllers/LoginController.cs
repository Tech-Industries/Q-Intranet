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
    public class LoginController : ApiController
    {
        private DashboardEntities db = new DashboardEntities();
        // GET: api/Organizations
        [ResponseType(typeof(List<LoginViewModel>))]
        public async Task<IHttpActionResult> Get(string UserName)
        {
            return Ok(await LoginViewModel.MapFromAsync(db.Users.Where(x => x.UserName.Equals(UserName)).ToList()));
        }
    }
}