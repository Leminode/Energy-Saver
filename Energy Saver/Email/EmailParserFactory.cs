using Energy_Saver.Email.Parsers;
using Energy_Saver.Email.Parsers.Generic;
using MimeKit;

namespace Energy_Saver.Email
{
    public class EmailParserFactory
    {
        public delegate EmailParser ParserCreator(MimeMessage email);
        public delegate bool ParserCompatibilityChecker(MimeMessage email);

        private static readonly (ParserCompatibilityChecker check, ParserCreator create)[] ParsersInfo = 
        { 
            (EnefitElectricityParser.CanParse, email => new EnefitElectricityParser() { Email = email }),
            (IgnitisElectricityParser.CanParse, email => new IgnitisElectricityParser() { Email = email }),
            (LiteskoHeatingParser.CanParse, email => new LiteskoHeatingParser() { Email = email }),
            (VilniausSilumosTinkaiHeatingParser.CanParse, email => new VilniausSilumosTinkaiHeatingParser() { Email = email }),
            
            // Generic parsers
            (email => email.Subject.Contains("Gas"), email => new GasParser() { Email = email }),
            (email => email.Subject.Contains("Water"), email => new WaterParser() { Email = email }),
            (email => email.Subject.Contains("Electricity"), email => new ElectricityParser() { Email = email }),
            (email => email.Subject.Contains("Heating"), email => new HeatingParser() { Email = email }),
        };

        public static EmailParser? GetParser(MimeMessage email)
        {
            foreach ((var check, var create) in ParsersInfo)
            {
                if (check(email))
                    return create(email);
            }
                        
            return null;
        }
    }
}
