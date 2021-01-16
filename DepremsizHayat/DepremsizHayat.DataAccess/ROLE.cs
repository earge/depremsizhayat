using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DepremsizHayat.DataAccess
{
    public class ROLE
    {
        [Key]
        public int ROLE_ID { get; set; }
        [Required]
        public DateTime CREATED_DATE { get; set; }
        [Required]
        public bool ACTIVE { get; set; }
        [Required]
        public bool DELETED { get; set; }
        [Required]
        public string NAME { get; set; }

    }
}
