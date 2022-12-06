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

namespace acme.Pages
{
    [ExcludeFromCodeCoverage]
    public class ProfileModel : PageModel
    {
        private readonly ILogger<ProfileModel> _logger;
        private readonly IConfiguration _config;
        private readonly INotificationService _notificationService;

        public event EventHandler<NotificationService.NotificationArgs> ProfileHandler;

        [StringLength(30, MinimumLength = 3)]
        [BindProperty]
        public string? UserName { get; set; }
        [BindProperty]
        public string? UserEmailAddress { get; set; }
        public string? UserProfileImage { get; set; }

        public ProfileModel(ILogger<ProfileModel> logger, IConfiguration config, INotificationService notificationService)
        {
            _logger = logger;
            _config = config;
            _notificationService = notificationService;
            ProfileHandler += _notificationService.CreateNotification;
        }

        public void OnGet()
        {
            UserName = User.FindFirst(c => c.Type == "nickname")?.Value;
            UserEmailAddress = User.FindFirst(c => c.Type == ClaimTypes.Email)?.Value;
            UserProfileImage = User.FindFirst(c => c.Type == "picture")?.Value;
        }

        public IActionResult OnPost()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var client = new RestClient("https://priolette.eu.auth0.com/api/v2/");
            var request = new RestRequest($"/users/{userId}", RestSharp.Method.Patch);

            request.AddHeader("content-type", "application/json");
            request.AddHeader("authorization", $"Bearer {_config["Auth0ApiToken"]}");
            request.AddHeader("cache-control", "no-cache");
            request.AddParameter("application/json", $"{{\"nickname\": \"{UserName}\" }}", ParameterType.RequestBody);

            RestResponse response = client.Execute(request);
            _logger.LogInformation(response.Content);

            OnProfileEditSuccess();

            return RedirectToPage("./Profile");
        }

        protected virtual void OnProfileEditSuccess()
        {
            ProfileHandler?.Invoke(this, new NotificationService.NotificationArgs
            {
                Message = $"Successfully changed name to {UserName}",
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
