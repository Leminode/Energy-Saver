using Auth0.Core.Exceptions;
using Auth0.ManagementApi;
using Auth0.ManagementApi.Models;
using Energy_Saver.DataSpace;
using Energy_Saver.Model;
using Energy_Saver.Pages;
using Energy_Saver.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.DotNet.Scaffolding.Shared.CodeModifier.CodeChange;
using Microsoft.EntityFrameworkCore;
using RestSharp;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;

namespace acme.Pages
{
    [ExcludeFromCodeCoverage]
    public class ProfileModel : PageModel
    {
        private readonly ILogger<ProfileModel> _logger;
        private readonly INotificationService _notificationService;
        private readonly IUserProfileService _profileService;
        private readonly EnergySaverTaxesContext _context;

        public event EventHandler<NotificationService.NotificationArgs> ProfileHandler;

        [BindProperty]
        public Users? DbUser { get; set; }

        public ProfileModel(ILogger<ProfileModel> logger, INotificationService notificationService, IUserProfileService profileService, EnergySaverTaxesContext context)
        {
            _logger = logger;
            _notificationService = notificationService;
            _profileService = profileService;
            ProfileHandler += _notificationService.CreateNotification;
            _context = context;
        }

        public void OnGet()
        {
            var tempString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value.Split('|').Last();
            int userID = int.Parse(tempString);

            try
            {
                var user = _context.Users.FirstOrDefault(u => u.UserId == userID);
                DbUser = user;

                DbUser.UserName = User.FindFirst(c => c.Type == "nickname")?.Value;
                DbUser.UserEmailAddress = User.FindFirst(c => c.Type == ClaimTypes.Email)?.Value;
                DbUser.UserProfileImage = User.FindFirst(c => c.Type == "picture")?.Value;
            }
            catch (Exception)
            {
                OnProfileEditError("There has been an error when conneting to the database");
            }
        }

        public async Task<IActionResult> OnPostDetails()
        {  
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var hasUpdatedName = await _profileService.UpdateUserNameAsync(userId, DbUser.UserName);
            var hasUpdatedEmail = await _profileService.UpdateUserEmailAsync(userId, DbUser.UserEmailAddress);

            if (hasUpdatedEmail && hasUpdatedName)
                OnProfileEditSuccess();
            else
                OnProfileEditError("Could not update user profile");

            return RedirectToPage("./Profile");
        }

        public async Task<IActionResult> OnPostAccountPassword()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var ticketUrl = await _profileService.UpdateUserPasswordAsync(userId);

            if (ticketUrl == null)
            {
                OnProfileEditError("Something went wrong");
                return RedirectToPage("./Profile");
            }

            return Redirect(ticketUrl);
        }

        public async Task<IActionResult> OnPostEmailPassword()
        {
            var tempString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value.Split('|').Last();
            int userID = int.Parse(tempString);

            DbUser.UserId = userID;
            DbUser.UserEmailAddress = User.FindFirst(c => c.Type == ClaimTypes.Email)?.Value;

            _context.Attach(DbUser).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                OnProfileEditSuccess();
            }
            catch (DbUpdateException)
            {
                OnProfileEditError("Something went wrong");
            }

            return RedirectToPage("./Profile");
        }

        protected virtual void OnProfileEditSuccess()
        {
            ProfileHandler?.Invoke(this, new NotificationService.NotificationArgs
            {
                Message = "Profile updated successfully",
                Type = NotificationService.NotificationType.Success
            });
        }

        protected virtual void OnProfileEditError(string errorMessage)
        {
            ProfileHandler?.Invoke(this, new NotificationService.NotificationArgs
            {
                Message = errorMessage,
                Type = NotificationService.NotificationType.Error
            });
        }
    }
}
