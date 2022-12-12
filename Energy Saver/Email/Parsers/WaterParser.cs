using Energy_Saver.Model;

namespace Energy_Saver.Email.Parsers
{
    public class WaterParser : EmailParser
    {
        public override void Parse(Taxes taxes)
        {
            taxes.WaterAmount = int.Parse(Email.GetTextBody(MimeKit.Text.TextFormat.Text));
        }
    }
}
