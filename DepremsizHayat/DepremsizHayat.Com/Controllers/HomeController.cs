using DepremsizHayat.Business.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace DepremsizHayat.Com.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        public void SendMail(string name, string mail, string phone, string subject, string message)
        {
            var body = "Gönderen: " + name;
            body = body + " Telefon: " + phone;
            body = body + "Mesaj: " + message;
            var receiver = /*_mailService.GetMailByCode("app")*/"ytgokk@gmail.com";
            var smtp = new SmtpClient
            {
                Host = "mail.depremsizhayat.com",
                Port = 587,
                EnableSsl = false,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(receiver, "8PxAVcBL3PyKt6fZ")
            };
            using (var mess = new MailMessage(new MailAddress("app@depremsizhayat.com", name), new MailAddress(receiver,"Depremsiz Hayat"))
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
            {
                smtp.Send(mess);
            }
        }
    }
}