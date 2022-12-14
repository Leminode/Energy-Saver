using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Energy_Saver.Model;
using Energy_Saver.Services;
using Energy_Saver.DataSpace;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MailKit.Net.Imap;
using MailKit;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using static Energy_Saver.Pages.StatisticsModel;

namespace Energy_Saver.Pages
{
    [ExcludeFromCodeCoverage]
    public class InputModel : PageModel
    {
        private readonly ILogger<InputModel> _logger;
        private readonly EnergySaverTaxesContext _context;
        private readonly INotificationService _notificationService;
        private readonly IEmailDataExtractorService _emailDataExtractorService;

        public event EventHandler<NotificationService.NotificationArgs> InputTaxesHandler;

        [BindProperty]
        public Taxes Taxes { get; set; }

        [BindProperty]
        public int Year { get; set; }

        [BindProperty]
        public Months Month { get; set; }

        [BindProperty]
        public string UserEmailPassword { get; set; }

        public InputModel(ILogger<InputModel> logger, EnergySaverTaxesContext context, INotificationService notificationService, IEmailDataExtractorService emailDataExtractorService)
        {
            _logger = logger;
            _context = context;
            _notificationService = notificationService;
            InputTaxesHandler += _notificationService.CreateNotification;
            _emailDataExtractorService = emailDataExtractorService;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostData()
        {
            //if (!ModelState.IsValid)
            //{
            //    OnTaxInputError("An error has occured");
            //    return Page();
            //}

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

            return RedirectToPage("./Statistics", new { selectedMonth = Taxes.Month, selectedYear = Taxes.Year });
        }

        protected virtual void OnTaxInputSuccess()
        {
            InputTaxesHandler?.Invoke(this, new NotificationService.NotificationArgs
            {
                Message = $"Successfully added entry for {Taxes.Year}-{Utilities.FormatMonth(Taxes.Month)}",
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

        public async Task OnPostEmail()
        {
            string userEmail = User.FindFirst(ClaimTypes.Email).Value;
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserEmailAddress == userEmail);

            try
            {
                var taxes = await _emailDataExtractorService.Extract(user.UserEmailAddress, user.UserEmailPassword, Year, Month);

                Taxes.Year = taxes.Year;
                Taxes.Month = taxes.Month;
                Taxes.GasAmount = taxes.GasAmount;
                Taxes.WaterAmount = taxes.WaterAmount;
                Taxes.HeatingAmount = taxes.HeatingAmount;
                Taxes.ElectricityAmount = taxes.ElectricityAmount;
                
                if (taxes.ElectricityAmount > 0 || taxes.GasAmount > 0 || taxes.WaterAmount > 0 || taxes.HeatingAmount > 0)
                {
                    InputTaxesHandler?.Invoke(this, new NotificationService.NotificationArgs
                    {
                        Message = $"Succesfully extracted data from {userEmail}",
                        Type = NotificationService.NotificationType.Success
                    });
                }
                else
                {
                    InputTaxesHandler?.Invoke(this, new NotificationService.NotificationArgs
                    {
                        Message = "No data was extracted",
                        Type = NotificationService.NotificationType.Warning
                    });
                }

            }
            catch (Exception ex)
            {
                OnTaxInputError("Error: " + ex.Message);
            }
        }
    }
}
