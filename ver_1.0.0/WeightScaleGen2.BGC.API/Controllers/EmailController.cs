using Asp.Versioning;
using MailKit.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MimeKit;
using System;
using WeightScaleGen2.BGC.Models.ServicesModels;

namespace WeightScaleGen2.BGC.API.Controllers
{
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class SendMailController : ControllerBase
    {
        private readonly IConfiguration _config;
        public SendMailController(IConfiguration config)
        {
            _config = config;
        }


        [HttpGet("sendmailDemo/{sentto}/{type}")]
        public IActionResult sendMailDemo(string sentto, string type)
        {
            try
            {
                var _email = new MimeMessage();
                _email.From.Add(MailboxAddress.Parse(_config["Email:Username"]));


                _email.To.Add(MailboxAddress.Parse(sentto));

                var subject = TypeSubject(type);
                var body = TypeBody(type);
                _email.Subject = subject;
                _email.Body = new TextPart("plain")
                {
                    Text = @$"{body}"
                };

                // send email
                using (var smtp = new MailKit.Net.Smtp.SmtpClient())
                {
                    smtp.Connect("webmail.bgiglass.com", 587, SecureSocketOptions.StartTls);
                    smtp.Authenticate(_config["Email:Username2"], _config["Email:Password"]);
                    smtp.Send(_email);
                    smtp.Disconnect(true);
                    return StatusCode(200, new { message = "mail" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { result = ex, message = "Internal Server Error" });
            }
        }

        [HttpPost("sendmail")]
        public IActionResult sendMail(SendMail mail)
        {
            try
            {
                var _email = new MimeMessage();
                _email.From.Add(MailboxAddress.Parse(_config["Email:Username"]));

                foreach (var email in mail.Email)
                {
                    _email.To.Add(MailboxAddress.Parse(email));
                }
                var subject = TypeSubject(mail.Type);
                var body = TypeBody(mail.Type);
                _email.Subject = subject;
                _email.Body = new TextPart("plain")
                {
                    Text = @$"{body}"
                };

                // send email
                using (var smtp = new MailKit.Net.Smtp.SmtpClient())
                {
                    smtp.Connect("webmail.bgiglass.com", 587, SecureSocketOptions.StartTls);
                    smtp.Authenticate(_config["Email:Username2"], _config["Email:Password"]);
                    smtp.Send(_email);
                    smtp.Disconnect(true);
                    return StatusCode(200, new { message = "mail" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { result = ex, message = "Internal Server Error" });
            }

        }

        private string TypeSubject(string Type)
        {
            switch (Type)
            {
                case "Demo":
                    return @$"SW Dev. Demo Email";
                default:
                    return @$"SW Dev. Demo Email"; ;
            }
        }

        private string TypeBody(string Type)
        {

            switch (Type)
            {
                case "Demo":
                    return @$"Dear SW Dev,
                                    
                                This is Demo Email.
                                For test system only. 
                            
                            Comment : None

                            Best Regards,
                            SW Dev system";

                default:
                    return @$"Dear SW Dev,
                                    
                                This is Demo Email.
                                For test system only. 
                            
                            Comment : None

                            Best Regards,
                            SW Dev system";
            }
        }
    }
}
