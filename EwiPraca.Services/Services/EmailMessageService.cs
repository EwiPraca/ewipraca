using EwiPraca.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using EwiPraca.Model;
using EwiPraca.Data.Interfaces;
using System;
using System.Net.Mail;
using System.Net;
using System.Configuration;

namespace EwiPraca.Services.Services
{
    public class EmailMessageService : IEmailMessageService
    {
        private readonly IRepository<EmailMessage> _emailMessageRepository;
        private SmtpClient client;

        public EmailMessageService(IRepository<EmailMessage> emailMessageRepository)
        {
            _emailMessageRepository = emailMessageRepository;
            client = new SmtpClient();
        }

        public IEnumerable<EmailMessage> All()
        {
            return _emailMessageRepository.All().ToList();
        }

        public int Create(EmailMessage entity)
        {
            _emailMessageRepository.Insert(entity);

            return entity.Id;
        }

        public void Delete(EmailMessage entity)
        {
            _emailMessageRepository.Delete(entity);
        }

        public EmailMessage GetById(int id)
        {
            return _emailMessageRepository.Query(x => x.Id == id).FirstOrDefault();
        }

        public SendResult SendEmailMessage(EmailMessage message)
        {
            SendResult result = new SendResult()
            {
                Success = true,
                Message = string.Empty
            };

            try
            {
                using (var smtpClient = new SmtpClient())
                {
                    string userName = ConfigurationManager.AppSettings["EmailUsername"];

                    smtpClient.Credentials = new NetworkCredential(userName, ConfigurationManager.AppSettings["EmailPassword"]);

                    using (MailMessage mailMessage = new MailMessage())
                    {
                        mailMessage.From = new MailAddress(userName);
                        mailMessage.To.Add(message.Recipient);
                        mailMessage.Subject = message.Subject;
                        mailMessage.Body = message.Body;

                        smtpClient.Send(mailMessage);
                    }
                }

                message.SentDate = DateTime.Now;

                Update(message);
            }
            catch(Exception e)
            {
                result.Message = e.Message;
                result.Success = false;
            }            

            return result;
        }

        public void Update(EmailMessage entity)
        {
            _emailMessageRepository.Update(entity);
        }
    }
}
