using DepremsizHayat.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DepremsizHayat.DTO.User
{
    public class CreateUserRequest
    {
        private string password { get; set; }
        [Required]
        public string FIRST_NAME { get; set; }
        [Required]
        public string LAST_NAME { get; set; }
        [Required]
        public string E_MAIL { get; set; }
        [Required]
        public string PASSWORD
        {
            get { return password; }
            set { password = Encryptor.Encrypt(value); }
        }
        public string PROFILE_IMAGE { get; set; }
    }
}
