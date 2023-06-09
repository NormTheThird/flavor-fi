using FlavorFi.Common.Enums;
using FlavorFi.Common.RequestAndResponses.DatabaseRequestAndResponses;
using FlavorFi.Common.RequestAndResponses.ShopWorksRequestAndResponses;
using FlavorFi.Services.DatabaseServices;
using System;
using System.Linq;

namespace FlavorFi.Services.ShopWorksServices
{
    public static class ShopWorksOrderService
    {
        public static CreateShopWorksOrderFileResponse CreateShopworksOrderFile(CreateShopWorksOrderFileRequest request)
        {
            try
            {
                var now = Common.Helpers.DateTimeConvert.GetTimeZoneDateTime(TimeZoneInfoId.CentralStandardTime);
                var response = new CreateShopWorksOrderFileResponse();
                var body = Environment.NewLine + "\"" + Environment.NewLine;

                // ORDER:
                body += CreateHeader("Start Order");

                // ORDER: ID SubBlock
                body += CreateLine("ExtOrderID", request.Order.Id.ToString());
                body += CreateLine("ExtSource", "Shopify Website");
                body += CreateLine("date_External", request.Order.CreatedAt.ToString("MM/dd/yyyy"));
                body += CreateLine("id_OrderType", "69"); // Switched from 95 = Equipment to 69 = online per curt.    

                // ORDER: Details SubBlock
                body += CreateLine("CustomerPurchaseOrder", ""); //discount code needs to go into customer po field
                //body += CreateLine("TermsName", "");
                //body += CreateLine("CustomerServiceRep", "");
                //body += CreateLine("CustomerType", "");
                //body += CreateLine("id_CompanyLocation", "");
                //body += CreateLine("id_SalesStatus", "");
                //body += CreateLine("sts_CommishAllow", "");
                //body += CreateLine("HoldOrderText", "");

                // ORDER: Dates SubBlock
                body += CreateLine("date_OrderPlaced", request.Order.CreatedAt.ToString("MM/dd/yyyy"));
                //body += CreateLine("date_OrderRequestedToShip", "");
                //body += CreateLine("date_OrderDropDead", "");

                // ORDER: Sales Tax SubBlock
                //body += CreateLine("sts_Order_SalesTax_Override", "");
                //body += CreateLine("sts_ApplySalesTax01", "");
                //body += CreateLine("sts_ApplySalesTax02", "");
                //body += CreateLine("sts_ApplySalesTax03", "");
                //body += CreateLine("sts_ApplySalesTax04", "");
                //body += CreateLine("coa_AccountSalesTax01", "");
                //body += CreateLine("coa_AccountSalesTax02", "");
                //body += CreateLine("coa_AccountSalesTax03", "");
                //body += CreateLine("coa_AccountSalesTax04", "");
                //body += CreateLine("sts_ShippingTaxable", "");

                // ORDER: Shipping Address SubBlock
                body += CreateLine("AddressDescription", "Shipping Address From Shopify");
                body += CreateLine("AddressCompany", request.Order.ShippingAddress.Company);
                body += CreateLine("Address1", request.Order.ShippingAddress.Address1);
                body += CreateLine("Address2", request.Order.ShippingAddress.Address2);
                body += CreateLine("AddressCity", request.Order.ShippingAddress.City);
                body += CreateLine("AddressState", request.Order.ShippingAddress.Province);
                body += CreateLine("AddressZip", request.Order.ShippingAddress.ZipCode);
                body += CreateLine("AddressCountry", request.Order.ShippingAddress.Country);
                if (request.Order.ShippingLines.Count > 0)
                {
                    var shipping = request.Order.ShippingLines[0];
                    body += CreateLine("ShipMethod", shipping.Source);
                    body += CreateLine("cur_Shipping", shipping.Price.ToString());
                    body += CreateLine("sts_Order_ShipAddress_Add", "1");
                }

                body += CreateHeader("End Order");

                // CUSTOMER:
                body += CreateHeader("Start Customer");

                // CUSTOMER: Details SubBlock
                body += CreateLine("ExtCustomerID", request.Order.Customer.Id.ToString());
                var metafield = request.CustomerMetafields.FirstOrDefault(m => m.Key.Equals("original_id", StringComparison.CurrentCultureIgnoreCase));
                if (metafield == null) body += CreateLine("id_Customer", "4728");
                else body += CreateLine("id_Customer", metafield.Value);
                body += CreateLine("Company", request.Order.Customer.DefaultAddress.Company);

                // CUSTOMER: Billing Address SubBlock
                body += CreateLine("AddressDescription", "Billing Address From Shopify");
                body += CreateLine("AddressCompany", request.Order.BillingAddress.Company);
                body += CreateLine("Address1", request.Order.BillingAddress.Address1);
                body += CreateLine("Address2", request.Order.BillingAddress.Address2);
                body += CreateLine("AddressCity", request.Order.BillingAddress.City);
                body += CreateLine("AddressState", request.Order.BillingAddress.Province);
                body += CreateLine("AddressZip", request.Order.BillingAddress.ZipCode);
                body += CreateLine("AddressCountry", request.Order.BillingAddress.Country);

                body += CreateHeader("End Customer");

                // CONTACT:
                body += CreateHeader("Start Contact ");
                body += CreateLine("NameFirst", request.Order.Customer.FirstName);
                body += CreateLine("NameLast", request.Order.Customer.LastName);
                body += CreateLine("Email", request.Order.Customer.Email);
                body += CreateLine("sts_Contact_Add", "1");
                body += CreateLine("sts_EnableBulkEmail", "1");
                body += CreateHeader("End Contact ");

                // PRODUCT:
                foreach (var lineItem in request.Order.LineItems)
                {
                    var skuSplit = lineItem.Sku.Split('-');
                    var color = skuSplit.Length > 2 ? skuSplit[1] : "";
                    var size = skuSplit.Length > 2 ? skuSplit[2] : skuSplit[1];

                    body += CreateHeader("Start Product");
                    body += CreateLine("PartNumber", skuSplit[0]);
                    body += CreateLine("PartColor", color);
                    body += CreateLine("cur_UnitPriceUserEntered", lineItem.Price.ToString());

                    if (size.Equals("s"))
                    {
                        body += CreateLine("Size01_Req", lineItem.Quantity.ToString());
                        body += CreateLine("Size02_Req", "");
                        body += CreateLine("Size03_Req", "");
                        body += CreateLine("Size04_Req", "");
                        body += CreateLine("Size05_Req", "");
                        body += CreateLine("Size06_Req", "");
                    }
                    else if (size.Equals("m"))
                    {
                        body += CreateLine("Size01_Req", "");
                        body += CreateLine("Size02_Req", lineItem.Quantity.ToString());
                        body += CreateLine("Size03_Req", "");
                        body += CreateLine("Size04_Req", "");
                        body += CreateLine("Size05_Req", "");
                        body += CreateLine("Size06_Req", "");
                    }
                    else if (size.Equals("l"))
                    {
                        body += CreateLine("Size01_Req", "");
                        body += CreateLine("Size02_Req", "");
                        body += CreateLine("Size03_Req", lineItem.Quantity.ToString());
                        body += CreateLine("Size04_Req", "");
                        body += CreateLine("Size05_Req", "");
                        body += CreateLine("Size06_Req", "");
                    }
                    else if (size.Equals("xl"))
                    {
                        body += CreateLine("Size01_Req", "");
                        body += CreateLine("Size02_Req", "");
                        body += CreateLine("Size03_Req", "");
                        body += CreateLine("Size04_Req", lineItem.Quantity.ToString());
                        body += CreateLine("Size05_Req", "");
                        body += CreateLine("Size06_Req", "");
                    }
                    else if (size.Equals("xxl"))
                    {
                        body += CreateLine("Size01_Req", "");
                        body += CreateLine("Size02_Req", "");
                        body += CreateLine("Size03_Req", "");
                        body += CreateLine("Size04_Req", "");
                        body += CreateLine("Size05_Req", lineItem.Quantity.ToString());
                        body += CreateLine("Size06_Req", "");
                    }
                    else
                    {
                        body += CreateLine("Size01_Req", "");
                        body += CreateLine("Size02_Req", "");
                        body += CreateLine("Size03_Req", "");
                        body += CreateLine("Size04_Req", "");
                        body += CreateLine("Size05_Req", "");
                        body += CreateLine("Size06_Req", lineItem.Quantity.ToString());
                    }

                    body += CreateHeader("End Product");
                    }

                // PAYMENT:
                body += CreateHeader("Start Payment ");
                body += CreateLine("date_Payment", request.Order.CreatedAt.ToString("MM/dd/yyyy"));
                body += CreateLine("cur_Payment", request.Order.TotalPrice.ToString());
                if (request.Order.PaymentDetails != null)
                {
                    body += CreateLine("PaymentType", request.Order.PaymentDetails.CreditCardCompany);
                    //body += CreateLine("PaymentNumber", request.Order.PaymentDetails.CreditCardNumber);
                }
                body += CreateLine("Card_Name_Last", request.Order.BillingAddress.FirstName);
                body += CreateLine("Card_Name_First", request.Order.BillingAddress.LastName);
                body += CreateLine("Notes", "Payment processed in shopify");
                body += CreateHeader("End Payment ");

                body += Environment.NewLine + "\"" + Environment.NewLine;

                response.FileName = "ShopifyOrder_" + request.Order.OrderNumber + "_" + now.ToString("yyyyMMddhhmmss") + ".txt";

                response.File = System.Text.Encoding.ASCII.GetBytes(body);
                response.IsSuccess = true;
                return response;
            }
            catch (Exception ex)
            {
                LoggingService.LogError(new LogErrorRequest { ex = ex });
                return new CreateShopWorksOrderFileResponse { ErrorMessage = ex.Message };
            }
        }

        private static string CreateHeader(string name)
        {
            return Environment.NewLine + "---- " + name.Trim() + " ----" + Environment.NewLine;
        }

        private static string CreateLine(string key, string value, bool extraLine = false)
        {
            if (string.IsNullOrEmpty(key)) return "";
            return key + ": " + value + Environment.NewLine + (extraLine ? Environment.NewLine : "");
        }
    }
}