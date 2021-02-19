using DepremsizHayat.Business.BaseRepository;
using DepremsizHayat.DataAccess;
using DepremsizHayat.DTO.Models;
using DepremsizHayat.DTO.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DepremsizHayat.Business.IServiceRepository
{
    public interface IUserRepository:IRepository<USER_ACCOUNT>
    {
        USER_ACCOUNT GetByMail(string mail);
        USER_ACCOUNT GetByMailForProcedure(string mail);
        USER_ACCOUNT CreateUser(UserModel user);
        bool Login(UserModel user);
        bool CreateForgottenPwdResetRequest(string mail);
        bool ResetForgottenPassword(ResetPasswordRequest request);
        USER_ACCOUNT GetByResetAuth(string authCode);
        bool ResetPassword(string mail, string pwd);
        List<USER_ACCOUNT> GetAdmins();

    }
}
