using DepremsizHayat.Business.Factory;
using DepremsizHayat.DataAccess;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DepremsizHayat.Business.BaseRepository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected DepremsizHayatEntities _dbContext;
        private IDbFactory _dbFactory;
        private IDbSet<T> _dbSet;
        public Repository(IDbFactory dbFactory)
        {
            this._dbFactory = dbFactory;
            this._dbContext = this._dbFactory.Init();
            this._dbSet = _dbContext.Set<T>();
        }
        public T Add(T entity)
        {
            _dbSet.Add(entity);
            return entity;
        }

        public bool Delete(T entity)
        {
            //_dbSet.Attach(entity);
            _dbSet.Remove(entity);
            return true;
        }

        public List<T> GetAll()
        {
            return _dbSet.ToList();
        }

        public T GetById(int id)
        {
            return _dbSet.Find(id);
        }

        public T Update(T entity)
        {
            _dbSet.Attach(entity);
            //_dbContext.Entry(entity).State=EntityState.Modified;
            return entity;
        }
    }
}
