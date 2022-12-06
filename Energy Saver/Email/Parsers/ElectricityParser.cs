using Energy_Saver.Model;

namespace Energy_Saver.Email.Parsers
{
    public class ElectricityParser : EmailParser
    {
        public override void Parse(Taxes taxes)
        {
            taxes.ElectricityAmount = int.Parse(Email.GetTextBody(MimeKit.Text.TextFormat.Text));
        }
    }
}
