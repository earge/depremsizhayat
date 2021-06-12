using DepremsizHayat.Business.BaseRepository;
using DepremsizHayat.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DepremsizHayat.Business.IServiceRepository
{
    public interface IAnalyseRequestRepository : IRepository<ANALYSE_REQUEST>
    {
        ANALYSE_REQUEST GetByUniqueCode(string code);
        bool IsUniqueCodeExist(string code);
        IOrderedQueryable<ANALYSE_REQUEST> GetByUserIdDescendingDate(int userId);
    }
}
