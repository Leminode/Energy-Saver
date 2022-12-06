using Energy_Saver.Model;
using MimeKit;

namespace Energy_Saver.Email
{
    public abstract class EmailParser
    {
        public MimeMessage Email { get; init; }

        public abstract void Parse(Taxes taxes);
    }
}
