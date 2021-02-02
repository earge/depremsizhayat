using DepremsizHayat.DataAccess;
using DepremsizHayat.DTO;
using DepremsizHayat.DTO.Admin;
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
        List<USER_ACCOUNT> GetAll();
        USER_ACCOUNT CreateUser(UserModel user);
        bool Login(string mail, string pwd);
        bool Activate(string actCode, string mail);
        USER_ACCOUNT GetByMail(string mail);
        BaseResponse ResetForgottenPassword(ResetPasswordRequest request);
        BaseResponse SendResetMail(string mail);
        ResetForgottenPaswordResponse CheckResetAuth(string code);
        USER_ACCOUNT GetByResetAuth(string authCode);
        bool UpdateUserRole(EditRoleRequest request);
        USER_ACCOUNT GetById(int id);
        BaseResponse EditNameSurname(EditNameSurnameRequest request);

    }
}
