using System;
using System.ComponentModel.DataAnnotations;

namespace DepremsizHayat.DataAccess
{
    public class FILE_TYPE
    {
        [Key]
        public int FILE_TYPE_ID { get; set; }
        [MaxLength(10)]
        public string NAME { get; set; }
        public string DESCRIPTION { get; set; }
        public Nullable<bool> ACTIVE { get; set; }
    }
}
