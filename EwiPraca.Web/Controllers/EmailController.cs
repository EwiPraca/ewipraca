using EwiPraca.Services.Interfaces;
using System;
using System.Web.Mvc;
using EwiPraca.Model;
using EwiPraca.Encryptor;
using NLog;
using EwiPraca.Attributes;
using EwiPraca.Model.EmployeeArea;

namespace EwiPraca.Controllers
{
    [CookieConsent]
    public class EmailController : Controller
    {
        private readonly IEmailMessageService _emailMessageService;
        private readonly IOSHTrainingService _oshTrainingService;
        private readonly IMedicalReportService _medicalReportService;
        private readonly ILeaveService _leaveService;
        private readonly ICustomEventService _customEventService;
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public EmailController(IEmailMessageService emailMessageService,
            IOSHTrainingService oshTrainingMessageService,
            IMedicalReportService medicalReportService,
            ILeaveService leaveService,
            ICustomEventService customEventService)
        {
            _emailMessageService = emailMessageService;
            _oshTrainingService = oshTrainingMessageService;
            _medicalReportService = medicalReportService;
            _leaveService = leaveService;
            _customEventService = customEventService;
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

        public ActionResult SendLeaveReminderNotifications()
        {
            foreach (var leave in _leaveService.GetLeavesToRemind())
            {
                try
                {
                    var message = CreateLeaveReminderMessage(leave);

                    var result = _emailMessageService.SendEmailMessage(message);
                }
                catch (Exception e)
                {
                    logger.Error(e, e.Message);
                }
            }

            return null;
        }

        public ActionResult SendCustomEventReminder()
        {
            foreach (var customEvent in _customEventService.GetCustomEventsToRemind())
            {
                try
                {
                    var message = CreateCustomEventReminderMessage(customEvent);

                    var result = _emailMessageService.SendEmailMessage(message);
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

        private EmailMessage CreateLeaveReminderMessage(Leave leave)
        {
            var applicationUser = leave.Employee.UserCompany.ApplicationUser;

            EmailMessage message = new EmailMessage()
            {
                EmailType = Enumerations.EmailType.OSHTrainingExpiredReminder,
                EmployeeId = leave.EmployeeId,
                ApplicationUserID = applicationUser.Id,
                Recipient = EncryptionService.DecryptEmail(applicationUser.Email),
                Body = string.Format(WebResources.LeaveReminderMessageBody, string.Format("{0} {1}",leave.Employee.FirstName, leave.Employee.Surname), leave.DateFrom.ToShortDateString(), leave.DateTo.ToShortTimeString()),
                Subject = "Przypomnienie o nadchodzącym urlopie Twojego pracownika"
            };

            _emailMessageService.Create(message);

            return message;
        }

        private EmailMessage CreateCustomEventReminderMessage(CustomEvent customEvent)
        {
            var applicationUser = customEvent.Company.ApplicationUser;

            EmailMessage message = new EmailMessage()
            {
                EmailType = Enumerations.EmailType.CustomEventReminder,
                ApplicationUserID = applicationUser.Id,
                Recipient = EncryptionService.DecryptEmail(applicationUser.Email),
                Body = string.Format(WebResources.CustomEventReminderMessageBody, customEvent.Title),
                Subject = "Przypomnienie o nadchodzącym zdarzeniu z Twojego kalendarza"
            };

            _emailMessageService.Create(message);

            return message;
        }
    }
}