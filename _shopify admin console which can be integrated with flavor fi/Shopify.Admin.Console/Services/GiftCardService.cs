using Newtonsoft.Json.Linq;
using Shopify.Admin.Console.RequestAndResponses;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web.Script.Serialization;

namespace Shopify.Admin.Console.Services
{
    public static class GiftCardService
    {
        public static SaveGiftCardResponse SaveGiftCard(SaveGiftCardRequest _request)
        {
            try
            {
                var response = new SaveGiftCardResponse();
                var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://fillyflairdev.myshopify.com/admin/gift_cards.json");
                httpWebRequest.Credentials = new NetworkCredential("20db1a52bc41d4bef974884132743955", "5c6fbd097ca585d05d40e620c1bc3be0");
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Accept = "application/json";
                httpWebRequest.Method = "POST";

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    string json = "{\"gift_card\": {\"initial_value\": " + _request.InitialValue.ToString("#.##") + ", \"customer_id\": " + _request.UserId + "}}" ;
                    streamWriter.Write(json);
                    streamWriter.Flush();
                    streamWriter.Close();
                }

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    JavaScriptSerializer js = new JavaScriptSerializer();
                    var result = streamReader.ReadToEnd();
                    dynamic data = JObject.Parse(result);
                    response.GiftCard.Last4Characters = data["gift_card"]["last_characters"].Value;
                    response.GiftCard.Code = data["gift_card"]["code"].Value;
                    response.GiftCard.Balance = Convert.ToDecimal(data["gift_card"]["balance"].Value);
                }

                response.IsSuccess = true;
                return response;
            }
            catch (Exception ex)
            {
                return new SaveGiftCardResponse { ErrorMessage = ex.Message };
            }
        }

        private static void SetBasicAuthHeader(WebRequest request, String userName, String userPassword)
        {
            string authInfo = userName + ":" + userPassword;
            authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(authInfo));
            request.Headers["Authorization"] = "Basic " + authInfo;
        }
    }
}