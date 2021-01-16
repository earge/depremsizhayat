using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DepremsizHayat.DataAccess
{
    public class USER
    {
        [Key]
        public int USER_ID { get; set; }
        [Required]
        public int ROLE_ID { get; set; }
        //public DateTime CREATED_DATE { get; set; }
        [Required]
        public bool ACTIVE { get; set; }
        [Required]
        public bool DELETED { get; set; }
        [Required]
        public string FIRST_NAME { get; set; }
        [Required]
        public string LAST_NAME { get; set; }
        [Required]
        public string E_MAIL { get; set; }
        [Required]
        public string PASSWORD { get; set; }
        //public DateTime LAST_ANSWER_DATE { get; set; }
        public int MAX_COUNT_REQUEST { get; set; }
        public int COUNT_ANSWER { get; set; }
        public string PROFILE_IMAGE { get; set; }

    }
}
