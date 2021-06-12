using DepremsizHayat.DataAccess;
using DepremsizHayat.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DepremsizHayat.DTO.Admin
{
    public class ExpertNotAnsweredRequest
    {
        public string USER_ANALYSE_REQUEST_ID
        {
            get { return user_analyse_request_id; }
            set { user_analyse_request_id = Encryptor.Encrypt(value); }
        }
        private string user_analyse_request_id { get; set; }
        public string ANALYSE_REQUEST_ID
        {
            get { return analyse_request_id; }
            set { analyse_request_id = Encryptor.Encrypt(value); }
        }
        private string analyse_request_id { get; set; }
        public USER_ACCOUNT EXPERT_USER { get; set; }
        public USER_ACCOUNT REQUESTER_USER { get; set; }

        public DateTime CREATED_DATE { get; set; }
        public bool ACTIVE { get; set; }
        public bool DELETED { get; set; }
        public ANALYSE_REQUEST ANALYSE_REQUEST { get; set; }
        public USER_ANALYSE_REQUEST_STATUS STATUS { get; set; }
        public string USER_NOTE { get; set; }
    }
}
