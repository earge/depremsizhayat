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
    public class AnalyseRequestRepository : Repository<ANALYSE_REQUEST>, IAnalyseRequestRepository
    {
        public AnalyseRequestRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
