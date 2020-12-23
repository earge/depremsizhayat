using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DepremsizHayat.Models
{
    public class UserAccountCreateModel: BaseModel
    {
        public string FIRST_NAME { get; set; }

        public string LAST_NAME { get; set; }

        public string E_MAIL { get; set; }

        public string PASSWORD { get; set; }

        public int ROLE_ID { get; set; }

    }
}
