using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DepremsizHayat.DataAccess;
namespace DepremsizHayat.DTO.Admin
{
    public class EditRoleRequest
    {
        public int USER_ACCOUNT_ID { get; set; }
        public int NEW_ROLE_ID { get; set; }
    }
}
