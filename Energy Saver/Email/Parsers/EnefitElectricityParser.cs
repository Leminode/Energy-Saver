using Energy_Saver.Model;
using MimeKit;

namespace Energy_Saver.Email.Parsers
{
    public class EnefitElectricityParser : EmailParser
    {
        public override void Parse(Taxes taxes)
        {
            string body = Email.GetTextBody(MimeKit.Text.TextFormat.Text);

            int startIndex = body.IndexOf("Mokėtina suma:");
            int endIndex   = body.IndexOf("Eur", startIndex);
            string pricePretty = body.Substring(startIndex, endIndex - startIndex);
            int priceIndex = pricePretty.LastIndexOf('*');

            string price = pricePretty.Substring(priceIndex + 1, 5)
                                      .Replace(',', '.');

            if (decimal.TryParse(price, out decimal parsedPrice))
            {
                taxes.ElectricityAmount = parsedPrice;
            }
        }

        public static bool CanParse(MimeMessage email)
        {
            return email.Subject.Contains("Enefit UAB");
        }
    }
}
