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
    public class UsersController : ApiController
    {
        private DashboardEntities db = new DashboardEntities();


        //Get All Users
        [ResponseType(typeof(List<UsersViewModel>))]
        public async Task<IHttpActionResult> Get()
        {
            return Ok(await UsersViewModel.MapFromAsync(db.Users.Where(x => x.UserName.Contains("")).OrderBy(o => o.FirstName).ToList()));
        }


        //Get selected user
        [ResponseType(typeof(List<UsersViewModel>))]
        public async Task<IHttpActionResult> Get(int ID)
        {
            return Ok(await UsersViewModel.MapFromAsync(db.Users.Where(x => x.ID.Equals(ID)).ToList()));
        }


        [ResponseType(typeof(List<UsersViewModel>))]
        public async Task<IHttpActionResult> Get(string type)
        {
            if(type == "Onboarding")
            {
                return Ok(await db.Users.Join(db.UserGroups, u => u.ID, ug => ug.UserID, (u, ug) => new { u, ug }).Where(x => x.ug.GroupID == 17).Select(s => new { s.u.ID, s.u.UserName, s.u.FirstName, s.u.LastName, s.u.Email }).OrderBy(o => o.FirstName).ToListAsync<object>());
            }
            return NotFound();            
        }

        [ResponseType(typeof(List<object>))]
        public List<object> Get(int ID, string type)
        {
            if(type == "authorized")
            {
                return db.Users.Join(db.UserGroups, u => u.ID, ug => ug.UserID, (u, ug) => new { u, ug }).Where(x => x.ug.GroupID == ID).Select(s => new { s.u.ID, s.u.UserName, s.u.FirstName, s.u.LastName, s.u.Email}).OrderBy(o => o.FirstName).ToList<object>();
            }
            else if(type == "directReports")
            {
                return db.Users.Join(db.UserGroups, u => u.ID, ug => ug.UserID, (u, ug) => new { u, ug }).Where(x => x.ug.GroupID == 2 && x.u.SupervisorID == ID).Select(s => new { s.u.ID, s.u.UserName, s.u.FirstName, s.u.LastName, s.u.Email }).OrderBy(o => o.FirstName).ToList<object>();
                
            }
            else
            {
                return new List<object>();
            }
        }

        // POST: api/Roles
        [ResponseType(typeof(UsersViewModel))]
        public async Task<IHttpActionResult> Post(User user)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }
            user.Email = user.Email.ToLower();
            db.Users.Add(user);
            await db.SaveChangesAsync();

            return Ok(UsersViewModel.MapFrom(user));
        }

        //[HttpPost, ActionName("Delete")]
        //public async Task<IHttpActionResult> Delete(long id)
        //{
        //    User user = await db.Users.FindAsync(id);
        //    db.Users.Remove(user);
        //    await db.SaveChangesAsync();
        //    return Ok(UsersViewModel.MapFrom(user));
        //}


        //Remove
        [ResponseType(typeof(UsersViewModel))]
        public async Task<IHttpActionResult> Delete(long id)
        {
            User user = await db.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            else
            {
                db.Users.Remove(user);
                await db.SaveChangesAsync();
                return Ok(UsersViewModel.MapFrom(user));
            }


        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}