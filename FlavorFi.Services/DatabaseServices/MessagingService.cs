using System;
using System.Net.Mail;
using System.Net;
using SendGrid;
using FlavorFi.Common.RequestAndResponses.DatabaseRequestAndResponses;
using System.Configuration;
using SendGrid.Helpers.Mail;

namespace FlavorFi.Services.DatabaseServices
{
    public interface IMessagingService : IBaseService
    {
        SendEmailResponse SendEmail(SendEmailRequest request);
        SendEmailResponse SendResetEmail(SendResetEmailRequest request);
    }

    public class MessagingService : BaseService, IMessagingService
    {
        public SendEmailResponse SendEmail(SendEmailRequest request)
        {
            try
            {
                var response = new SendEmailResponse();
                //var sendGridMessage = new SendGridMessage();
                //sendGridMessage.From = new MailAddress(request.From);
                //sendGridMessage.AddTo(request.Recipients);
                //sendGridMessage.Subject = request.Subject;
                //sendGridMessage.Text = request.Body;

                //var credintials = GetCredentials();
                //var transportWeb = new Web(credintials);
                //transportWeb.Deliver(sendGridMessage);
                response.IsSuccess = true;
                return response;
            }
            catch (Exception ex)
            {
                LoggingService.LogError(new LogErrorRequest { ex = ex });
                return new SendEmailResponse { ErrorMessage = ex.Message };

            }
        }

        public SendEmailResponse SendResetEmail(SendResetEmailRequest request)
        {
            try
            {
                var response = new SendEmailResponse();
                var client = new SendGridClient(ConfigurationManager.AppSettings["SendGridApiKey"]);
                var from = new EmailAddress("no-reply@flavorfi.io", "Flavor Fi");
                var subject = "Please reset your password.";
                var to = new EmailAddress(request.Recipient);
                var plainTextContent = $"Your Flavor Fi account password has been reset. Please click on the following link to reset your account password. {request.Url}";
                var htmlContent = "<center>" +
                                          "<div style=\"text-align: center; max-width:500px;\" cellspacing=\"0\" cellpadding=\"0\">" +
                                              "<div cellpadding=\"0\" cellspacing=\"0\">" +
                                                  "<div>" +
                                                      "<p style=\"text-align: left;\">Hello, <br> <br>" +
                                                          "Your Flavor Fi account password has been reset. Please click on the following link to reset your account password. <br>" +
                                                          "<b><a class=\"blue-link\" href=\"" + request.Url + "\">Click Here</a></b>" +
                                                      "</p>" +
                                                  "</div>" +
                                              "</div>" +
                                          "</div>" +
                                      "</center>";
                var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
                var emailResponse = client.SendEmailAsync(msg).Result;
                if (emailResponse.StatusCode != HttpStatusCode.Accepted)
                    throw new ApplicationException(emailResponse.StatusCode.ToString());
                response.IsSuccess = true;
                return response;
            }
            catch (Exception ex)
            {
                LoggingService.LogError(new LogErrorRequest { ex = ex });
                return new SendEmailResponse { ErrorMessage = ex.Message };

            }
        }
    }
}