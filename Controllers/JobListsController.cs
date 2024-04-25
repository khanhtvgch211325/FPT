using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Fpt.Data;
using Fpt.Models;
using System.Security.Claims;

namespace Fpt.Controllers
{
    public class JobListsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public JobListsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Apply(string searchString)
        {
            var jobLists = from j in _context.JobList
                           select j;

            if (!string.IsNullOrEmpty(searchString))
            {
                jobLists = jobLists.Where(s => s.Name.Contains(searchString) || s.Description.Contains(searchString));
            }
            var applicationDbContext = jobLists.Include(j => j.Application);

            return View(await applicationDbContext.ToListAsync());
        }
    

            public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _context.Users.FindAsync(userId);

            if (user.UserRole == "Employer")
            {
                var jobLists = await _context.JobList
                    .Where(j => j.ApplicationId == userId)
                    .Include(j => j.Application)
                    .ToListAsync();

                return View(jobLists);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jobListing = await _context.JobList
                .Include(j => j.Application)
                .Include(j => j.JobApplication)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (jobListing == null)
            {
                return NotFound();
            }

            return View(jobListing);
        }

        public IActionResult Create()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _context.Users.Find(userId);

            if (user.UserRole == "Employer")
            {
                var jobList = new JobList
                {
                    ApplicationId = userId
                };

                return View(jobList);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,RequiredSkills,DateTime,ApplicationId")] JobList jobList)
        {
            if (ModelState.IsValid)
            {
                _context.Add(jobList);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(jobList);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jobListing = await _context.JobList.FindAsync(id);

            if (jobListing == null)
            {
                return NotFound();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _context.Users.Find(userId);

            if (user.UserRole != "Employer")
            {
                return RedirectToAction("Index", "Home");
            }

            return View(jobListing);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,RequiredSkills,DateTime,ApplicationId")] JobList jobList)
        {
            if (id != jobList.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(jobList);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!JobListExists(jobList.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            return View(jobList);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jobListing = await _context.JobList
                .Include(j => j.Application)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (jobListing == null)
            {
                return NotFound();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _context.Users.Find(userId);

            if (user.UserRole != "Employer")
            {
                return RedirectToAction("Index", "Home");
            }

            return View(jobListing);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var jobListing = await _context.JobList.FindAsync(id);
            if (jobListing != null)
            {
                _context.JobList.Remove(jobListing);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool JobListExists(int id)
        {
            return _context.JobList.Any(e => e.Id == id);
        }
    }
}
