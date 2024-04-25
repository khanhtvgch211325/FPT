using Fpt.Areas.Data;

namespace Fpt.Models
{
    public class JobList
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string RequiredSkills { get; set; }
        public DateTime DateTime { get; set; }


        public string ApplicationId { get; set; }
        public virtual Application? Application { get; set; }
        public virtual ICollection<JobApplication>? JobApplication { get; set; }
    }
}
