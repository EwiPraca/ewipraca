using EwiPraca.Model;

namespace EwiPraca.Services.Interfaces
{
    public interface IEmailMessageService : IService<EmailMessage>
    {
        SendResult SendEmailMessage(EmailMessage message);
    }
}
