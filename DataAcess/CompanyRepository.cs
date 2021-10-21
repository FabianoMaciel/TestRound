using DataAcess.Entities;
using System.Collections.Generic;
using System.Data.SqlClient;
using static DataAcess.DBUtils;

namespace DataAcess
{
    public class CompanyRepository
    {
        private string _connectionString;

        public CompanyRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IEnumerable<Company> GetCompanies(string companyName = null, string isin = null)
        {
            List<Company> companies = new List<Company>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("GetCompanies", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@Name", companyName);
                    command.Parameters.AddWithValue("@ISIN", isin);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        Company company;
                        while (reader.Read())
                        {
                            company = new Company();
                            company.ISIN = reader[0].ToString();
                            company.Name = reader[1].ToString();
                            company.Exchange = reader[2].ToString();
                            company.Ticker = reader[3].ToString();
                            company.WebSite = reader[4] != null ? reader[4].ToString() : string.Empty;

                            companies.Add(company);
                        }
                    }
                }
            }

            return companies;
        }

        public TransactionReturn UpdateCompany(Company company)
        {
            TransactionReturn result = TransactionReturn.Unexpected;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("UpdateCompany", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@ISIN", company.ISIN);
                    command.Parameters.AddWithValue("@Name", company.Name);
                    command.Parameters.AddWithValue("@Ticker", company.Ticker);
                    command.Parameters.AddWithValue("@Exchange", company.Exchange);
                    command.Parameters.AddWithValue("@WebSite", company.WebSite);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result = (TransactionReturn)reader[0];
                        }
                    }
                }
            }

            return result;
        }

        public TransactionReturn CreateCompany(Company company)
        {
            TransactionReturn result = TransactionReturn.Unexpected;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("CreateCompany", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@ISIN", company.ISIN);
                    command.Parameters.AddWithValue("@Name", company.Name);
                    command.Parameters.AddWithValue("@Ticker", company.Ticker);
                    command.Parameters.AddWithValue("@Exchange", company.Exchange);
                    command.Parameters.AddWithValue("@WebSite", company.WebSite);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result = (TransactionReturn)reader[0];
                        }
                    }
                }
            }

            return result;
        }
    }
}
