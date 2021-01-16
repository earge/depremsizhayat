using DepremsizHayat.Business.Factory;
using DepremsizHayat.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DepremsizHayat.Business.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private DepremsizHayatEntities _dbContext;
        private IDbFactory _dbFactory;
        public UnitOfWork(IDbFactory dbFactory)
        {
            this._dbFactory = dbFactory;
            _dbContext = this._dbFactory.Init();
        }
        public bool Commit()
        {
            return (_dbContext.SaveChanges() > 0) ? true : false;
        }
    }
}
