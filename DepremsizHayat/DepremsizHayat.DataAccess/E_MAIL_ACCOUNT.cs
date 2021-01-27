using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DepremsizHayat.DataAccess
{
    public class E_MAIL_ACCOUNT
    {
        [Key]
        public int E_MAIL_ACCOUNT_ID { get; set; }
        [Required]
        public string USER_NAME { get; set; }
        [Required]
        public string HOST { get; set; }
        [Required]
        public string PASSWORD { get; set; }
        public Nullable<bool> ENABLE_SSL { get; set; }
        [Required]
        public int PORT { get; set; }
        [Required]
        public bool ACTIVE { get; set; }
        [Required]
        [MaxLength(50)]
        public string E_MAIL_ACCOUNT_CODE { get; set; }
    }
}
