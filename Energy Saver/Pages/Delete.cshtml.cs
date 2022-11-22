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
    public class DeleteModel : PageModel
    {
        private readonly EnergySaverTaxesContext _context;
        private readonly INotificationService _notificationService;

        public event EventHandler<NotificationService.NotificationArgs> DeleteTaxesHandler;

        public DeleteModel(EnergySaverTaxesContext context, INotificationService notificationService)
        {
            _context = context;
            _notificationService = notificationService;
            DeleteTaxesHandler += _notificationService.CreateNotification;
        }

        [BindProperty]
        public Taxes Taxes { get; set; }

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

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.Taxes == null)
            {
                OnTaxDeleteError();
                return RedirectToPage("./Index");
            }

            var taxes = await _context.Taxes.FindAsync(id);

            if (taxes != null)
            {
                Taxes = taxes;
                _context.Taxes.Remove(Taxes);
                try
                {
                    await _context.SaveChangesAsync();
                    OnTaxDeleteSuccess();
                }
                catch (DbUpdateConcurrencyException)
                {
                    OnTaxDeleteError();
                }
            }

            return RedirectToPage("./Index");
        }

        protected virtual void OnTaxDeleteSuccess()
        {
            DeleteTaxesHandler?.Invoke(this, new NotificationService.NotificationArgs 
            { 
                Message = $"Successfully deleted entry for {Taxes.Year}-{Utilities.FormatMonth(Taxes.Month)}", 
                Type = NotificationService.NotificationType.Success 
            });
        }

        protected virtual void OnTaxDeleteError()
        {
            DeleteTaxesHandler?.Invoke(this, new NotificationService.NotificationArgs 
            { 
                Message = "Could not delete tax record", 
                Type = NotificationService.NotificationType.Error 
            });
        }

        protected virtual void OnTaxGetError()
        {
            DeleteTaxesHandler?.Invoke(this, new NotificationService.NotificationArgs
            { 
                Message = "Could not retrieve tax list", 
                Type = NotificationService.NotificationType.Error 
            });
        }
    }
}
