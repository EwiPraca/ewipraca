using EwiPraca.Data;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EwiPraca.Model
{
    public class ResetPasswordRequest
    {
        public int Id { get; set; }


        [ForeignKey("ApplicationUser")]
        public string ApplicationUserID { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }

        [Required]
        [StringLength(128)]
        public string ResetGuid { get; set; }

        public DateTime? MailSentDate { get; set; }

        [DefaultValue(0)]
        public bool IsCompleted { get; set; }

        public DateTime ValidTo { get; set; }
    }
}
