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
    public class FileRepository : Repository<FILE>, IFileRepository
    {
        public FileRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
