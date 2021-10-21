using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using TestRound.Controllers;
using TestRound.Models;
using TestRound.Services;
using Xunit;
using static DataAcess.DBUtils;

namespace TestRoundTest
{
    public class CompanyControllerTest 
    {
        CompanyController _controller;
        CompanyService _service;

        public CompanyControllerTest()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            var mapper = config.CreateMapper();

            _service = new CompanyService(mapper);
            _controller = new CompanyController(mapper);
        }

        [Fact]
        public void Get_ReturnsOk()
        {
            var okResult = _controller.Get();

            Assert.NotNull(okResult);
        }

        [Fact]
        public void Create_ReturnOK()
        {
            CompanyModel model = CreateCompanyWithRandomISIN();

            string message = _controller.Post(model);
            string testMessage = GetMessage(string.Empty, TransactionReturn.Success, "created");

            Assert.Equal(testMessage, message);
        }

        [Fact]
        public void Create_ReturnISINDuplicated()
        {
            CompanyModel model = CreateCompanyWithRandomISIN();

            string message = _controller.Post(model);
            string testMessage = GetMessage(string.Empty, TransactionReturn.Success, "created");
            Assert.Equal(testMessage, message);

            message = _controller.Post(model);
            testMessage = GetMessage(string.Empty, TransactionReturn.ISINDuplicate, "created");
            Assert.Equal(testMessage, message);
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

        private static CompanyModel CreateCompanyWithRandomISIN()
        {
            Random generator = new Random();
            string randonNumber = generator.Next(0, 1000000).ToString();

            CompanyModel model = new CompanyModel();
            model.ISIN = string.Format("{0}{1}", "US", randonNumber); //Use a ISIN that has not been used yet 
            model.Name = "Apple Inc.";
            model.Exchange = "NASDAQ";
            model.Ticker = "AAPL";
            model.WebSite = "http://www.apple.com";
            return model;
        }
    }
}
