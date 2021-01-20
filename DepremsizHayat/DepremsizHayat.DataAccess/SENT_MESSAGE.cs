using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DepremsizHayat.DataAccess
{
    public class SENT_MESSAGE
    {
        [Key]
        public int SENT_E_MAIL_ID { get; set; }
        [Required]
        public string FROM_ADDRESS { get; set; }
        [Required]
        public string TO_ADDRESS { get; set; }
        [Required]
        public int MESSAGE_TEMPLATE_ID { get; set; }
        public DateTime SENT_DATE { get; set; }
    }
}
