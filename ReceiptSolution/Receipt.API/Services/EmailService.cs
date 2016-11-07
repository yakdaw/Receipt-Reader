namespace Receipt.API.Services
{
    using System;
    using System.Net.Mail;

    public class EmailService
    {
        public Exception SendLostPasswordMail(string email, string accessToken)
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient smtpServer = new SmtpClient("smtp.gmail.com");

                mail.From = new MailAddress("receiptreader.service@gmail.com");
                mail.To.Add(email);
                mail.Subject = "ReceiptReader password reset";
                mail.Body = "Your access token to reset the password: " + accessToken;

                smtpServer.Port = 587;
                smtpServer.Credentials = new System.Net.NetworkCredential("receiptreader.service@gmail.com", "jackamwoj");
                smtpServer.EnableSsl = true;

                smtpServer.Send(mail);
            }
            catch (Exception ex)
            {
                return ex;
            }

            return null;
        }
    }
}