﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading.Tasks;

namespace Portfolio.Services
{
    // This class is used by the application to send email for account confirmation and password reset.
    // For more details see https://go.microsoft.com/fwlink/?LinkID=532713
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string message)
        {
            //commented out email verification for testing purposes
            try
            {
                MailMessage mailMsg = new MailMessage();

                // To
                mailMsg.To.Add(new MailAddress(email, "To Name"));

                // From
                mailMsg.From = new MailAddress("info@gifthub.com", "Customer Service");

                // Subject and multipart/alternative Body
                mailMsg.Subject = subject;


                // You can send text instead of HTML if you prefer.
                // mailMsg.AlternateViews.Add(
                //         AlternateView.CreateAlternateViewFromString(message, 
                //         null, MediaTypeNames.Text.Plain));

                // Use this technique to add HTML tags if you need.  
                // string html = @"<p>html body</p>";
                mailMsg.AlternateViews.Add(
                        AlternateView.CreateAlternateViewFromString(message,
                        null, MediaTypeNames.Text.Html));

                // Init SmtpClient and send
                SmtpClient smtpClient
                = new SmtpClient("smtp.sendgrid.net", Convert.ToInt32(587));

                System.Net.NetworkCredential credentials
                = new System.Net.NetworkCredential("atalene", "BCITssd2018");

                smtpClient.Credentials = credentials;
                smtpClient.Send(mailMsg);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return Task.CompletedTask;
        }
    }
}
