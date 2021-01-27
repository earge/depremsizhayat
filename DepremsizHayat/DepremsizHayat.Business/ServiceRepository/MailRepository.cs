using DepremsizHayat.Business.BaseRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DepremsizHayat.DataAccess;
using DepremsizHayat.Business.IServiceRepository;
using DepremsizHayat.Business.Factory;
using System.Net.Mail;
using System.Net;

namespace DepremsizHayat.Business.ServiceRepository
{
    public class MailRepository : Repository<E_MAIL_ACCOUNT>, IMailRepository
    {
        public MailRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public MailAddress GetMailInfo(string mailAddress)
        {
            if (mailAddress.Split('@')[1] == "depremsizhayat.com")
            {
                return new MailAddress(mailAddress, "Depremsiz Hayat");
            }
            USER_ACCOUNT receiver = _dbContext.USER_ACCOUNT.FirstOrDefault(p => p.E_MAIL == mailAddress);
            return new MailAddress(mailAddress, receiver.FIRST_NAME + " " + receiver.LAST_NAME);
        }
        public bool SendMail(string senderCode,string receiverMail, string subject, string body)
        {
            try
            {
                E_MAIL_ACCOUNT emailAccount = _dbContext.E_MAIL_ACCOUNT.FirstOrDefault(p => p.E_MAIL_ACCOUNT_CODE == senderCode);
                var smtp = new SmtpClient
                {
                    Host = emailAccount.HOST,
                    Port = emailAccount.PORT,
                    EnableSsl = (emailAccount.ENABLE_SSL != null) ? (bool)emailAccount.ENABLE_SSL : false,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(emailAccount.USER_NAME, emailAccount.PASSWORD)
                };
                using (var mess = new MailMessage(GetMailInfo(emailAccount.USER_NAME), GetMailInfo(receiverMail))
                {
                    Subject = subject,
                    Body = body,
                    IsBodyHtml=true
                })
                {
                    smtp.Send(mess);
                }
                return true;
        }
            catch (Exception)
            {
                return false;
            }
}
    }
}
