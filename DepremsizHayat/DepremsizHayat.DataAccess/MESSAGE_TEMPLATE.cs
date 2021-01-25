using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DepremsizHayat.DataAccess
{
    public class MESSAGE_TEMPLATE
    {
        [Key]
        public int MESSAGE_TEMPLATE_ID { get; set; }
        [Required]
        [MaxLength(200)]
        public string SUBJECT { get; set; }
        [Required]
        public string BODY { get; set; }
        [Required]
        public bool ACTIVE { get; set; }
        [MaxLength(250)]
        public string TITLE { get; set; }
    }
}
