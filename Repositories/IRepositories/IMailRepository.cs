using DomainModel.Entities;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Repositories.IRepositories
{
    public interface IMailRepository
    {
        IEnumerable<Mail> GetAll (Func<Mail, bool> predicate = null);
        Mail GetById(int id);
        void Create(Mail mail);
        void Update(Mail mail);
        void Delete(Mail mail);
    }
}
