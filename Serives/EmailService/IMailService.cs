using System.Collections;
using System.Collections.Generic;

namespace Serives.EmailService
{
    public interface IMailService
    {
        IList<MailDto> GetAll();
        MailDto GetById(int id);
        void Create(MailDto mail);
        void Send(MailDto mail);
        bool IsMailExisted(string user);
        void Update(int id, MailDto mailDto);
        void Delete(int id);
    }
}
