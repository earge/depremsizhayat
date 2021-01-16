using DepremsizHayat.DataAccess;
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
    }
}
