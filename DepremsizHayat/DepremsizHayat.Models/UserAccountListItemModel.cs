using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DepremsizHayat.Models
{
    public class UserAccountListItemModel:BaseModel
    {
        public int ACCOUNT_ID { get; set; }
        public string FIRST_NAME { get; set; }
        public string LAST_NAME { get; set; }
        public string E_MAIL { get; set; }
        public string PASSWORD { get; set; }
        public Nullable<bool> IS_ACTIVE { get; set; }
        public Nullable<int> USER_ROLE_ID { get; set; }
        public string ACTIVATIONCODE { get; set; }
    }
}
