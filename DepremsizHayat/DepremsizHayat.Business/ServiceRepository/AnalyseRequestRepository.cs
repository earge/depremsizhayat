using DepremsizHayat.Business.BaseRepository;
using DepremsizHayat.Business.Factory;
using DepremsizHayat.Business.IServiceRepository;
using DepremsizHayat.DataAccess;
using DepremsizHayat.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DepremsizHayat.Business.ServiceRepository
{
    public class AnalyseRequestRepository : Repository<ANALYSE_REQUEST>, IAnalyseRequestRepository
    {
        private IUserAnalyseRequestRepository _userAnalyseRequestRepository;
        private IUserRepository _userRepository;
        public AnalyseRequestRepository(IDbFactory dbFactory,
            IUserAnalyseRequestRepository userAnalyseRequestRepository,
            IUserRepository userRepository) : base(dbFactory)
        {
            this._userAnalyseRequestRepository = userAnalyseRequestRepository;
            this._userRepository = userRepository;
        }

        public ANALYSE_REQUEST GetByUniqueCode(string code)
        {
            return _dbContext.ANALYSE_REQUEST.FirstOrDefault(p => p.UNIQUE_KEY == code);
        }

        public bool IsUniqueCodeExist(string code)
        {
            if (_dbContext.ANALYSE_REQUEST.FirstOrDefault(P => P.UNIQUE_KEY == code) == null)
                return false;
            else
                return true;
        }
    }
}
