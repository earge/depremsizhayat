using DepremsizHayat.DataAccess;
using DepremsizHayat.DTO.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DepremsizHayat.Business.IService
{
    public interface IRoleService
    {
        List<ROLE> GetAll();
        int GetIdByName(string name);
    }
}
