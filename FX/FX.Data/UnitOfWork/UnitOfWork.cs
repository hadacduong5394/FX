using FX.Context;
using FX.Data.DBFactory;
using System.Data.Entity;

namespace FX.Data.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDbFactory _dbFactory;
        private ContextConnection _dbContext;
        private DbContextTransaction _tranContext;

        public UnitOfWork(IDbFactory dbFactory)
        {
            this._dbFactory = dbFactory;
        }

        public ContextConnection DbContext
        {
            get { return _dbContext ?? (_dbContext = _dbFactory.Init()); }
        }

        public virtual void CommitChanges()
        {
            DbContext.SaveChanges();
        }

        public virtual void BeginTrans()
        {
            _tranContext = DbContext.Database.BeginTransaction();
        }

        public virtual void CommitTrans()
        {
            if (_tranContext != null)
            {
                _tranContext.Commit();
                _tranContext.Dispose();
            }
        }

        public virtual void RollbackTrans()
        {
            if (_tranContext != null)
            {
                _tranContext.Rollback();
                _tranContext.Dispose();
            }
        }

        public virtual void ClearProxy()
        {
            DbContext.Configuration.ProxyCreationEnabled = false;
        }
    }
}