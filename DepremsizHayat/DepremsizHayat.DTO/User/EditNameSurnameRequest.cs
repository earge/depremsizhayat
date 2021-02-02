using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DepremsizHayat.DTO.User
{
    public class EditNameSurnameRequest
    {
        public int USER_ACCOUNT_ID { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }
    }
}
