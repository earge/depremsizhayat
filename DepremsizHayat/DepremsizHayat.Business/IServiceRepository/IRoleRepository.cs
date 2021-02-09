using DepremsizHayat.Business.BaseRepository;
using DepremsizHayat.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DepremsizHayat.Business.IServiceRepository
{
    public interface IRoleRepository:IRepository<ROLE>
    {
        ROLE GetByName(string name);
    }
}
