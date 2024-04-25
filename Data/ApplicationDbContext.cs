using Fpt.Areas.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Fpt.Models;

namespace Fpt.Data
{
    public class ApplicationDbContext : IdentityDbContext<Application>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Fpt.Models.JobList> JobList { get; set; } = default!;
        public DbSet<Fpt.Models.JobApplication> JobApplication { get; set; } = default!;
    }
}