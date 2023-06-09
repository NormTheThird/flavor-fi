using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;

namespace FlavorFi.Common.Helpers
{
    public static class Bitly
    {
        public static string Shorten(string _url)
        {
            try
            {
                var accessToekn = "65f6191cf146de121e41fd4a9a49052442971f80";
                var url = string.Format("https://api-ssl.bitly.com/v3/shorten?format=txt&longUrl={0}&access_token={1}", HttpUtility.UrlEncode(_url), accessToekn);

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                WebResponse response = request.GetResponse();
                using (Stream responseStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                    JavaScriptSerializer js = new JavaScriptSerializer();
                    return reader.ReadToEnd();
                }
            }
            catch (Exception)
            {
                return _url;
            }
        }
    }
}