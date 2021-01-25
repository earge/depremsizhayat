using DepremsizHayat.Business.BaseRepository;
using DepremsizHayat.DataAccess;
using DepremsizHayat.DTO.Models;
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
        bool SendMail(string mail,string subject,string body);
        USER_ACCOUNT CreateUser(UserModel user);
        bool Login(UserModel user);
    }
}
