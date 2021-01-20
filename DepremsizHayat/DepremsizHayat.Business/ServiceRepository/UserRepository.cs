using DepremsizHayat.Business.BaseRepository;
using DepremsizHayat.Business.Factory;
using DepremsizHayat.Business.IServiceRepository;
using DepremsizHayat.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DepremsizHayat.Business.ServiceRepository
{
    public class UserRepository : Repository<USER>, IUserRepository
    {
        public UserRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public USER GetByMail(string mail)
        {
            return _dbContext.USER.FirstOrDefault(p => p.E_MAIL == mail);
        }

        public bool SendMail(string mail, string subject, string body)
        {
            try
            {
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
