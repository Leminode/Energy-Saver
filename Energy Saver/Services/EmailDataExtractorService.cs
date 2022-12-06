using Energy_Saver.Model;
using MailKit.Net.Imap;
using MailKit;
using MailKit.Search;
using System.Text.RegularExpressions;
using System.Net.Mail;
using Energy_Saver.Email;

namespace Energy_Saver.Services
{
    public class EmailDataExtractorService : IEmailDataExtractorService
    {
        public async Task<Taxes> Extract(string emailAddress, string password, int year, Months month)
        {
            using (var client = new ImapClient())
            {
                var imapServer = "imap." + emailAddress.Split("@")[1];
                
                await client.ConnectAsync(imapServer, 993, true);

                await client.AuthenticateAsync(emailAddress, password);

                IMailFolder inbox = client.Inbox;
                await inbox.OpenAsync(FolderAccess.ReadOnly);

                var emails =
                    from email in inbox
                    where email.Date.Month.Equals((int)month) && email.Date.Year.Equals(year)
                    select email;

                Taxes taxes = new Taxes();

                taxes.Year = year;
                taxes.Month = month;

                foreach (var e in emails) 
                {
                    EmailParser? parser = EmailParserFactory.GetParser(e);
                    if (parser != null)
                        parser.Parse(taxes);
                }

                client.Disconnect(true);

                return taxes;
            }
        }
    }
}
