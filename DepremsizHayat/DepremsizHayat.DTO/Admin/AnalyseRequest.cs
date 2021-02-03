using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DepremsizHayat.DTO.Admin
{
    public class AnalyseRequest
    {
        public int USER_ACCOUNT_ID { get; set; }
        public string FIRST_NAME { get; set; }
        public string LAST_NAME { get; set; }
        public int ANALYSIS_REQUEST_ID { get; set; }
        public DateTime CREATED_DATE { get; set; }
        public string STATUS { get; set; }
        public string ADDRESS { get; set; }
        public string PHONE_NUMBER_1 { get; set; }
        public string PHONE_NUMBER_2 { get; set; }
        public int YEAR_OF_CONSTRUCTION { get; set; }
        public string COUNTRY { get; set; }
        public string DISTRICT { get; set; }
        public int NUMBER_OF_FLOORS { get; set; }
        public string USER_NOTE { get; set; }
    }
}
