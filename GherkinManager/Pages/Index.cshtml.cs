using GherkinManager.Model;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GherkinManager.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _dataBase;
        public IndexModel(ApplicationDbContext dataBase)
        {
            _dataBase = dataBase;
        }

        public IEnumerable<Model.Project> Projects { get; set; }
        public IEnumerable<Model.Feature> Features { get; set; }

        public int CountFeatures(int projectId) => Features.Where(x => x.ProjectId == projectId).Count();

        public async Task OnGet()
        {
            Projects = await _dataBase.Project.OrderBy(x => x.Name).ToListAsync();
            Features = await _dataBase.Feature.ToListAsync();
        }
    }
}
