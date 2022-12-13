using Energy_Saver.Model;

namespace Energy_Saver.Email.Parsers.Generic
{
    public class GasParser : EmailParser
    {
        public override void Parse(Taxes taxes)
        {
            taxes.GasAmount = int.Parse(Email.GetTextBody(MimeKit.Text.TextFormat.Text));
        }
    }
}
