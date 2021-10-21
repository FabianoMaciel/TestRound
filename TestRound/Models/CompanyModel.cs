using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestRound.Models
{
    public class CompanyModel
    {
        public string Name { get; set; }
        public string Exchange { get; set; }
        public string Ticker { get; set; }
        public string ISIN { get; set; }
        public string WebSite { get; set; }
    }
}
