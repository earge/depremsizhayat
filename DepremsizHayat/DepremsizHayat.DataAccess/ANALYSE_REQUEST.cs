using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DepremsizHayat.DataAccess
{
    public class ANALYSE_REQUEST
    {
        [Key]
        public int ANALYSIS_REQUEST_ID { get; set; }
        [Required]
        public DateTime CREATED_DATE { get; set; }
        [Required]
        public int USER_ACCOUNT_ID { get; set; }
        [Required]
        public int STATUS_ID { get; set; }
        [Required]
        public bool DELETED { get; set; }
        [Required]
        public string ADDRESS { get; set; }
        [MaxLength(20)]
        public string PHONE_NUMBER_1 { get; set; }
        [MaxLength(20)]
        public string PHONE_NUMBER_2 { get; set; }
        [Required]
        public int YEAR_OF_CONSTRUCTION { get; set; }
        [Required]
        [MaxLength(100)]
        public string COUNTRY { get; set; }
        [Required]
        [MaxLength(100)]
        public string DISTRICT { get; set; }
        [Required]
        public int NUMBER_OF_FLOORS { get; set; }
        [MaxLength(500)]
        public string USER_NOTE { get; set; }
        public virtual STATUS STATUS { get; set; }
    }
}
