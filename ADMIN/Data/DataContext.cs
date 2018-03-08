using ADMIN.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ADMIN.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<UsuariosDASHBOARDMIAUTO> Users { get; set; }

        public DbSet<Value> Values { get; set; }



    }
}
