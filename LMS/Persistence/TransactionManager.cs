using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMS.Persistence
{
    public interface ITransactionManager
    {
        IRepository<T> Create<T>();
        int Save();
    }
    public class TransactionManager : ITransactionManager
    {
        IDbContext _dbContext;
        Hashtable _repositories;
        public TransactionManager(IDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IRepository<T> Create<T>()
        {
            if (_repositories == null)
                _repositories = new Hashtable();
            var type = typeof(T).Name;
            if(!_repositories.Contains(type))
            {
                var repoType = typeof(Repository<>);
                var repositoryInstance = Activator.CreateInstance(repoType.MakeGenericType(typeof(T)), _dbContext);
                _repositories.Add(type, repositoryInstance);
            }
            return (IRepository<T>)_repositories[type];
        }

        public int Save()
        {
            return _dbContext.Save();
        }
    }
}
