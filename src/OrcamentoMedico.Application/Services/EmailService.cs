using System.Net.Mail;
using OrcamentoMedico.Application.Interfaces;

public class EmailService : IEmailService
{
    public async Task SendAsync(string to, string subject, string body)
    {
        using var client = new SmtpClient("localhost", 1025);
        var message = new MailMessage("no-reply@orcamento.com", to, subject, body);
        await client.SendMailAsync(message);
    }
}