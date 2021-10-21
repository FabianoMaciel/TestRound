using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestRound.Models;

namespace TestRound.Services.Contracts
{
    interface ICompanyService
    {
        IEnumerable<CompanyModel> GetCompanies(string isin = null, string name = null);
        string CreateCompany(CompanyModel model);
        string UpdateCompany(CompanyModel model);
    }
}
