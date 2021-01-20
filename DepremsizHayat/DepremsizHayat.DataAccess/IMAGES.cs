using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DepremsizHayat.DataAccess
{
    public class IMAGES
    {
        [Key]
        public int IMAGE_ID { get; set; }
        [Required]
        public string URL { get; set; }
        [Required]
        public DateTime CREATED_DATE { get; set; }
        [Required]
        public bool DELETED { get; set; }
        [Required]
        public int ENTITY_ID { get; set; }
        [Required]
        [MaxLength(50)]
        public string ENTITY_TYPE_CODE { get; set; }
        [MaxLength(500)]
        public string COMMENT { get; set; }
    }
}
