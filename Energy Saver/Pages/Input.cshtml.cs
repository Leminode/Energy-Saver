using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Energy_Saver.Model;
using Energy_Saver.Services;
using Energy_Saver.DataSpace;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Energy_Saver.Pages
{
    public class InputModel : PageModel
    {
        private readonly ILogger<InputModel> _logger;
        private readonly EnergySaverTaxesContext _context;
        private readonly INotificationService _notificationService;

        public delegate void InputTaxesHandler(object source, NotificationService.NotificationArgs args);
        public event InputTaxesHandler InputTaxes;

        [BindProperty]
        public Taxes? Taxes { get; set; }

        public InputModel(ILogger<InputModel> logger, EnergySaverTaxesContext context, INotificationService notificationService)
        {
            _logger = logger;
            _context = context;
            _notificationService = notificationService;
            InputTaxes += _notificationService.CreateNotification;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                OnTaxInputError();
                return Page();
            }

            var tempString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value.Split('|').Last();
            Taxes.UserID = int.Parse(tempString);

            _context.Taxes.Add(Taxes);

            try
            {
                await _context.SaveChangesAsync();
                OnTaxInputSuccess();
            } 
            catch(DbUpdateException)
            {
                OnTaxInputError();
            }

            return RedirectToPage("./Index");
        }

        protected virtual void OnTaxInputSuccess()
        {
            InputTaxes?.Invoke(this, new NotificationService.NotificationArgs
            {
                Message = $"Successfully added entry for {Taxes.Year}-{Serialization.FormatMonth(Taxes.Month)}",
                Type = NotificationService.NotificationType.Success
            });
        }

        protected virtual void OnTaxInputError()
        {
            InputTaxes?.Invoke(this, new NotificationService.NotificationArgs
            {
                Message = "Could not add tax record",
                Type = NotificationService.NotificationType.Error
            });
        }
    }
}
