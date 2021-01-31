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
    public class StatusRepository : Repository<STATUS>, IStatusRepository
    {
        public StatusRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public STATUS GetByName(string name)
        {
            return _dbContext.STATUS.FirstOrDefault(p => p.NAME == name);
        }
    }
}
