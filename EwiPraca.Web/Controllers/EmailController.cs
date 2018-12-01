using EwiPraca.Services.Interfaces;
using System;
using System.Web.Mvc;
using EwiPraca.Model;
using EwiPraca.Encryptor;
using NLog;

namespace EwiPraca.Controllers
{
    public class EmailController : Controller
    {
        private readonly IEmailMessageService _emailMessageService;
        private readonly IOSHTrainingService _oshTrainingService;
        private readonly IMedicalReportService _medicalReportService;
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public EmailController(IEmailMessageService emailMessageService,
            IOSHTrainingService oshTrainingMessageService,
            IMedicalReportService medicalReportService)
        {
            _emailMessageService = emailMessageService;
            _oshTrainingService = oshTrainingMessageService;
            _medicalReportService = medicalReportService;
        }

        public ActionResult SendEmailOSHTrainingExpiredReminderNotifications()
        {
            int daysBeforeExpiration = SettingsHandler.DaysBeforeIntervalReminder;
            var oshTrainingsToExpire = _oshTrainingService.GetOSHTrainingsToExpire(daysBeforeExpiration);

            foreach(var oshTraining in oshTrainingsToExpire)
            {
                try
                {
                    var message = CreateOSHTrainingReminderMessage(oshTraining);

                    var result = _emailMessageService.SendEmailMessage(message);

                    oshTraining.ReminderSent = true;
                    _oshTrainingService.Update(oshTraining);
                }
                catch(Exception e)
                {
                    logger.Error(e, e.Message);
                }
            }

            return null;
        }

        public ActionResult SendEmailMedicalReportsExpiredReminderNotifications()
        {
            int daysBeforeExpiration = SettingsHandler.DaysBeforeIntervalReminder;
            var medicalReportsToExpire = _medicalReportService.GetMedicalReportsToExpire(daysBeforeExpiration);

            foreach (var medicalReport in medicalReportsToExpire)
            {
                try
                {
                    var message = CreateMedicalReportReminderMessage(medicalReport);

                    var result = _emailMessageService.SendEmailMessage(message);

                    medicalReport.ReminderSent = true;
                    _medicalReportService.Update(medicalReport);
                }
                catch (Exception e)
                {
                    logger.Error(e, e.Message);
                }
            }

            return null;
        }

        private EmailMessage CreateMedicalReportReminderMessage(MedicalReport medicalReport)
        {
            var applicationUser = medicalReport.Employee.UserCompany.ApplicationUser;

            EmailMessage message = new EmailMessage()
            {
                EmailType = Enumerations.EmailType.OSHTrainingExpiredReminder,
                EmployeeId = medicalReport.EmployeeId,
                ApplicationUserID = applicationUser.Id,
                Recipient = EncryptionService.DecryptEmail(applicationUser.Email),
                Body = WebResources.MedicalResultToExpireReminderEmailBody,
                Subject = WebResources.MedicalResultToExpireReminderEmailTitle
            };

            _emailMessageService.Create(message);

            return message;
        }

        private EmailMessage CreateOSHTrainingReminderMessage(OSHTraining oshTrainingToExpire)
        {
            var applicationUser = oshTrainingToExpire.Employee.UserCompany.ApplicationUser;

            EmailMessage message = new EmailMessage()
            {
                EmailType = Enumerations.EmailType.OSHTrainingExpiredReminder,
                EmployeeId = oshTrainingToExpire.EmployeeId,
                ApplicationUserID = applicationUser.Id,
                Recipient = EncryptionService.DecryptEmail(applicationUser.Email),
                Body = WebResources.OSHTrainingToExpireReminderEmailBody,
                Subject = WebResources.OSHTrainingToExpireReminderEmailTitle
            };

            _emailMessageService.Create(message);

            return message;
        }
    }
}