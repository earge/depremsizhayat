using DepremsizHayat.Business.IService;
using DepremsizHayat.Business.IServiceRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DepremsizHayat.Business.Service
{
    public class MailService : IMailService
    {
        private IMailRepository _mailRepository;
        public MailService(IMailRepository mailRepository)
        {
            this._mailRepository = mailRepository;
        }
        public bool SendMail(string receiver, string subject, string body)
        {
            return _mailRepository.SendMail("app",receiver, subject, body);
        }
        public string GetMailByCode(string code)
        {
            return _mailRepository.GetMailByCode(code);
        }
    }
}
