using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DepremsizHayat.Models
{
    public class BaseModel
    {
        public List<string> ErrorCodes = new List<string>();

        public bool IsSuccess { get { return ErrorCodes.Count == 0 } }

        public string ErrorCodesCommaSeperated
        {
            get
            {
                return string.Join(",", ErrorCodes);
            }
        }

        public string SuccessMessageCode { get; set; }

    }
}
