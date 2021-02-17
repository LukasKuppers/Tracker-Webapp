using System.ComponentModel.DataAnnotations;

namespace Tracker_Web.Model.Project
{
    public class NewProjectForm
    {
        [Required]
        [DataType(DataType.Text)]
        public string Title { get; set; }
    }
}
