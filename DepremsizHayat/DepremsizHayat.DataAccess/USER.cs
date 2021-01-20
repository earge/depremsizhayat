using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        [Required]
        public DateTime CREATED_DATE { get; set; }
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
        [MaxLength(50)]
        public string ACTIVATION_CODE { get; set; }
        [DefaultValue(null)]
        public Nullable<DateTime> LAST_ANSWER_DATE { get; set; }
        public Nullable<int> MAX_COUNT_REQUEST { get; set; }
        public Nullable<int> COUNT_ANSWER { get; set; }
        [MaxLength(50)]
        public string HASH_HELPER { get; set; }

    }
}
