using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tracker_Server.Services.ActionFilters;
using Tracker_Server.Services.DataAccess;
using Tracker_Server.Services.Constants;
using Tracker_Server.Models.Projects;
using Tracker_Server.Models.Users;

namespace Tracker_Server.Controllers
{
    [Route("api/projects")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        [HttpGet("{projectId}")]
        [AuthorizationFilter]
        public ActionResult<Project> GetProject(string projectId)
        {
            if (projectId == null || projectId == "")
            {
                return BadRequest();
            }

            bool success = Guid.TryParse(projectId, out Guid id);
            if (!success)
            {
                return BadRequest();
            }

            User user = (User) HttpContext.Items["currentUser"];
            IResource resources = new Resource();
            IDbClient db = new DbClient(resources.GetString("db_base_path"));

            if (db.Contains<Project, Guid>(resources.GetString("db_projects_path"), "_id", id))
            {
                Project project = db.FindByField<Project, Guid>(resources.GetString("db_projects_path"), "_id", id)[0];
                if (project.Owner == user.Id || project.Members.Contains(user.Id))
                {
                    return Ok(project);
                }
                return Forbid();
            }
            return NotFound();
        }

        [HttpGet("list/{userId}")]
        [AuthorizationFilter]
        public ActionResult<GetProjsOut> GetProjects(string userId)
        {
            if (userId == null || userId == "")
            {
                return BadRequest();
            }

            bool success = Guid.TryParse(userId, out Guid id);
            if (!success)
            {
                return BadRequest();
            }

            IResource resources = new Resource();
            IDbClient db = new DbClient(resources.GetString("db_base_path"));

            if (db.Contains<User, Guid>(resources.GetString("db_users_path"), "_id", id))
            {
                User user = db.FindByField<User, Guid>(resources.GetString("db_users_path"), "_id", id)[0];
                var projects = user.Projects;
                GetProjsOut response = new GetProjsOut() { Projects = new List<ProjectMinimal>() };
                foreach(Guid projId in projects)
                {
                    Project project = db.FindByField<Project, Guid>(resources.GetString("db_projects_path"), "_id", projId)[0];
                    ProjectMinimal item = new ProjectMinimal()
                    {
                        Id = projId,
                        Title = project.Title,
                        DateCreated = project.DateCreated,
                        Owner = project.Owner
                    };
                    response.Projects.Add(item);
                }
                return Ok(response);
            }
            return NotFound();
        }

        [HttpPost]
        [AuthorizationFilter]
        public ActionResult<PostProjOut> CreateProject(PostProjIn projectTitle)
        {
            if (projectTitle == null || projectTitle.Title == "")
            {
                return BadRequest();
            }

            User currentUser = (User) HttpContext.Items["currentUser"];
            Project project = new Project()
            {
                Id = Guid.NewGuid(),
                Title = projectTitle.Title, 
                DateCreated = DateTime.Now, 
                Owner = currentUser.Id, 
                Members = new List<Guid>(), 
                Tasks = new List<Guid>()
            };

            IResource resources = new Resource();
            IDbClient db = new DbClient(resources.GetString("db_base_path"));
            db.InsertRecord(resources.GetString("db_projects_path"), project);

            var projects = currentUser.Projects;
            projects.Add(project.Id);
            db.UpdateRecord<User, List<Guid>>(resources.GetString("db_users_path"), currentUser.Id, "Projects", projects);

            PostProjOut responseBody = new PostProjOut()
            {
                Id = project.Id
            };

            return CreatedAtAction("createProject", responseBody);
        }
    }
}
