namespace Energy_Saver.Services
{
    public interface IUserProfileService
    {
        public Task<bool> UpdateUserNameAsync(string userId, string newName);
        public Task<bool> UpdateUserEmailAsync(string userId, string newEmail);
        public Task<string> UpdateUserPasswordAsync(string userId);
    }
}
