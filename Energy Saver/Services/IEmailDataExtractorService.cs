using Energy_Saver.Model;

namespace Energy_Saver.Services
{
    public interface IEmailDataExtractorService
    {
        Task<Taxes> Extract(string email, string password, int year, Months month);
    }
}
