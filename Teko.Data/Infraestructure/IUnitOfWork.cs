using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Teko.Data.Infrastructure
{
    public interface IUnitOfWork
    {
        DbEntities GetContext();
        void Commit();
    }
}
