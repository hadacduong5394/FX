using FX.Context;

namespace FX.Data.UnitOfWork
{
    public interface IUnitOfWork
    {
        ContextConnection DbContext { get; }

        void CommitChanges();

        void BeginTrans();

        void CommitTrans();

        void RollbackTrans();

        void ClearProxy();
    }
}