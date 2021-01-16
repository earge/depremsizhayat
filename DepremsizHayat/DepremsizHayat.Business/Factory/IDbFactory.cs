using DepremsizHayat.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DepremsizHayat.Business.Factory
{
    public interface IDbFactory
    {
        DepremsizHayatEntities Init();
        void Dispose(bool disposeState);
    }
}
