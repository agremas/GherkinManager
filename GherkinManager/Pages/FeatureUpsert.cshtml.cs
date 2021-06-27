using GherkinManager.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace GherkinManager.Pages
{
    public class FeatureUpsertModel : PageModel
    {
        private ApplicationDbContext _dataBase;

        public FeatureUpsertModel(ApplicationDbContext dataBase)
        {
            _dataBase = dataBase;
        }

        public int ProjectId
        {
            get
            {
                return (int)HttpContext.Session.GetInt32("ProjectId");
            }
        }

        [BindProperty]
        public Feature Feature { get; set; }

        public async Task<IActionResult> OnGet(int projectId, int? id)
        {
            Feature = new Feature();
            HttpContext.Session.SetInt32("ProjectId", projectId);

            if (id == null)
            {
                // create
                return Page();
            }
            else
            {
                Feature = await _dataBase.Feature.FirstOrDefaultAsync(x => x.Id == id);
                if (Feature == null)
                {
                    return NotFound();
                }

                return Page();
            }
        }

        public async Task<IActionResult> OnPost()
        {
            if (ModelState.IsValid)
            {
                if (Feature.Id == 0)
                {
                    Feature.ProjectId = ProjectId;
                    _dataBase.Feature.Add(Feature);
                }
                else
                {
                    _dataBase.Feature.Update(Feature);
                }

                await _dataBase.SaveChangesAsync();

                return RedirectToPage("ProjectUpsert", new { id = Feature.ProjectId });
            }

            return RedirectToPage();
        }
    }
}
