﻿using System;
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

            PostProjOut responseBody = new PostProjOut()
            {
                Id = project.Id
            };

            return CreatedAtAction("createProject", responseBody);
        }
    }
}
