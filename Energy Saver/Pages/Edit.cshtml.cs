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
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;
using Microsoft.AspNetCore.Mvc.Filters;

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

                Taxes = taxes;

                return Page();
            }
            catch (Exception)
            {
                OnTaxEditError("There has been an error when conneting to the database");
                return RedirectToPage("./Index");
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                var temp = await _context.Taxes.FirstOrDefaultAsync(m => m.ID != Taxes.ID && m.UserID == Taxes.UserID
                && m.Year == Taxes.Year && m.Month == Taxes.Month);

                if (temp != null)
                {
                    OnTaxEditError("The selected year and month already exist in your tax list");
                    return RedirectToPage("./Index");
                }
            }
            catch (Exception)
            {
                OnTaxEditError("There has been an error when conneting to the database");
                return RedirectToPage("./Index");
            }

            _context.Attach(Taxes).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                OnTaxEditSuccess();
            }
            catch (DbUpdateException)
            {
                OnTaxEditError("Could not edit tax record");
            }

            return RedirectToPage("./Index");
        }

        protected virtual void OnTaxEditSuccess()
        {
            EditTaxesHandler?.Invoke(this, new NotificationService.NotificationArgs 
            { 
                Message = $"Successfully edited entry for {Taxes.Year}-{Serialization.FormatMonth(Taxes.Month)}",
                Type = NotificationService.NotificationType.Success 
            });
        }

        protected virtual void OnTaxEditError(string errorMessage)
        {
            EditTaxesHandler?.Invoke(this, new NotificationService.NotificationArgs 
            { 
                Message = errorMessage, 
                Type = NotificationService.NotificationType.Error 
            });
        }
    }
}
