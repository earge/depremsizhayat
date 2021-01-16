using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DepremsizHayat.Business.BaseRepository
{
    public interface IRepository<T> where T : class
    {
        T Add(T entity);
        T Update(T entity);
        bool Delete(T entity);
        T GetById(int id);
        List<T> GetAll();
    }
}
