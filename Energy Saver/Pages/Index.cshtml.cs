using Energy_Saver.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Energy_Saver.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        [BindProperty]
        public List<List<Taxes>>? Taxes { get; set; }

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            Taxes = Serialization.ReadFromFile();
        }

        public void OnPost()
        {

        }

        public IActionResult OnPostDelete(int index, int yearIndex)
        {
            Taxes = Serialization.ReadFromFile();
            Taxes[yearIndex].RemoveAt(index);
            Serialization.WriteText(Taxes.SelectMany(list => list).Distinct().ToList());

            return RedirectToPage();
        }
    }
}