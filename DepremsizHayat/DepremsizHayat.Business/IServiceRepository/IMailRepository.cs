using DepremsizHayat.Business.BaseRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DepremsizHayat.DataAccess;
using System.Net.Mail;

namespace DepremsizHayat.Business.IServiceRepository
{
    public interface IMailRepository:IRepository<E_MAIL_ACCOUNT>
    {
        bool SendMail(string senderCode, string receiverMail, string subject, string body);
        MailAddress GetMailInfo(string mailAddress);
        string GetMailByCode(string code);
        string GetTemplate(int id);
    }
}
