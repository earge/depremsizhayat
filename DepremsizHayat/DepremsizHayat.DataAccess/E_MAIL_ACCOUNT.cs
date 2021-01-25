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
        public string FROM_ADDRESS { get; set; }
        [Required]
        public string TO_ADDRESS { get; set; }
        [Required]
        public int MESSAGE_TEMPLATE_ID { get; set; }
        [DefaultValue(null)]
        public Nullable<DateTime> SENT_DATE { get; set; }
    }
}
