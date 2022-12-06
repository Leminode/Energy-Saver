using Auth0.Core.Exceptions;
using Auth0.ManagementApi.Models;
using Auth0.ManagementApi;
using acme.Pages;
using Energy_Saver.DataSpace;
using System.Security.Cryptography.X509Certificates;
using Microsoft.EntityFrameworkCore;
using System.Security.Policy;

namespace Energy_Saver.Services
{
    public class UserProfileService : IUserProfileService
    {
        private readonly ILogger<UserProfileService> _logger;
        private readonly IConfiguration _config;

        public UserProfileService(ILogger<UserProfileService> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
        }

        public async Task<bool> UpdateUserNameAsync(string userId, string newName)
        {
            var client = new ManagementApiClient(_config["Auth0ApiToken"], new Uri($"https://priolette.eu.auth0.com/api/v2"));
            var request = new UserUpdateRequest
            {
                NickName = newName
            };
            try
            {
                await client.Users.UpdateAsync(userId, request);
                _logger.LogInformation($"Successfully updated user's \"{userId}\" nickname to {newName}");
                return true;
            }
            catch (ErrorApiException ex)
            {
                _logger.LogError(ex.Message);
                return false;
            }
        }

        public async Task<bool> UpdateUserEmailAsync(string userId, string newEmail)
        {
            var client = new ManagementApiClient(_config["Auth0ApiToken"], new Uri($"https://priolette.eu.auth0.com/api/v2"));

            var request = new UserUpdateRequest
            {
                Email = newEmail
            };

            try
            {
                await client.Users.UpdateAsync(userId, request);
                _logger.LogInformation($"Successfully updated user's \"{userId}\" email to {newEmail}");
                return true;
            }
            catch (ErrorApiException ex)
            {
                _logger.LogError(ex.Message);
                return false;
            }
        }

        public async Task<string> UpdateUserPasswordAsync(string userId)
        {
            var client = new ManagementApiClient(_config["Auth0ApiToken"], new Uri($"https://priolette.eu.auth0.com/api/v2"));

            var request = new PasswordChangeTicketRequest
            {
                ClientId = _config["Auth0:ClientId"],
                UserId = userId
            };

            _logger.LogError(_config["Auth0:ClientId"]);

            try
            {
                var ticket = await client.Tickets.CreatePasswordChangeTicketAsync(request);
                _logger.LogInformation($"Successfully sent password change ticket for user \"{userId}\"");
                return ticket.Value;
            }
            catch (ErrorApiException ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }
    }
}
