using DepremsizHayat.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DepremsizHayat.DTO.User
{
    public class ResetPasswordRequest
    {
        private string password { get; set; }
        private string helper { get; set; }
        public string Mail { get; set; }
        public string NewPassword
        {
            get { return password; }
            set { password = Encryptor.Encrypt(value); }
        }
        public string PASSWORD_RESET_HELPER
        {
            get { return Decryptor.Decrypt(helper); }
            set { helper = Encryptor.Encrypt(value); }
        }
    }
}
