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

namespace acme.Pages
{
    public class ProfileModel : PageModel
    {
        private readonly ILogger<ProfileModel> _logger;
        private readonly INotificationService _notificationService;
        private readonly IUserProfileService _profileService;

        public event EventHandler<NotificationService.NotificationArgs> ProfileHandler;

        [StringLength(30, MinimumLength = 3)]
        [BindProperty]
        public string? UserName { get; set; }

        [RegularExpression(@"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*"
        + "@"
        + @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))$", ErrorMessage = "Invalid email address")]
        [BindProperty]
        public string? UserEmailAddress { get; set; }

        public string? UserProfileImage { get; set; }

        public ProfileModel(ILogger<ProfileModel> logger, INotificationService notificationService, IUserProfileService profileService)
        {
            _logger = logger;
            _notificationService = notificationService;
            _profileService = profileService;
            ProfileHandler += _notificationService.CreateNotification;
        }

        public void OnGet()
        {
            UserName = User.FindFirst(c => c.Type == "nickname")?.Value;
            UserEmailAddress = User.FindFirst(c => c.Type == ClaimTypes.Email)?.Value;
            UserProfileImage = User.FindFirst(c => c.Type == "picture")?.Value;
        }

        public async Task<IActionResult> OnPostDetails()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var hasUpdatedName = await _profileService.UpdateUserNameAsync(userId, UserName);
            var hasUpdatedEmail = await _profileService.UpdateUserEmailAsync(userId, UserEmailAddress);

            if (hasUpdatedEmail && hasUpdatedName)
                OnProfileEditSuccess();
            else
                OnProfileEditError("Could not update user profile");

            return RedirectToPage("./Profile");

            //OLD IMPLEMENTATION (DO NOT REMOVE YET):

            //var client = new RestClient("https://priolette.eu.auth0.com/api/v2/");
            //var request = new RestRequest($"/users/{userId}", RestSharp.Method.Patch);

            //request.AddHeader("content-type", "application/json");
            //request.AddHeader("authorization", $"Bearer {_config["Auth0ApiToken"]}");
            //request.AddHeader("cache-control", "no-cache");
            //request.AddParameter("application/json", $"{{\"nickname\": \"{UserName}\" }}", ParameterType.RequestBody);

            //RestResponse response = client.Execute(request);
            //_logger.LogInformation(response.Content);

            //OnProfileEditSuccess();

            //return RedirectToPage("./Profile");
        }

        public async Task<IActionResult> OnPostPassword()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var ticketUrl = await _profileService.UpdateUserPasswordAsync(userId);

            if (ticketUrl.Equals(null))
            {
                OnProfileEditError("Something went wrong");
                return RedirectToPage("./Profile");
            }

            return Redirect(ticketUrl);
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
