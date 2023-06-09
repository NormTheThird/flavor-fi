using SendGrid;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;

namespace Shopify.Admin.Console.Helpers
{
    public static class EmailHelper
    {
        public static void Send(string _name, string _email, string _link)
        {
            try
            {
                var sendGridMessage = new SendGridMessage();

                sendGridMessage.From = new MailAddress("support@fillyflair.com", "Filly Flair Support");
                sendGridMessage.AddTo(_email);
                //sendGridMessage.AddTo(new List<string> { "WilliamRNorman@Gmail.com", "RogerJepsen@Gmail.com", "clara@fillyflair.com" });
                sendGridMessage.Subject = "Welcome to the New Filly Flair Website";
                //sendGridMessage.Html = GetNewEmailHtml(_name, _link);

                var credintials = new NetworkCredential("fillyflair", "sldiuy3sds");
                var transportWeb = new Web(credintials);
                transportWeb.Deliver(sendGridMessage);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("Unable to send email [Name: " + _name + "][Email: " + _email + "]");
                if (ex.InnerException != null) System.Console.WriteLine("Inner Exception Message: " + ex.InnerException.Message);
                else System.Console.WriteLine("Error Message: " + ex.Message);
            }

        }
        private static string GetNewEmailHtml(string _name, string _link)
        {
            return "<div style='margin-left:40px;'>" +
                            "<img src='https://cdn.shopify.com/s/files/1/1666/8903/files/Filly_Flair_logo-sm.png?7756784769822517949'></img>" +
                            //"<h2 style='color:black;'>Filly Flair</h2>" +
                            "<h4 style='font-family: Arial, Helvetica, sans-serif; color:gray;'>Hey " + _name + ",</h4>" +
                            "<h4 style='font-family: Arial, Helvetica, sans-serif; color:gray;'>We are so excited that our new website is now live. In order to view your account, you will need to activate it. " +
                            "Click the link below to activate your account and set your new password.</h4>" +
                            "<h4 style='font-family: Arial, Helvetica, sans-serif; color:gray;'>This link will expire in 7 days.</h4>" +
                            "<br />" +
                            "<h4><a href='" + _link.Trim() + "' style='padding:20px; font-family: \"Trebuchet MS\", Helvetica, sans-serif; border-radius:5px; color:#FFF; background-color:#54b0af; text-decoration:none; line-height:30px;'><b>Activate your account</b></a></h4>" +
                            "<br>" +
                            "<h4 style='font-family: Arial, Helvetica, sans-serif; color:gray;'>Or paste this unique link into your browser: <a href='" + _link.Trim() + "' style='color:#54b0af'>" + _link.Trim() + "</a></h4>" +
                            "<h4 style='font-family: Arial, Helvetica, sans-serif; color:gray;'>If you have any questions about our new site please contact us at <a href='mailto:support@fillyflair.com' style='color:#54b0af'>support@fillyflair.com</a> or (855) 777-3455.</h4>" +
                            "<br><br>" +
                        "</div>" +
                        "<br>";
        }

        private static string GetGiftCardEmailHtml(string _name, string _link)
        {
            return "<div style='margin-left:40px;'>" +
                            "<img src='https://cdn.shopify.com/s/files/1/1666/8903/files/Filly_Flair_logo-sm.png?7756784769822517949'></img>" +
                            //"<h2 style='color:black;'>Filly Flair</h2>" +
                            "<h4 style='font-family: Arial, Helvetica, sans-serif; color:gray;'>Hey " + _name + ",</h4>" +
                            "<h4 style='font-family: Arial, Helvetica, sans-serif; color:gray;'>We hope you've had a chance to visit the new Filly Flair site. Your store credit is now available for your use in a gift card format. You will find your new code below. Please keep this email for your records and to see your gift card balance.</h4>" +
                            "<br />" +
                            "<h4><a href='" + _link.Trim() + "' style='padding:20px; font-family: \"Trebuchet MS\", Helvetica, sans-serif; border-radius:5px; color:#FFF; background-color:#54b0af; text-decoration:none; line-height:30px;'><b>Activate your account</b></a></h4>" +
                            "<br>" +
                            "<h4 style='font-family: Arial, Helvetica, sans-serif; color:gray;'>Or paste this unique link into your browser: <a href='" + _link.Trim() + "' style='color:#54b0af'>" + _link.Trim() + "</a></h4>" +
                            "<h4 style='font-family: Arial, Helvetica, sans-serif; color:gray;'>If you have any questions about our new site please contact us at <a href='mailto:support@fillyflair.com' style='color:#54b0af'>support@fillyflair.com</a> or (855) 777-3455.</h4>" +
                            "<br><br>" +
                        "</div>" +
                        "<br>";




        }
    }
}
