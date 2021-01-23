﻿using DepremsizHayat.DataAccess;
using DepremsizHayat.DTO.Models;
using DepremsizHayat.DTO.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DepremsizHayat.Business.IService
{
    public interface IUserService
    {
        List<USER> GetAll();
        USER CreateUser(UserModel user);
        bool Login(string mail,string pwd);
        bool Activate(string actCode,string mail);
        USER GetByMail(string mail);
        bool ResetPassword(ResetPasswordRequest request);
        string SendResetMail(string mail);
        bool CheckResetAuth(string code, string mail);
    }
}
