using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Energy_Saver.Pages
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public Records Records { get; set; }
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }
        

        public void OnGet()
        {

        }
    }
}