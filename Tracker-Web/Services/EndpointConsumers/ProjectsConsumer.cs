using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Tracker_Web.Model.Project;

namespace Tracker_Web.Services.EndpointConsumers
{
    public class ProjectsConsumer
    {
        private readonly IApiConsumer api;
        
        public ProjectsConsumer(HttpClient http, IJSRuntime js)
        {
            api = new Consumer(http, js);
        }

        public ProjectsConsumer(IApiConsumer api)
        {
            this.api = api;
        }

        public async Task<Project> GetProject(Guid projectId)
        {
            if (projectId == null)
            {
                return null;
            }

            var response = await api.MakeEmptyRequest<Project>(MethodType.GET, "/api/projects/" + projectId.ToString());
            var code = response.Item2;

            switch(code)
            {
                case System.Net.HttpStatusCode.OK:
                    Project proj = response.Item1;
                    return proj;
                default:
                    return null;
            }
        }

        public async Task<Guid> CreateProject(string title)
        {
            if (title == null || title == "")
            {
                return Guid.Empty;
            }

            PostProjIn body = new PostProjIn() { Title = title };

            var response = await api.MakeRequest<PostProjOut, PostProjIn>(MethodType.POST, "/api/projects", body);
            var code = response.Item2;

            switch(code)
            {
                case System.Net.HttpStatusCode.Created:
                    string idRaw = response.Item1.Id;
                    return Guid.Parse(idRaw);
                default:
                    return Guid.Empty;
            }
        }
    }
}
