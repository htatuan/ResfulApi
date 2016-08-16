using DomainModel.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel
{
    public class MyContext:DbContext
    {
        public DbSet<Mail> Mails { get; set; }
        public MyContext() :base("MyDBConnectionString")
        {
            Database.SetInitializer<MyContext>(new CreateDatabaseIfNotExists<MyContext>());
        }
    }
}
