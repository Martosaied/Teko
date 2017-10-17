using Teko.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.ModelConfiguration;

namespace Teko.Data
{
    public class ContenidosConfiguration : EntityTypeConfiguration<Contenidos>
    {
        public ContenidosConfiguration()
        {
            this.HasKey(t => t.IsDeleted == false);
            this.ToTable("Contenidos");
        }
    }
}
