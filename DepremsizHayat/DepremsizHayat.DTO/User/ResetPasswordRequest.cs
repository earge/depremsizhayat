﻿using DepremsizHayat.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DepremsizHayat.DTO.User
{
    public class ResetPasswordRequest
    {
        private string password { get; set; }
        public string Mail { get; set; }
        public string Password
        {
            get { return password; }
            set { password = Encryptor.Encrypt(value); }
        }
    }
}
