using FlavorFi.Common.Models.ShopifyModels;
using FlavorFi.Common.RequestAndResponses.ShopWorksRequestAndResponses;
using FlavorFi.Services.DatabaseServices;
using System;
using System.Data;
using System.Data.Odbc;

namespace FlavorFi.Services.ShopWorksServices
{
    public class ShopWorksCustomerService : ShopWorksBaseService
    {
        public ShopWorksCustomerService(Guid companyId) : base(companyId) { }

        public GetShopWorksCustomersResponse GetShopWorksCustomers(GetShopWorksCustomersRequest request)
        {
            try
            {
                var response = new GetShopWorksCustomersResponse();
                var cmdText = CreateCustomerCmdText();
                using (var cmd = new OdbcCommand(cmdText, this.FileMakerCoreConnection))
                {
                    cmd.Connection.Open();
                    using (var reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                        while (reader.Read())
                        {
                            //LogReader(reader);
                            response.Customers.Add(CreateCustomerModel(this, reader));
                        }
                }

                response.IsSuccess = true;
                return response;
            }
            catch (Exception ex)
            {
                LoggingService.LogError(new Common.RequestAndResponses.DatabaseRequestAndResponses.LogErrorRequest { ex = ex });
                return new GetShopWorksCustomersResponse { ErrorMessage = ex.Message };
            }
        }

        private ShopifyCreateCustomerModel CreateCustomerModel(ShopWorksBaseService baseService, OdbcDataReader reader)
        {
            try
            {
                var shopifyCreateCustomerModel = new ShopifyCreateCustomerModel { HasVerifiedEmail = false, SendEmailInvite = true };
                if (reader["Email"] != DBNull.Value) shopifyCreateCustomerModel.Email = Convert.ToString(reader["Email"]);
                if (reader["NameFirst"] != DBNull.Value) shopifyCreateCustomerModel.FirstName = Convert.ToString(reader["NameFirst"]);
                if (reader["NameLast"] != DBNull.Value) shopifyCreateCustomerModel.LastName = Convert.ToString(reader["NameLast"]);

                this.CreateMetafield(shopifyCreateCustomerModel, "original_id", ConvertToString(reader["ID_Contact"]));
                this.CreateMetafield(shopifyCreateCustomerModel, "customer_id", ConvertToString(reader["ID_Customer"]));
                this.CreateMetafield(shopifyCreateCustomerModel, "company_name", ConvertToString(reader["CompanyName"]));
                return shopifyCreateCustomerModel;
            }
            catch (Exception ex) { throw ex; }
        }

        private static string CreateCustomerCmdText()
        {
            return "SELECT cu.ID_Customer, cu.CompanyName, co.ID_Contact, co.NameFirst, co.NameLast, cn.Email " +
                   "FROM Cont co " +
                   "INNER JOIN Cust cu ON cu.ID_Customer = co.id_Customer " +
                   "INNER JOIN ContNum cn ON cn.id_Contact = co.ID_Contact AND cn.cn_sts_Email = 1";

            //return "SELECT * FROM ContNum cn WHERE cn.id_Contact = 13013";
            //       "INNER JOIN Cust cu ON cu.ID_Customer = co.id_Customer " + 
            //       "INNER JOIN ContNum cn ON cn.id_Contact = co.ID_Contact ";
        }

        private static void LogReader(OdbcDataReader reader)
        {
            try
            {
                var msg = "";
                for (int i = 0; i < reader.FieldCount; i++)
                    msg += "[" + reader.GetName(i) + ": " + Convert.ToString(reader[i]) + "]" + Environment.NewLine;
                Console.WriteLine(msg);
                Console.WriteLine("----------------------------------------------------------------------------------");
            }
            catch (Exception) { }
        }

        private void CreateMetafield(ShopifyCreateCustomerModel customer, string key, string value)
        {
            try
            {
                if (!string.IsNullOrEmpty(value))
                {
                    var metafield = new ShopifyCreateMetafieldModel { Namespace = "global", Key = key, Value = value, ValueType = "string" };
                    customer.Metafields.Add(metafield);
                }
            }
            catch (Exception) { }
        }

        private static string ConvertToString(object value)
        {
            try
            {
                if (value == DBNull.Value) return "";
                return Convert.ToString(value).Trim();
            }
            catch (Exception) { return ""; }
        }
    }
}