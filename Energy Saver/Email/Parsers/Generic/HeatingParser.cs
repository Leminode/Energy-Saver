using Energy_Saver.Model;

namespace Energy_Saver.Email.Parsers.Generic
{
    public class HeatingParser : EmailParser
    {
        public override void Parse(Taxes taxes)
        {
            taxes.HeatingAmount = int.Parse(Email.GetTextBody(MimeKit.Text.TextFormat.Text));
        }
    }
}
