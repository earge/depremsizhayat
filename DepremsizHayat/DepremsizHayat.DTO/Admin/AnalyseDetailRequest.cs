using DepremsizHayat.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DepremsizHayat.DTO.Admin
{
    public class AnalyseDetailRequest
    {
        public string ANALYSIS_REQUEST_ID
        {
            get { return analysis_request_id; }
            set { analysis_request_id = Encryptor.Encrypt(value); }
        }
        private string analysis_request_id { get; set; }
        public DateTime CREATED_DATE { get; set; }
        public string ADDRESS { get; set; }
        public string PHONE_NUMBER_1 { get; set; }
        public string PHONE_NUMBER_2 { get; set; }
        public string USER_NOTE { get; set; }
        public string ANSWER { get; set; }
        public int? RISK_SCORE { get; set; }
        public List<string> IMAGES { get; set; }
    }
}
