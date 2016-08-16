using Serives.EmailService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Resful.Controllers
{
    [RoutePrefix("api/mails")]
    public class MailsController : ApiController
    {
        private IMailService _mailService;
        public MailsController()
        {
            _mailService = new MailService();
        }
        [HttpGet]
        [Route("")]
        public IList<MailDto> GetMails()
        {
            return _mailService.GetAll();
        }

        [HttpGet]
        [Route("{id}")]
        public HttpResponseMessage GetMail(int id)
        {
            var responseData = new DataResponose<MailDto>();
            try
            {
                _mailService = new MailService();
                var mail = _mailService.GetById(id);
                responseData.SetData(mail);
            }
            catch (EmailException ex)
            {
                responseData.Status = HttpStatusCode.InternalServerError;
                responseData.AddError(ex.Error);

            }
            return Request.CreateResponse<DataResponose<MailDto>>(HttpStatusCode.OK, responseData);
        }

        [HttpPost]
        [Route("")]
        public HttpResponseMessage Create([FromBody] MailDto mailDto)
        {
            var responseData = new DataResponose<MailDto>();
            try
            {
                if (_mailService.IsMailExisted(mailDto.To))
                {
                    responseData.Status = HttpStatusCode.Found;
                    responseData.AddError(new Error("mail.createEmail.emailIsExisted"));
                }
                else
                {
                    _mailService = new MailService();
                    // _mailService.Send(mailDto);
                    mailDto.Date = DateTime.Now;
                    _mailService.Create(mailDto);
                    responseData.SetData(mailDto);
                }
            }
            catch (EmailException ex)
            {
                responseData.Status = HttpStatusCode.InternalServerError;
                responseData.AddError(ex.Error);
                
            }
            catch
            {
                responseData.Status = HttpStatusCode.ExpectationFailed;
            }
            return Request.CreateResponse<DataResponose<MailDto>>(HttpStatusCode.OK, responseData);
        }
        [HttpPut]
        [Route("{id}")]
        public HttpResponseMessage Update([FromUri] int id, [FromBody] MailDto mail)
        {
            var responseData = new DataResponose<MailDto>();
            try
            {
                _mailService.Update(id, mail);
                responseData.SetData(mail);

            }
            catch (EmailException ex)
            {

                responseData.Status = HttpStatusCode.InternalServerError;
                responseData.AddError(ex.Error);
            }
            return Request.CreateResponse<DataResponose<MailDto>>(HttpStatusCode.OK, responseData);
        }
        [HttpDelete]
        [Route("{id}")]
        public HttpResponseMessage Delete(int id)
        {
            var responseData = new DataResponose<MailDto>();
            try
            {
                _mailService.Delete(id);

            }
            catch (EmailException ex)
            {

                responseData.Status = HttpStatusCode.InternalServerError;
                responseData.AddError(ex.Error);
            }
            return Request.CreateResponse<DataResponose<MailDto>>(HttpStatusCode.OK, responseData);
        }
    }

}
