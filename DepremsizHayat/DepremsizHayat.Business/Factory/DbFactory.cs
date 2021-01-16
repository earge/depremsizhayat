using DepremsizHayat.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DepremsizHayat.Business.Factory
{
    public class DbFactory : IDisposable, IDbFactory
    {
        DepremsizHayatEntities _dbContext;
        public void Dispose(bool disposeState)
        {
            if (disposeState==true)
            {
                _dbContext.Dispose();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        public DepremsizHayatEntities Init()
        {
            return _dbContext ?? (_dbContext = new DepremsizHayatEntities());
        }
    }
}
