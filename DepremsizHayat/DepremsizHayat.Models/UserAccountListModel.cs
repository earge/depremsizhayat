using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DepremsizHayat.Models
{
    public class UserAccountListModel : BaseModel
    {
        public List<UserAccountListItemModel> Items { get; set; }
    }
}
