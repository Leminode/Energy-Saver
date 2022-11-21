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
        private readonly EnergySaverTaxesContext _context;
        private readonly INotificationService _notificationService;

        public delegate void DetailsTaxesHandler(object source, NotificationService.NotificationArgs args);
        public event DetailsTaxesHandler DetailsTaxes;

        public DetailsModel(EnergySaverTaxesContext context, INotificationService notificationService)
        {
            _context = context;
            _notificationService = notificationService;
            DetailsTaxes += _notificationService.CreateNotification;
        }

        public Taxes? Taxes { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Taxes == null)
            {
                return NotFound();
            }

            var tempString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value.Split('|').Last();
            int userID = int.Parse(tempString);

            try
            {
                var taxes = await _context.Taxes.FirstOrDefaultAsync(m => m.ID == id && m.UserID == userID);

                if (taxes == null)
                {
                    return NotFound();
                }
                else
                {
                    Taxes = taxes;
                }

                return Page();
            }
            catch (Exception)
            {
                OnTaxGetError();
                return RedirectToPage("./Index");
            }
        }

        protected virtual void OnTaxGetError()
        {
            DetailsTaxes?.Invoke(this, new NotificationService.NotificationArgs
            {
                Message = "Could not retrieve tax list",
                Type = NotificationService.NotificationType.Error
            });
        }
    }
}
