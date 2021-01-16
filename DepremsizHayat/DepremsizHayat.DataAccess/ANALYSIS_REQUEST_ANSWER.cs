using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DepremsizHayat.DataAccess
{
    public class ANALYSIS_REQUEST_ANSWER
    {
        [Key]
        public int ANALYSIS_REQUEST_ANSWER_ID { get; set; }
        [Required]
        public int ANALYSIS_REQUEST_ID { get; set; }
        [Required]
        public string DETAIL { get; set; }
        [Required]
        public DateTime CREATED_DATE { get; set; }
        [Required]
        public bool DELETED { get; set; }
        [Required]
        public int RISK_SCORE { get; set; }
        [Required]
        public int USER_ID { get; set; }
    }
}
