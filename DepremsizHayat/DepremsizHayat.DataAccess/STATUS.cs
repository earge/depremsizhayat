using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DepremsizHayat.DataAccess
{
    public class STATUS
    {
        [Key]
        public int STATUS_ID { get; set; }
        [Required]
        [MaxLength(100)]
        public string NAME { get; set; }
        public bool ACTIVE { get; set; }
        public DateTime CREATED_DATE { get; set; }
        public string DESCRIPTION { get; set; }
        [MaxLength(50)]
        public string STATUS_CODE { get; set; }

    }
}
