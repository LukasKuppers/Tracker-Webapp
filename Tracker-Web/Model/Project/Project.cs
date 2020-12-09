using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tracker_Web.Model.Project
{
    public class Project
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public DateTime DateCreated { get; set; }

        public Guid Owner { get; set; }

        public List<Guid> Members { get; set; }

        public List<Guid> Tasks { get; set; }
    }   
}
