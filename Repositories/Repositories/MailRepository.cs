using DomainModel;
using DomainModel.Entities;
using Repositories.IRepositories;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Repositories.Repositories
{
    public class MailRepository : IMailRepository
    {
        private MyContext context;
        private DbSet<Mail> dbSet;
        public MailRepository()
        {
            context = new MyContext();
            dbSet = context.Mails;
        }
        public MailRepository(MyContext context)
        {
            this.context = context;
            this.dbSet = this.context.Mails;
        }

        public IEnumerable<Mail> GetAll(Func<Mail,bool> predicate=null)
        {
            return dbSet;
        }

        public Mail GetById(int id)
        {
            return dbSet.Find(id);
        }

        public void Create(Mail mail)
        {
            dbSet.Add(mail);
        }

        public void Delete(Mail mail)
        {
            dbSet.Remove(mail);
        }

        public void Update(Mail mail)
        {
            context.Entry(mail).State= System.Data.Entity.EntityState.Modified;
        }
    }
}
