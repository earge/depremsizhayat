using DepremsizHayat.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DepremsizHayat.DTO.User
{
    public class UserLoginRequest
    {
        private string password { get; set; }
        [Required]
        public string E_MAIL { get; set; }
        [Required]
        public string PASSWORD 
        {
            get { return password; }
            set { password = Encryptor.Encrypt(value); }
        }
    }
}
