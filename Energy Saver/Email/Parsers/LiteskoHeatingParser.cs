using Energy_Saver.Model;
using MimeKit;

namespace Energy_Saver.Email.Parsers
{
    public class LiteskoHeatingParser : EmailParser
    {
        public override void Parse(Taxes taxes)
        {
            string body = Email.GetTextBody(MimeKit.Text.TextFormat.Text);

            string searchPhrase = "Sąskaitos suma";
            int startIndex = body.IndexOf(searchPhrase) + searchPhrase.Length;
            
            string price = body.Substring(startIndex, 5)
                               .Replace(',', '.');

            if (decimal.TryParse(price, out decimal parsedPrice))
            {
                taxes.HeatingAmount = parsedPrice;
            }
        }

        public static bool CanParse(MimeMessage email)
        {
            return email.GetTextBody(MimeKit.Text.TextFormat.Text).Contains("Litesko");
        }
    }
}
