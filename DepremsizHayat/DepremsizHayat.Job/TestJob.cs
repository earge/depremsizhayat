using DepremsizHayat.Business.IService;
using DepremsizHayat.Business.IServiceRepository;
using DepremsizHayat.DataAccess;
using DepremsizHayat.Utility;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DepremsizHayat.Job
{
    public class TestJob : IJob
    {
        public IUserService _userService;
        public TestJob(IUserService userService)
        {
            _userService = userService;
        }
        Task IJob.Execute(IJobExecutionContext context)
        {
            /* Mail Deneme */
            //var senderEmail = new MailAddress("Gönderen Mail", "Gönderici Adı");
            //var receiverEmail = new MailAddress("Alıcı Mail", "Alıcı Adı");
            //var password = "Mail Şifre";
            //var sub = "Zamanlanmış Mail";
            //var body = "Bu mail sistem tarafından deneme amaçlı otomatik olarak "+DateTime.Now.ToString()+" tarihinde gönderilmiştir. 30 saniyelik periyotlarla toplam 3 kez gönderilecektir.";
            //var smtp = new SmtpClient
            //{
            //    Host = "mail.depremsizhayat.com",
            //    Port = 587,
            //    EnableSsl = false,
            //    DeliveryMethod = SmtpDeliveryMethod.Network,
            //    UseDefaultCredentials = false,
            //    Credentials = new NetworkCredential(senderEmail.Address, password)
            //};
            //using (var mess = new MailMessage(senderEmail, receiverEmail)
            //{
            //    Subject = sub,
            //    Body = body
            //})
            //{
            //    smtp.Send(mess);
            //}
            return Task.CompletedTask;
        }
    }
}
