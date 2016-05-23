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
    public class UserGroupsController : ApiController
    {
        private DashboardEntities db = new DashboardEntities();

        //[ResponseType(typeof(List<UserGroupsViewModel>))]
        //public async Task<IHttpActionResult> Get()
        //{
        //    return Ok(await UserGroupsViewModel.MapFromAsync(db.UserGroups.ToList()));
        //}

        [ResponseType(typeof(List<UserGroupsViewModel>))]
        public async Task<IHttpActionResult> Get(int? userID, int? groupID)
        {
            return Ok(await UserGroupsViewModel.MapFromAsync(db.UserGroups.Where(x => x.GroupID == groupID && x.UserID == userID).ToList()));
        }

        

        [ResponseType(typeof(List<UserGroupsViewModel>))]
        public async Task<IHttpActionResult> Get(int? userID)
        {
            return Ok(await UserGroupsViewModel.MapFromAsync(db.UserGroups.Where(x => x.UserID == userID).ToList()));
        }

        // Add UserGroup
        [ResponseType(typeof(UserGroupsViewModel))]
        public async Task<IHttpActionResult> Post(UserGroup userGroup)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            db.UserGroups.Add(userGroup);
            await db.SaveChangesAsync();

            return Ok(UserGroupsViewModel.MapFrom(userGroup));
        }



        //Remove UserGroup
        [ResponseType(typeof(UserGroupsViewModel))]
        public async Task<IHttpActionResult> Delete(long id)
        {

            UserGroup userGroup = await db.UserGroups.FindAsync(id);
            if (userGroup == null)
            {
                return NotFound();
            }
            else
            {
                db.UserGroups.Remove(userGroup);
                await db.SaveChangesAsync();
                return Ok(UserGroupsViewModel.MapFrom(userGroup));
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