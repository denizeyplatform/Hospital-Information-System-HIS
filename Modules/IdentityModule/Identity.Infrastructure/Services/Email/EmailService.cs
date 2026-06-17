using Identity.Application.Contracts.Interface;
using Identity.Domain.ValueObjects;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MimeKit;
using static System.Net.Mime.MediaTypeNames;

namespace Identity.Infrastructure.Services.Email
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _settings;
        public EmailService(IOptions<EmailSettings> options)
        {
            _settings = options.Value;
        }
        public async Task SendAsync(string to,string subject,string body)
        {
            var msg = new MimeMessage();
            msg.From.Add(MailboxAddress.Parse(_settings.From));
            msg.To.Add(MailboxAddress.Parse(to));
            msg.Subject =subject;
            msg.Body = new TextPart("plain") { Text = body };

            using var client = new MailKit.Net.Smtp.SmtpClient();
            await client.ConnectAsync(_settings.Host, _settings.Port, MailKit.Security.SecureSocketOptions.SslOnConnect);
            await client.AuthenticateAsync(_settings.From, _settings.Password);
            await client.SendAsync(msg);
            await client.DisconnectAsync(true);

        }
    }
}
