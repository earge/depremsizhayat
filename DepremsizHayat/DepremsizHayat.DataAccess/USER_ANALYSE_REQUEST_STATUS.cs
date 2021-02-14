using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DepremsizHayat.DataAccess
{
    public class USER_ANALYSE_REQUEST_STATUS
    {
        [Key]
        [MaxLength(50)]
        public string USER_ANALYSE_REQ_STATUS_CODE { get; set; }
        [Required]
        public string NAME { get; set; }
        public string DESCRIPTION { get; set; }
        [Required]
        [DefaultValue(false)]
        public bool ACTIVE { get; set; }
        public virtual List<USER_ANALYSE_REQUEST> USER_ANALYSE_REQUEST { get; set; }
    }
}
