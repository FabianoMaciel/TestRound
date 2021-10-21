using System.Linq;

namespace DataAcess.Entities
{
    public class Company
    {
        public string Name { get; set; }
        public string Exchange { get; set; }
        public string Ticker { get; set; }
        public string ISIN { get; set; }
        public string WebSite { get; set; }

        public string ValidateCompanyValues()
        {
            string message = string.Empty;

            string prefix = ISIN.Substring(0, 2);
            if (!prefix.All(x => char.IsLetter(x)))
            {
                message = "The first two characters of the ISIN should be letters.";
            }

            return message;
        }
    }
}
