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

        public event EventHandler<NotificationService.NotificationArgs> InputTaxesHandler;

        [BindProperty]
        public Taxes? Taxes { get; set; }

        public InputModel(ILogger<InputModel> logger, EnergySaverTaxesContext context, INotificationService notificationService)
        {
            _logger = logger;
            _context = context;
            _notificationService = notificationService;
            InputTaxesHandler += _notificationService.CreateNotification;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                OnTaxInputError("An error has occured");
                return Page();
            }

            var tempString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value.Split('|').Last();
            int userID = int.Parse(tempString);
            Taxes.UserID = userID;

            try
            {
                var temp = await _context.Taxes.FirstOrDefaultAsync(t => t.UserID == userID && t.Year == Taxes.Year && t.Month == Taxes.Month);

                if (temp != null)
                {
                    OnTaxInputError("The selected year and month already exist in your tax list");
                    return RedirectToPage("./Index");
                }
            }
            catch (Exception)
            {
                OnTaxInputError("There has been an error when connecting to the database.");
                return RedirectToPage("./Index");
            }

            _context.Taxes.Add(Taxes);

            try
            {
                await _context.SaveChangesAsync();
                OnTaxInputSuccess();
            } 
            catch(DbUpdateException)
            {
                OnTaxInputError("Could not write enrty. Please try again.");
            }

            return RedirectToPage("./Index");
        }

        protected virtual void OnTaxInputSuccess()
        {
            InputTaxesHandler?.Invoke(this, new NotificationService.NotificationArgs
            {
                Message = $"Successfully added entry for {Taxes.Year}-{Serialization.FormatMonth(Taxes.Month)}",
                Type = NotificationService.NotificationType.Success
            });
        }

        protected virtual void OnTaxInputError(string errorMessage)
        {
            InputTaxesHandler?.Invoke(this, new NotificationService.NotificationArgs
            {
                Message = errorMessage,
                Type = NotificationService.NotificationType.Error
            });
        }
    }
}
