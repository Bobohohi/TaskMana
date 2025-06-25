using System.Net.Mail;
using System.Net;

namespace Task_API.EmailHelper
{
    public class EmailHelper
    {
        public static async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var message = new MailMessage();
            message.To.Add(toEmail);
            message.Subject = subject;
            message.From = new MailAddress("hoangvipa15@gmail.com","WolfTask Daily");
            message.IsBodyHtml = true;
            message.Body = $@"
                <div style='font-family: Arial, sans-serif; max-width: 600px; padding: 20px; border: 1px solid #ddd; border-radius: 10px; background-color: #f9f9f9;'>
                    <h2 style='color: #333;'>WolfTask - Verification Code</h2>
                    <p>Hello,</p>
                    <p>Here is the verification code you requested:</p>
                    <div style='font-size: 24px; font-weight: bold; color: #2d89ef; background-color: #eef3fb; padding: 10px 20px; display: inline-block; border-radius: 5px;'>
                        {body}
                    </div>
                    <p style='margin-top: 20px;'>If you did not request this code, please ignore this email.</p>
                    <p>Best regards,<br/>WolfTask Team</p>
                </div>
                ";
            using var smtp = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential("hoangvipa15@gmail.com", "qwwd rzus gazq cnjp"),
                EnableSsl = true
            };

            await smtp.SendMailAsync(message);
        }
    }
}
