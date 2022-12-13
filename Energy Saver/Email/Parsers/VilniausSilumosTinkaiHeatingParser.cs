using Energy_Saver.Model;
using MimeKit;

namespace Energy_Saver.Email.Parsers
{
    public class VilniausSilumosTinkaiHeatingParser : EmailParser
    {
        public override void Parse(Taxes taxes)
        {
            string body = Email.GetTextBody(MimeKit.Text.TextFormat.Text);

            string searchPhrase = "MOKĖTINA SUMA";
            int startIndex = body.IndexOf(searchPhrase) + searchPhrase.Length;
            int endIndex = body.IndexOf("[", startIndex);
            string price = body.Substring(startIndex, endIndex - startIndex)
                               .Replace(',', '.');

            if (decimal.TryParse(price, out decimal parsedPrice))
            {
                Console.WriteLine("[VilniausSilumosTinkaiHeatingParser] Price parsed: " + parsedPrice);
                taxes.HeatingAmount = parsedPrice;
            }
        }

        public static bool CanParse(MimeMessage email)
        {
            return email.GetTextBody(MimeKit.Text.TextFormat.Text)
                .Contains("AB Vilniaus šilumos tinklai");
        }
    }
}
