using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DepremsizHayat.DataAccess;
using DepremsizHayat.Security;

namespace DepremsizHayat.DTO.Admin
{
    public class EditRoleRequest
    {
        private string user_account_id { get; set; }
        public string USER_ACCOUNT_ID
        {
            get { return user_account_id; }
            set { user_account_id = Encryptor.Encrypt(value); }
        }
        public int NEW_ROLE_ID { get; set; }
    }
}
