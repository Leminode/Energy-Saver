using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Energy_Saver.DataSpace;
using Energy_Saver.Model;
using System.Security.Claims;
using Energy_Saver.Services;

namespace Energy_Saver.Pages
{
    public class EditModel : PageModel
    {
        private readonly EnergySaverTaxesContext _context;
        private readonly INotificationService _notificationService;

        public event EventHandler<NotificationService.NotificationArgs> EditTaxesHandler;

        public EditModel(EnergySaverTaxesContext context, INotificationService notificationService)
        {
            _context = context;
            _notificationService = notificationService;
            EditTaxesHandler += _notificationService.CreateNotification;
        }

        [BindProperty]
        public Taxes Taxes { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Taxes == null)
            {
                return RedirectToPage("./Index");
            }

            var tempString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value.Split('|').Last();
            int userID = int.Parse(tempString);

            var taxes =  await _context.Taxes.FirstOrDefaultAsync(m => m.ID == id && m.UserID == userID);

            if (taxes == null)
            {
                return RedirectToPage("./Index");
            }

            Taxes = taxes;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Taxes).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                OnTaxEditSuccess();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TaxesExists(Taxes.ID))
                {
                    OnTaxEditError();
                    return RedirectToPage("./Index");
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool TaxesExists(int id)
        {
            return _context.Taxes.Any(e => e.ID == id);
        }

        protected virtual void OnTaxEditSuccess()
        {
            EditTaxesHandler?.Invoke(this, new NotificationService.NotificationArgs 
            { 
                Message = $"Successfully edited entry for {Taxes.Year}-{Utilities.FormatMonth(Taxes.Month)}",
                Type = NotificationService.NotificationType.Success 
            });
        }

        protected virtual void OnTaxEditError()
        {
            EditTaxesHandler?.Invoke(this, new NotificationService.NotificationArgs 
            { 
                Message = "Could not edit tax record", 
                Type = NotificationService.NotificationType.Error 
            });
        }
    }
}
