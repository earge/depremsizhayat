using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DepremsizHayat.DataAccess
{
    public class USER_ANALYSE_REQUEST
    {
        [Key]
        public int USER_ANALYSE_REQUEST_ID { get; set; }
        public Nullable<int> ANALYSE_REQUEST_ID { get; set; }
        public Nullable<int> USER_ACCOUNT_ID { get; set; }
        public Nullable<DateTime> CREATED_DATE { get; set; }
        public Nullable<bool> ACTIVE { get; set; }
        public Nullable<bool> DELETED { get; set; }
        [MaxLength(50)]
        public string USER_ANALYSE_REQ_STATUS_CODE { get; set; }
        public virtual USER_ANALYSE_REQUEST_STATUS USER_ANALYSE_REQUEST_STATUS { get; set; }
    }
}
