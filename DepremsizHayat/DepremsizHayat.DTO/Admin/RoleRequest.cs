using DepremsizHayat.DataAccess;
using DepremsizHayat.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DepremsizHayat.DTO.Admin
{
    public class RoleRequest
    {
        private string user_account_id { get; set; }
        public string USER_ACCOUNT_ID
        {
            get { return user_account_id; }
            set { user_account_id = Encryptor.Encrypt(value); }
        }
        public string FIRST_NAME { get; set; }
        public string LAST_NAME { get; set; }
        public string E_MAIL { get; set; }
        public ROLE CURRENTROLE { get; set; }
        public List<ROLE> AVAILABLEROLES { get; set; }
    }
}
