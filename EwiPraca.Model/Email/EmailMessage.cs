using EwiPraca.Data;
using EwiPraca.Enumerations;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace EwiPraca.Model
{
    public class EmailMessage
    {
        public int Id { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string Recipient { get; set; }
        public DateTime? SentDate { get; set; }
        public EmailType EmailType { get; set; }
        public int? EmployeeId { get; set; }
        public virtual Employee Employee { get; set; }
        [ForeignKey("ApplicationUser")]
        public string ApplicationUserID { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
