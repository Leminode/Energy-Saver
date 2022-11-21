using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Energy_Saver.Model;
using Energy_Saver.Services;
using Energy_Saver.DataSpace;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace Energy_Saver.Pages
{
    public class InputModel : PageModel
    {
        private readonly EnergySaverTaxesContext _context;
        private readonly INotificationService _notificationService;

        public event EventHandler<NotificationService.NotificationArgs> InputTaxesHandler;

        [BindProperty]
        public Taxes? Taxes { get; set; }

        public InputModel(EnergySaverTaxesContext context, INotificationService notificationService)
        {
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
                OnTaxInputError();
                return Page();
            }

            //will need a better implementation
            var tempString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value.Split('|').Last();
            Taxes.UserID = int.Parse(tempString);

            //var taxes = await _context.Taxes.FirstOrDefaultAsync(m => m.ID == id && m.UserID == userID);

            //if (taxes == null)
            //{
            //    return NotFound();
            //}

            _context.Taxes.Add(Taxes);
            try
            {
                await _context.SaveChangesAsync();
                OnTaxInputSuccess();
            } 
            catch(DbUpdateConcurrencyException)
            {
                OnTaxInputError();
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

        protected virtual void OnTaxInputError()
        {
            InputTaxesHandler?.Invoke(this, new NotificationService.NotificationArgs
            {
                Message = "Could not add tax record",
                Type = NotificationService.NotificationType.Error
            });
        }
    }
}
