using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DepremsizHayat.DataAccess
{
    public class MESSAGE_QUERY
    {
        [Key]
        public int MESSAGE_QUERY_ID { get; set; }
        public DateTime SENT_DATE { get; set; }
        public int SENT_EMAIL_ID { get; set; }
    }
}
