using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tracker_Server.Models.Projects
{
    public class GetProjsOut
    {
        public List<ProjectMinimal> Projects { get; set; }
    }

    public class ProjectMinimal
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public DateTime DateCreated { get; set; }

        public Guid Owner { get; set; }
    }
}
