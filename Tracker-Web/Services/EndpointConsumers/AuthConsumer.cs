using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Tracker_Web.Model;
using Tracker_Web.Model.Login;
using Tracker_Web.Model.Register;

namespace Tracker_Web.Services.EndpointConsumers
{
    public class AuthConsumer
    {
        private readonly IApiConsumer api;
        private readonly NavigationManager navManager;

        public AuthConsumer(HttpClient http, IJSRuntime js, NavigationManager navManager)
        {
            api = new Consumer(http, js);
            this.navManager = navManager;
        }

        public AuthConsumer(IApiConsumer api, NavigationManager navManager)
        {
            this.api = api;
            this.navManager = navManager;
        }

        public async Task<string> CheckAuth()
        {
            var response = await api.MakeEmptyRequest<AuthStatusOut>(MethodType.GET, "/api/authorization");
            var code = response.Item2;

            switch(code)
            {
                case System.Net.HttpStatusCode.OK:
                    string role = response.Item1.Role;
                    return role;
                default:
                    navManager.NavigateTo("login", forceLoad: true);
                    return null;
            }
        }

        public async Task<string> Login(LoginIn credentials)
        {
            var response = await api.MakeRequest<LoginOut, LoginIn>(MethodType.PUT, "/api/authorization", credentials);
            var code = response.Item2;

            switch(code)
            {
                case System.Net.HttpStatusCode.OK:
                    string sessionId = response.Item1.SessionId;
                    return sessionId;
                default:
                    return null;
            }
        }
    }
}
