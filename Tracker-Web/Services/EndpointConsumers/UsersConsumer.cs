using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Tracker_Web.Model.User;

namespace Tracker_Web.Services.EndpointConsumers
{
    public class UsersConsumer
    {
        private readonly IApiConsumer api;
        private readonly NavigationManager navManager;

        public UsersConsumer(HttpClient http, IJSRuntime js, NavigationManager navManager)
        {
            api = new Consumer(http, js);
            this.navManager = navManager;
        }

        public UsersConsumer(IApiConsumer api, NavigationManager navManager)
        {
            this.api = api;
            this.navManager = navManager;
        }

        public async Task<User> GetCurrent()
        {
            var response = await api.MakeEmptyRequest<User>(MethodType.GET, "/api/users/current");
            var code = response.Item2;

            switch(code)
            {
                case System.Net.HttpStatusCode.OK:
                    User current = response.Item1;
                    return current;
                default:
                    navManager.NavigateTo("login", forceLoad: true);
                    return null;
            }
        }

        public async Task<User> GetById(Guid userId)
        {
            var response = await api.MakeEmptyRequest<User>(MethodType.GET, "/api/users/" + userId.ToString());
            var code = response.Item2;

            switch(code)
            {
                case System.Net.HttpStatusCode.OK:
                    User user = response.Item1;
                    return user;
                default:
                    return null;
            }
        }
    }
}
