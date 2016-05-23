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
    public class GroupsController : ApiController
    {
        private DashboardEntities db = new DashboardEntities();

        //GET All Groups
        [ResponseType(typeof(List<GroupsViewModel>))]
        public async Task<IHttpActionResult> Get()
        {
            return Ok(await GroupsViewModel.MapFromAsync(db.Groups.ToList()));
            
        }


        
        [ResponseType(typeof(List<GroupsViewModel>))]
        public async Task<IHttpActionResult> Get(int ID)
        {
            //return Ok(await GroupsViewModel.MapFromAsync(db.Groups.Join(db.UserGroups, g => g.ID, ug => ug.GroupID, (g, ug) => new { ID = g.ID, Category = g.Category, Access = g.Access, Description = g.Description, UserID = ug.UserID }).ToList().ConvertAll(x => new Group { ID = x.ID, Category = x.Category, Access = x.Access, Description = x.Description })));
            return Ok(await GroupsViewModel.MapFromAsync(db.Groups.Where(x => x.ID.Equals(ID)).ToList()));
        }

        //GET Specific User Assigned/Unassigned Groups
        [ResponseType(typeof(List<GroupsViewModel>))]
        public async Task<IHttpActionResult> Get(int ID, string ua)
        {
            if (ua == "assigned")
            {
                return Ok(await GroupsViewModel.MapFromAsync(db.Groups.Join(db.UserGroups, u => u.ID, ug => ug.GroupID, (u, ug) => new { u, ug }).Where(x => x.ug.UserID == ID).Select(x => x.u).Distinct().ToList()));
            }
            else
            {
                return Ok(await GroupsViewModel.MapFromAsync((from g in db.Groups
                                                              where !db.UserGroups.Any(ug => (ug.GroupID == g.ID) && (ug.UserID == ID))
                                                              select g).ToList()));
            }
            
            //return Ok(await GroupsViewModel.MapFromAsync(db.Groups.Join(db.UserGroups, g => g.ID, ug => ug.GroupID, (g, ug) => new { ID = g.ID, Category = g.Category, Access = g.Access, Description = g.Description, UserID = ug.UserID }).ToList().ConvertAll(x => new Group { ID = x.ID, Category = x.Category, Access = x.Access, Description = x.Description })));
            
        }



        // Add Group
        [ResponseType(typeof(GroupsViewModel))]
        public async Task<IHttpActionResult> Post(Group Group)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            db.Groups.Add(Group);
            await db.SaveChangesAsync();

            return Ok(GroupsViewModel.MapFrom(Group));
        }
        


        //Remove
        [ResponseType(typeof(GroupsViewModel))]
        public async Task<IHttpActionResult> Delete(long id)
        {
            Group Group = await db.Groups.FindAsync(id);
            if (Group == null)
            {
                return NotFound();
            }
            else
            {
                db.Groups.Remove(Group);
                await db.SaveChangesAsync();
                return Ok(GroupsViewModel.MapFrom(Group));
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