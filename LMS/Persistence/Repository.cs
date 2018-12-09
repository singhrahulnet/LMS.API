using System;
using System.Collections.Generic;

namespace LMS.Persistence
{
    public interface IRepository<T>
    {
        IEnumerable<T> Get();
        T GetById(int id);
        void Update(T entity);
        void Add(T entity);
        void AddRange(List<T> entities);
    }
    public class Repository<T> : IRepository<T> where T : class
    {
        IDbContext _ctx;
        public Repository(IDbContext ctx)
        {
            _ctx = ctx;
        }
        public void Add(T entity)
        {
            try
            {
                _ctx.Set<T>().Add(entity);
            }
            catch (Exception)
            {
                //shout/catch/throw/log
            }
        }

        public void AddRange(List<T> entities)
        {
            try
            {
                _ctx.Set<T>().AddRange(entities);
            }
            catch (Exception)
            {
                //shout/catch/throw/log
            }
        }

        public IEnumerable<T> Get()
        {
            try
            {
                return _ctx.Set<T>();
            }
            catch (Exception)
            {
                //shout/catch/throw/log
            }
            return null;
        }

        public T GetById(int id)
        {
            try
            {
                return _ctx.Set<T>().Find(id);
            }
            catch (Exception)
            {
                //shout/catch/throw/log
            }
            return null;
        }

        public void Update(T entity)
        {
            try
            {
                _ctx.Set<T>().Attach(entity);
            }
            catch (Exception)
            {
                //shout/catch/throw/log
            }
        }
    }
}
