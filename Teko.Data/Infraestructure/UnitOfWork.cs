using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Teko.Data.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDbFactory dbFactory;
        private DbEntities dbContext;

        public UnitOfWork(IDbFactory dbFactory)
        {
            this.dbFactory = dbFactory;
        }

        public DbEntities DbContext
        {
            get { return dbContext ?? (dbContext = dbFactory.CrearContexto()); }
        }

        public void Commit()
        {
            DbContext.Commit();
        }

        public DbEntities GetContext()
        {
            return this.dbContext = dbFactory.CrearContexto();
        }
    }
}
