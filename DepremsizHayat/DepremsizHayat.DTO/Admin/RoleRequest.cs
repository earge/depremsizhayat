using DepremsizHayat.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DepremsizHayat.DTO.Admin
{
    public class RoleRequest
    {
        public int USER_ACCOUNT_ID { get; set; }
        public string FIRST_NAME { get; set; }
        public string LAST_NAME { get; set; }
        public string E_MAIL { get; set; }
        public ROLE CURRENTROLE { get; set; }
        public List<ROLE> AVAILABLEROLES { get; set; }
    }
}
