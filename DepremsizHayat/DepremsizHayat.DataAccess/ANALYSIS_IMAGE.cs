using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DepremsizHayat.DataAccess
{
    public class ANALYSIS_IMAGE
    {
        [Key]
        public int IMAGE_ID { get; set; }
        [Required]
        public int ANALYSIS_REQUEST_ID { get; set; }
        [Required]
        public string NAME { get; set; }
        [Required]
        public DateTime CREATED_DATE { get; set; }
        [Required]
        public bool IS_FROM_EXPERT { get; set; }
        [Required]
        public bool DELETED { get; set; }
    }
}
