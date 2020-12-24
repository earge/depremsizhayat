using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DepremsizHayat.Models
{
    public class UserAccountCreateModel : BaseModel
    {
        [Required]
        public string FIRST_NAME { get; set; }
        [Required]
        public string LAST_NAME { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Geçersiz e-posta adresi.")]
        public string E_MAIL { get; set; }
        [Required]
        public string PASSWORD { get; set; }
        [Required]
        [Compare("PASSWORD", ErrorMessage = "Şifreler eşleşmiyor.")]
        public string PASSWORD_VALIDATION { get; set; }
        public string ACTIVATIONCODE { get; set; }
        public int ROLE_ID { get; set; }
    }
}
