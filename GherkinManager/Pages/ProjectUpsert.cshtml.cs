using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GherkinManager.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace GherkinManager.Pages
{
    public class ProjectUpsertModel : PageModel
    {
        private ApplicationDbContext _dataBase;

        public ProjectUpsertModel(ApplicationDbContext dataBase)
        {
            _dataBase = dataBase;
        }

        [BindProperty]
        public Project Project { get; set; }

        public List<Feature> Features { get; set; }

        public async Task<IActionResult> OnGet(int? id)
        {
            Project = new Project();
            if (id == null)
            {
                // create
                Features = new List<Feature>();
                return Page();
            }
            else
            {
                // update
                Project = await _dataBase.Project.FirstOrDefaultAsync(x => x.Id == id);
                if (Project == null)
                {
                    return NotFound();
                }
                else
                {
                    Features = await _dataBase.Feature.Where(x => x.ProjectId == id).ToListAsync();
                }
                return Page();
            }
        }

        public async Task<IActionResult> OnPost()
        {
            if (ModelState.IsValid)
            {
                if (Project.Id == 0)
                {
                    _dataBase.Project.Add(Project);
                }
                else
                {
                    _dataBase.Project.Update(Project);
                }
                await _dataBase.SaveChangesAsync();

                return RedirectToPage("Index");
            }

            return RedirectToPage();
        }


        public async Task<IActionResult> OnPostDelete(int id)
        {
            var feature = await _dataBase.Feature.FindAsync(id);
            if (feature == null)
            {
                return NotFound();
            }
            _dataBase.Feature.Remove(feature);
            await _dataBase.SaveChangesAsync();

            return RedirectToPage("ProjectUpsert", new { id = feature.ProjectId });

        }
    }
}
