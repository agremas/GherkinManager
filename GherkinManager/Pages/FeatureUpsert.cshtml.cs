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
        private GherkinClient _client;

        public FeatureUpsertModel(ApplicationDbContext dataBase, GherkinClient client)
        {
            _dataBase = dataBase;
            _client = client;
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
                Feature.Content = await _client.GetSampleFeature();
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
                var validationResult = await _client.ValidateFeature(Feature.Content);
                if (validationResult)
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
                else
                {
                    ModelState.AddModelError("Feature.Content", "Validation error");
                    return Page();
                }
                
            }

            return RedirectToPage();
        }
    }
}
