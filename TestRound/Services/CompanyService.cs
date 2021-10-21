using AutoMapper;
using DataAcess;
using DataAcess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestRound.Models;
using TestRound.Services.Contracts;
using static DataAcess.DBUtils;

namespace TestRound.Services
{
    public class CompanyService : ICompanyService
    {
        private CompanyRepository _repository;
        private IMapper _mapper;

        public CompanyService(IMapper mapper)
        {
            //TO DO get from the .config
            _repository = new CompanyRepository(@"Server=(localdb)\MSSQLLocalDB; Database=CompanyGL");
            _mapper = mapper;
        }

        public IEnumerable<CompanyModel> GetCompanies(string isin = null, string name = null)
        {
            IEnumerable<CompanyModel> companies = _mapper.Map<IEnumerable<CompanyModel>>(_repository.GetCompanies(name, isin));

            return companies;
        }

        public string CreateCompany(CompanyModel model)
        {
            Company company = _mapper.Map<CompanyModel, Company>(model);

            string message = company.ValidateCompanyValues();

            if (string.IsNullOrWhiteSpace(message))
            {
                TransactionReturn result = _repository.CreateCompany(company);

                message = GetMessage(message, result, "created");
            }

            return message;
        }

        public string UpdateCompany(CompanyModel model)
        {
            Company company = _mapper.Map<CompanyModel, Company>(model);

            string message = string.Empty;

            if (string.IsNullOrWhiteSpace(message))
            {
                TransactionReturn result = _repository.UpdateCompany(company);

                message = GetMessage(message, result, "updated");
            }

            return message;
        }

        private static string GetMessage(string message, TransactionReturn result, string action)
        {
            switch (result)
            {
                case TransactionReturn.Success:
                    message = string.Format("Company has been {0} with success.", action);
                    break;
                case TransactionReturn.ISINDuplicate:
                    message = "This ISIN has already been used by other company.";
                    break;
                case TransactionReturn.ISINNotExist:
                    message = "Company doesn't exist.";
                    break;
                case TransactionReturn.Unexpected:
                    message = "An unexpected error has happen, please contact the administator of the system.";
                    break;
                default:
                    break;
            }

            return message;
        }
    }
}
