using Energy_Saver.Model;
using MimeKit;

namespace Energy_Saver.Email.Parsers
{
    public class IgnitisElectricityParser : EmailParser
    {
        public override void Parse(Taxes taxes)
        {
            string body = Email.GetTextBody(MimeKit.Text.TextFormat.Text);

            string searchPhrase = "Mokėtina suma:*";
            int startIndex = body.IndexOf(searchPhrase) + searchPhrase.Length;
            int endIndex = body.IndexOf("Eur", startIndex);
            string price = body.Substring(startIndex, endIndex - startIndex)
                               .Replace(',', '.');

            if (decimal.TryParse(price, out decimal parsedPrice))
            {
                Console.WriteLine("[IgnitisElectricityParser] Price parsed: " + parsedPrice);
                taxes.ElectricityAmount = parsedPrice;
            }
        }

        public static bool CanParse(MimeMessage email)
        {
            return email.GetTextBody(MimeKit.Text.TextFormat.Text).Contains("ignitis");
        }
    }
}
