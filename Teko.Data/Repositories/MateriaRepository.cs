﻿using Teko.Data.Infrastructure;
using Teko.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Teko.Data.Repositories
{
    public class MateriaRepository : RepositoryBase<Materias>, IMateriaRepository
    {
        public MateriaRepository(IDbFactory dbFactory)
            : base(dbFactory) { }
    }
    public interface IMateriaRepository : IRepository<Materias>
    {

    }
}
