using DepremsizHayat.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DepremsizHayat.DTO.User
{
    public class EditProfileRequest
    {
        private string user_account_id { get; set; }
        public string USER_ACCOUNT_ID
        {
            get { return user_account_id; }
            set { user_account_id = Encryptor.Encrypt(value); }
        }

        public string Name { get; set; }

        public string Surname { get; set; }
        private string password { get; set; }
        public string Password
        {
            get { return password; }
            set { password = Encryptor.Encrypt(Password); }
        }
    }
}
