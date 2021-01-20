using DepremsizHayat.DataAccess;
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
        USER CreateUser(USER user);
        bool Login(string mail,string pwd);
        bool Activate(string actCode,string mail);
        USER GetByMail(string mail);
        bool ResetPassword(ResetPasswordRequest request);
        void SendResetMail(string mail);

    }
}
