using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using TestRound.Models;
using TestRound.Services;
using TestRound.Services.Contracts;

namespace TestRound.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private ICompanyService _companyService;

        public CompanyController(IMapper mapper)
        {
            _companyService = new CompanyService(mapper);
        }

        [AllowAnonymous]
        [HttpGet]
        public IEnumerable<CompanyModel> Get()
        {
            return _companyService.GetCompanies();
        }

        [HttpGet]
        [Route("[action]/{isin}")]
        public IEnumerable<CompanyModel> GetByISIN(string isin)
        {
            return _companyService.GetCompanies(isin: isin.ToUpper());
        }


        [HttpGet]
        [Route("[action]/{name}")]
        public IEnumerable<CompanyModel> GetByName(string name)
        {
            return _companyService.GetCompanies(name: name.ToUpper());
        }

        [HttpPost]
        public string Post([FromBody]CompanyModel company)
        {
            return _companyService.CreateCompany(company);
        }

        [HttpPut()]
        public string Put([FromBody]CompanyModel company)
        {
            return _companyService.UpdateCompany(company);
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            //doesn't need to be implemented
        }
    }
}
