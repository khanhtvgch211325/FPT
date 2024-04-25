using Fpt.Areas.Data;

namespace Fpt.Models
{
    public class JobApplication
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string DescribeYourself {  get; set; }
        
        public int JobListId { get; set; }
        public virtual JobList? JobList { get; set; }
        public string ApplicationId { get; set; }
        public virtual Application? Application { get; set; }
    }
}
