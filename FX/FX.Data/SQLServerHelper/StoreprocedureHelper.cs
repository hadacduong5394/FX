using FX.Core;
using FX.Data.UnitOfWork;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;

namespace FX.Data.SQLServerHelper
{
    public class StoreprocedureHelper<T> where T : class
    {
        public DbRawSqlQuery<T> ExecuteStoreprocedure(string sqlQuery, SqlParameter[] parameters)
        {
            return IoC.Resolve<IUnitOfWork>().DbContext.Database.SqlQuery<T>(sqlQuery, parameters);
        }
    }
}