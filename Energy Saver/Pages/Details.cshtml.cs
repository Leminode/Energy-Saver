using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Energy_Saver.DataSpace;
using Energy_Saver.Model;
using System.Security.Claims;
using Energy_Saver.Services;

namespace Energy_Saver.Pages
{
    public class DetailsModel : PageModel
    {
        private EventHandler<NotificationService.NotificationArgs> TaxesDetailsHandler;

        private readonly EnergySaverTaxesContext _context;
        private readonly INotificationService _notificationService;

        public DetailsModel(EnergySaverTaxesContext context, INotificationService notificationService)
        {
            _context = context;
            _notificationService = notificationService;
            TaxesDetailsHandler += _notificationService.CreateNotification;
        }

        public Taxes? Taxes { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Taxes == null)
            {
                OnTaxesNotFound();
                return RedirectToPage("./Index");
            }

            var tempString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value.Split('|').Last();
            int userID = int.Parse(tempString);

            try
            {
                var taxes = await _context.Taxes.FirstOrDefaultAsync(m => m.ID == id && m.UserID == userID);

                if (taxes == null)
                {
                    OnTaxesNotFound();
                    return RedirectToPage("./Index");
                }
                else
                {
                    Taxes = taxes;
                }

                return Page();
            }
            catch (Exception)
            {
                OnTaxesNotFound();
                return RedirectToPage("./Index");
            }
        }

        protected virtual void OnTaxesNotFound()
        {
            TaxesDetailsHandler?.Invoke(this, new NotificationService.NotificationArgs
            {
                Message = "Could not find specified tax record",
                Type = NotificationService.NotificationType.Error
            });
        }
    }
}
