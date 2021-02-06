using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DepremsizHayat.DTO.Admin
{
    public class AnalyseDetailRequest
    {
        public DateTime CREATED_DATE { get; set; }
        public string ADDRESS { get; set; }
        public string PHONE_NUMBER_1 { get; set; }
        public string PHONE_NUMBER_2 { get; set; }
        public string USER_NOTE { get; set; }
    }
}
