using DomainModel.Entities;
using Repositories.IRepositories;
using Repositories.Repositories;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.Linq;
using Repositories;

namespace Serives.EmailService
{
    public class MailService : IMailService
    {
        private string _emailPassword;
        private IMailRepository mailRepository;
        public MailService()
        {
            _emailPassword = ConfigurationManager.AppSettings["passWord"];
            mailRepository = new MailRepository();
        }

        public IList<MailDto> GetAll()
        {
            return mailRepository.GetAll().Select(p =>
            {
                return new MailDto()
                {
                    Id = p.Id,
                    To = p.To,
                    Subject = p.Subject,
                    Message = p.Message,
                    Date = p.Date
                };
            }).ToList();
        }

        public void Send(MailDto mail)
        {
            try
            {

                var client = new SmtpClient("smtp.gmail.com", 587)
                {
                    Credentials = new NetworkCredential("hoangthoanhtuan@gmail.com", _emailPassword),
                    EnableSsl = true

                };
                client.Send("hoangthoanhtuan@gmail.com", mail.To, mail.Subject, mail.Message);


            }
            catch
            {
                var exception = new EmailException("");

                throw exception;

            }
        }

        public MailDto GetById(int id)
        {
            var mail = mailRepository.GetById(id);
            return new MailDto()
            {
                Id = mail.Id,
                To = mail.To,
                Subject = mail.Subject,
                Message = mail.Message,
                Date = mail.Date
            };
        }

        public void Create(MailDto mailDto)
        {
            if (string.IsNullOrEmpty(mailDto.Subject))
                throw new EmailException(new Error("mail.createMail.subjectIsRequired"));
            using (var unitOfWork = new UnitOfWork())
            {
                var repository = unitOfWork.MailRepository;
                var mail = new Mail()
                {
                    Id = mailDto.Id,
                    To = mailDto.To,
                    Subject = mailDto.Subject,
                    Message = mailDto.Message,
                    Date = mailDto.Date
                };
                repository.Create(mail);
                unitOfWork.SaveChanges();
            }
        }

        public bool IsMailExisted(string user)
        {
            return mailRepository.GetAll().FirstOrDefault(p => p.To == user) != null;
        }

        public void Update(int id, MailDto mailDto)
        {
            
           

            using (var unitOfwork = new UnitOfWork())
            {
                var reponsitory = unitOfwork.MailRepository;
                var mail = reponsitory.GetById(id);
                if (mail == null)
                {
                    throw new EmailException(new Error("mail.updateMail.emailisNotExisted"));
                }
                try
                {
                    mail.Date = mailDto.Date;
                    mail.To = mailDto.To;
                    mail.Subject = mailDto.Subject;
                    reponsitory.Update(mail);
                    unitOfwork.SaveChanges();
                }
                catch
                {

                    throw new EmailException(new Error("mail.updateMail.unexpectedError"));
                }
            
            }

        }
        public void Delete(int id)
        {
            using (var unitOfWork = new UnitOfWork())
            {
                try
                {
                    var reponsitory = unitOfWork.MailRepository;
                    var mail = reponsitory.GetById(id);
                    if (mail == null)
                        throw new EmailException(new Error("mail.deleteEmail.emailisNotExisted"));
                    reponsitory.Delete(mail);
                    unitOfWork.SaveChanges();

                }
                catch 
                {

                    throw new EmailException(new Error("mail.deleteEmail.unexpectedError"));
                }
            }
        }
    }
    public class EmailException : Exception
    {
        public EmailException(string message) : base(message)
        {

        }
        public EmailException(Error error)
        {
            this.Error = error;
        }
        public Error Error { get; set; }
    }
    public class Error
    {
        public Error(string errorCode)
        {
            this.ErrorCode = errorCode;
         
        }
        public string ErrorCode { get; set; }
    }




    public class DataResponose<T>
    {
        public DataResponose(T mail, HttpStatusCode httpStatusCode, List<Error> errors)
        {
            this.Data = mail;
            this.Status = httpStatusCode;
            this.Errors = errors;
        }
        public T Data { get; set; }
        public HttpStatusCode Status { get; set; }
        public IList<Error> Errors { get; set; }
        public DataResponose()
        {
            this.Status = HttpStatusCode.OK;
            this.Errors = new List<Error>();
        }
        public void SetData(T data)
        {
            this.Data = data;
        }
        public void AddError(Error error)
        {
            this.Errors.Add(error);
        }
    }
}
