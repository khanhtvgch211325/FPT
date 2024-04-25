using Fpt.Models;
using Microsoft.AspNetCore.Identity;

namespace Fpt.Areas.Data
{
    public class Application : IdentityUser

    {
        public string UserRole { get; set; }
        public virtual ICollection<JobApplication>? JobApply { get; set; }
        public virtual ICollection<JobList>? JobList { get; set; }

    }
}
