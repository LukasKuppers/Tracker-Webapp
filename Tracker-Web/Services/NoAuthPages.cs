using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tracker_Web.Services
{
    public class NoAuthPages
    {
        NavigationManager navManager;

        public NoAuthPages(NavigationManager navManager)
        {
            this.navManager = navManager;
        }

        public List<string> GetPages()
        {
            return new List<string>
            {
                navManager.BaseUri,
                navManager.BaseUri + "login",
            };
        }

        public bool IsNoAuthPage(string uri)
        {
            List<string> pages = GetPages();
            return pages.Contains(uri);
        }
    }
}
