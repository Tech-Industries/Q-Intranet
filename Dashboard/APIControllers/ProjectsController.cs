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
//using Dashboard.Models;
using Dashboard.ViewModels;
using Orizon.Web.Data;
using System.Web.Http.Results;

namespace Dashboard.APIControllers
{
    public class Project2Controller : ApiController
    {
        private OrizonEntities db = new OrizonEntities();
        //private OrizonDevEntities db = new OrizonDevEntities();

        #region Projects




        [Route("api/v1/projects")]
        [HttpGet]
        public async Task<IHttpActionResult> GetProjects()
        {
            return await GetProjects();

        }

        [Route("api/v1/projects")]
        [HttpGet]
        public async Task<IHttpActionResult> GetProjects(bool? top = false)
        {

            if (top.HasValue || !top.Value)
            {
                return Ok(await db.Projects.ToListAsync());
            }

            return Ok(await db.ProjectsTopLevels.ToListAsync());

        }


        [Route("api/v1/projects/{id:int}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetProjectId(int id)
        {
            return await GetProjectId(id);
        }

        [Route("api/v1/projects/{id:int}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetProjectId(int id, bool? top = false)
        {

            if (!top.Value)
            {
                var project = await db.Projects.FindAsync(id);
                if (project == null)
                {
                    return NotFound();
                }
                return Ok(project);
            }

            var projectTop = await db.ProjectsTopLevels.FindAsync(id);
            if (projectTop == null)
            {
                return NotFound();
            }
            return Ok(projectTop);

        }

        [Route("api/v1/projects")]
        [HttpPost]
        public async Task<IHttpActionResult> PostProject(Project project)
        {
            if (project == null)
            {
                return NotFound();
            }
            try
            {
                db.Projects.Add(project);
                await db.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return new ResponseMessageResult(new HttpResponseMessage(HttpStatusCode.BadRequest));
            }
            return new ResponseMessageResult(new HttpResponseMessage(HttpStatusCode.Created));
        }


        [Route("api/v1/projects/{id:int}")]
        [HttpPut]
        public async Task<IHttpActionResult> PutProject(int id, Project project)
        {


            var oldProject = await db.Projects.FindAsync(id);

            if (oldProject == null || project == null)
            {
                return NotFound();
            }
            try
            {

                var entry = db.Entry(oldProject);

                oldProject.Name = project.Name;
                oldProject.Description = project.Description;
                oldProject.Status = project.Status;
                oldProject.DateCreated = project.DateCreated;
                oldProject.CreatorID = project.CreatorID;
                oldProject.Priority = project.Priority;

                await db.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return new ResponseMessageResult(new HttpResponseMessage(HttpStatusCode.BadRequest));
            }

            return Ok(oldProject);
        }

        [Route("api/v1/projects/{id:int}")]
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteProject(int id)
        {
            try
            {
                db.Projects.Remove(await db.Projects.FindAsync(id));
                await db.SaveChangesAsync();
            }
            catch
            {
                return new ResponseMessageResult(new HttpResponseMessage(HttpStatusCode.BadRequest));
            }
            return Ok();
        }
        
        #endregion

        #region ProjectAssignees
        [Route("api/v1/projects/{projectId:int}/assignees")]
        [HttpGet]
        public async Task<IHttpActionResult> GetProjectAssignees(int projectId)
        {
            var assignees = await db.ProjectAssignees.Where(x => x.ProjID == projectId).ToListAsync();
            if (assignees == null)
            {
                return NotFound();
            }

            return Ok(assignees);
        }

        [Route("api/v1/projects/{projectId:int}/assignees")]
        [HttpPost]
        public async Task<IHttpActionResult> PostProjectAssingee(int projectId, ProjectAssignee assignee)
        {
            if (assignee == null)
            {
                return NotFound();
            }
            try
            {
                db.ProjectAssignees.Add(assignee);
                await db.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return new ResponseMessageResult(new HttpResponseMessage(HttpStatusCode.BadRequest));
            }
            return new ResponseMessageResult(new HttpResponseMessage(HttpStatusCode.Created));
        }

        [Route("api/v1/projects/{projectId:int}/assignees/{assigneeId:int}")]
        [HttpDelete]
        public async Task<IHttpActionResult> DelteProjectAssignee(int projectID, int assigneeId)
        {
            try
            {
                db.ProjectAssignees.Remove(db.ProjectAssignees.Where(x => x.ProjID == projectID && x.AssigneeID == assigneeId).First());
                await db.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return NotFound();
            }
            return Ok();


        }

        [Route("api/v1/projects/assignees/{id:int}")]
        [HttpDelete]
        public async Task<IHttpActionResult> DelteProjectAssigneeById(int id)
        {
            try
            {
                db.ProjectAssignees.Remove(await db.ProjectAssignees.FindAsync(id));
                await db.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return NotFound();
            }
            return Ok();


        }

        #endregion

        #region ProjectTasks

        [Route("api/v1/projects/{projectId:int}/tasks")]
        [HttpGet]
        public async Task<IHttpActionResult> GetProjectTasks(int projectId)
        {
            var tasks = await db.ProjectTasks.Where(x => x.ProjID == projectId).ToListAsync();
            if (tasks == null)
            {
                return NotFound();
            }

            return Ok(tasks);
        }

        [Route("api/v1/projects/{projectId:int}/tasks")]
        [HttpPost]
        public async Task<IHttpActionResult> PostProjectTasks(int projectId, ProjectTask task)
        {
            if (task == null)
            {
                return NotFound();
            }
            try
            {
                db.ProjectTasks.Add(task);
                await db.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return new ResponseMessageResult(new HttpResponseMessage(HttpStatusCode.BadRequest));
            }
            return new ResponseMessageResult(new HttpResponseMessage(HttpStatusCode.Created));
        }

        [Route("api/v1/projects/tasks/{id:int}")]
        [HttpDelete]
        public async Task<IHttpActionResult> DelteProjectTask(int id)
        {
            try
            {
                db.ProjectTasks.Remove(await db.ProjectTasks.FindAsync(id));
                await db.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return NotFound();
            }
            return Ok();


        }

        #endregion

        #region Bugs
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="type"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        [Route("api/v1/projects/{projectId:int}/bugs")]
        [HttpGet]
        [ResponseType(typeof(IEnumerable<Bug>))]
        public async Task<IHttpActionResult> GetProjectBugs(int projectId)
        {

            var bugs = await db.Bugs.Where(x => x.ProjectID == projectId).ToListAsync(); // SELECT * FROM Bugs WHERE Status IN ()
            return Ok(bugs);
        }

        [Route("api/v1/projects/bugs")]
        [HttpGet]
        [ResponseType(typeof(IEnumerable<Bug>))]
        public async Task<IHttpActionResult> GetAllBugs()
        {

            var bugs = await db.Bugs.ToListAsync(); // SELECT * FROM Bugs WHERE Status IN ()
            return Ok(bugs);
        }

        [Route("api/v1/projects/{id:int}/bugs")]
        [HttpGet]
        [ResponseType(typeof(IEnumerable<Bug>))]
        public async Task<IHttpActionResult> GetProjectBugs(int id, string type, string status)
        {
            var bugs = new List<Bug>();

            if (!string.IsNullOrEmpty(type))
            {
                var catnames = type.ToLower().Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                bugs = await db.Bugs.Where(x => catnames.Contains(x.Type.ToLower())).ToListAsync(); // SELECT * FROM Bugs WHERE Status IN ()

            }


            if (!string.IsNullOrEmpty(status))
            {
                var stat = status.ToLower().Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (bugs.Count == 0)
                {
                    bugs = await db.Bugs.Where(x => stat.Contains(x.Status.ToLower())).ToListAsync(); // SELECT * FROM Bugs WHERE Status IN ()
                }
                else
                {
                    bugs = bugs.Where(x => stat.Contains(x.Status.ToLower())).ToList();

                }
            }
            return Ok(bugs);
        }

        [Route("api/v1/projects/bugs/{bugId:int}")]
        [HttpGet]
        [ResponseType(typeof(IEnumerable<Bug>))]
        public async Task<IHttpActionResult> GetBug(int bugId)
        {
            var bug = await db.Bugs.FindAsync(bugId);
            if (bug == null)
            {
                return NotFound();
            }
            return Ok(bug);

        }

        [Route("api/v1/projects/bugs")]
        [HttpPost]
        public async Task<IHttpActionResult> PostBug(Bug bug)
        {
            //db.Projects.Add() add new project
            if (bug == null)
            {
                return NotFound();
            }
            try
            {
                db.Bugs.Add(bug);
                await db.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return new ResponseMessageResult(new HttpResponseMessage(HttpStatusCode.BadRequest));
            }
            return new ResponseMessageResult(new HttpResponseMessage(HttpStatusCode.Created));
        }

        [Route("api/v1/projects/bugs/{id:int}")]
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteBug(int id)
        {
            try
            {
                db.Bugs.Remove(await db.Bugs.FindAsync(id));
                await db.SaveChangesAsync();
            }
            catch
            {
                return new ResponseMessageResult(new HttpResponseMessage(HttpStatusCode.BadRequest));
            }
            return Ok();
        }
        #endregion

        #region BugTags
        [Route("api/v1/projects/bugs/{bugId:int}/tags")]
        [HttpGet]
        public async Task<IHttpActionResult> GetBugTags(int bugId)
        {
            var tags = await db.BugTags.Where(x => x.BugID == bugId).ToListAsync();
            if (tags == null)
            {
                return NotFound();
            }

            return Ok(tags);
        }

        [Route("api/v1/projects/bugs/{bugId:int}/tags")]
        [HttpPost]
        public async Task<IHttpActionResult> PostBugTag(int bugId, BugTag tag)
        {
            if (tag == null)
            {
                return NotFound();
            }
            try
            {
                db.BugTags.Add(tag);
                await db.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return new ResponseMessageResult(new HttpResponseMessage(HttpStatusCode.BadRequest));
            }
            return new ResponseMessageResult(new HttpResponseMessage(HttpStatusCode.Created));
        }

        [Route("api/v1/projects/bugs/tags/{id:int}")]
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteBugTag(int id)
        {
            try
            {
                db.BugTags.Remove(await db.BugTags.FindAsync(id));
                await db.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return NotFound();
            }
            return Ok();


        }

        #endregion

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
        }

    }
    public class ProjectsController : ApiController
    {
        private OrizonEntities db = new OrizonEntities();



        [ResponseType(typeof(List<object>))]
        public List<object> Get()
        {
            return db.ProjectsTopLevels.Where(x => x.Status != "Complete").OrderBy(o => o.ID).ToList<object>();
        }

        public List<object> Get(string Status)
        {
            return db.ProjectsTopLevels.Where(x => x.Status == Status).OrderBy(o => o.ID).ToList<object>();
        }

        [ResponseType(typeof(ProjectsTopLevelViewModel))]
        public async Task<IHttpActionResult> Post(Project proj)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            db.Projects.Add(proj);
            await db.SaveChangesAsync();


            return Ok(await ProjectsTopLevelViewModel.MapFromAsync(db.ProjectsTopLevels.Where(x => x.ID == proj.ID).ToList()));
        }

    }
}