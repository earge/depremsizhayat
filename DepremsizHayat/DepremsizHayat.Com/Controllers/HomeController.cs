using DepremsizHayat.Business.IService;
using DepremsizHayat.DTO.User;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
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
            body = body + " Mail: " + mail;
            body = body + "Mesaj: " + message;
            var receiver = "app@depremsizhayat.com";
            var smtp = new SmtpClient
            {
                Host = "mail.depremsizhayat.com",
                Port = 587,
                EnableSsl = false,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(receiver, "8PxAVcBL3PyKt6fZ")
            };
            using (var mess = new MailMessage(new MailAddress(mail, name), new MailAddress(receiver, "Depremsiz Hayat"))
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
            {
                smtp.Send(mess);
            }
        }
        public ActionResult GetResult(string code)
        {
            string apiUrl = "https://localhost:44303/api/Request?code=" + code;
            var apiData = new WebClient() { Encoding = Encoding.UTF8 }.DownloadString(apiUrl);
            var token = JToken.Parse(apiData);
            var request = token.ToObject<MyAnalyseRequest>();
            return View(request);
        }
        public ActionResult CallResult()
        {
            return View();
        }
    }
}