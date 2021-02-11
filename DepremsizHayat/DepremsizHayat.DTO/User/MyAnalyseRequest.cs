﻿using DepremsizHayat.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DepremsizHayat.DTO.User
{
    public class MyAnalyseRequest
    {
        private string user_account_id { get; set; }
        public string USER_ACCOUNT_ID
        {
            get { return user_account_id; }
            set { user_account_id = Encryptor.Encrypt(value); }
        }
        public string ANALYSIS_REQUEST_ID
        {
            get { return analysis_request_id; }
            set { analysis_request_id = Encryptor.Encrypt(value); }
        }
        private string analysis_request_id { get; set; }
        public string STATUS_NAME { get; set; }
        private string status_id { get; set; }
        public string STATUS_ID
        {
            get { return status_id; }
            set { status_id = Encryptor.Encrypt(value); }
        }
        public int YEAR_OF_CONSTRUCTION { get; set; }
        public string COUNTRY { get; set; }
        public string DISTRICT { get; set; }
        public int NUMBER_OF_FLOORS { get; set; }
        public string PHONE_NUMBER_1 { get; set; }
        public string PHONE_NUMBER_2 { get; set; }
        public DateTime CREATED_DATE { get; set; }
    }
}