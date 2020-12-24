using DepremsizHayat.Data;
using DepremsizHayat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace DepremsizHayat.Business
{
    public class UserAccountManager : BaseManager
    {
        public void CreateUser(UserAccountCreateModel model)
        {
            if (!IsEmailExist(model.E_MAIL))
            {
                string act = Guid.NewGuid().ToString();
                _db.USER_ACCOUNT.Add(new USER_ACCOUNT
                {
                    E_MAIL = model.E_MAIL,
                    FIRST_NAME = model.FIRST_NAME,
                    PASSWORD = model.PASSWORD,
                    USER_ROLE_ID = model.ROLE_ID,
                    LAST_NAME = model.LAST_NAME,
                    ACTIVATIONCODE = act
                }); ;
                if (_db.SaveChanges() > 0)
                {
                    var smtpClient = new SmtpClient("smtp.gmail.com", 587)
                    {
                        EnableSsl = true,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential("fortestmvc@gmail.com", "AbcAbc12_")
                    };

                    smtpClient.Send("fortestmvc@gmail.com", model.E_MAIL, "E-Posta Doğrulama", "/Register/Activate?actCode=" + act + "&email=" + model.E_MAIL);
                }
            }
            else
            {
                model.ErrorCodes.Add("409");
            }
        }

        private bool IsEmailExist(string mail)
        {
            bool dummy;
            dummy = (_db.USER_ACCOUNT.FirstOrDefault(p => p.E_MAIL == mail) == null) ? false : true;
            return dummy;
        }
        public string Activate(string code, string email)
        {
            if (_db.USER_ACCOUNT.FirstOrDefault(p => p.E_MAIL == email) != null)
            {
                DepremsizHayat.Data.USER_ACCOUNT user = _db.USER_ACCOUNT.FirstOrDefault(p => p.E_MAIL == email);
                if (user.ACTIVATIONCODE == code)
                {
                    if (user.IS_ACTIVE == false)
                    {
                        user.IS_ACTIVE = true;
                        _db.SaveChanges();
                    }
                    else
                    {
                        return "Hesabınız zaten doğrulanmış.";
                    }
                    return "Doğrulandı";
                }
                else
                {
                    return "Sayfa Bulunamadı";
                }
            }
            else
            {
                return "Sayfa Bulunamadı";
            }
        }
    }
}
