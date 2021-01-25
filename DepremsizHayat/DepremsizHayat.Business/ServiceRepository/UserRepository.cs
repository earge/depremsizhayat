using DepremsizHayat.Business.BaseRepository;
using DepremsizHayat.Business.Factory;
using DepremsizHayat.Business.IServiceRepository;
using DepremsizHayat.DataAccess;
using System;
using System.Data.SqlClient;
using System.Linq;
using System.Data;
using System.Net.Mail;
using System.Net;
using DepremsizHayat.DTO.Models;

namespace DepremsizHayat.Business.ServiceRepository
{
    public class UserRepository : Repository<USER_ACCOUNT>, IUserRepository
    {
        public UserRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
        public USER_ACCOUNT CreateUser(UserModel user)
        {
            if (GetByMail(user.E_MAIL)==null)
            {
                SqlParameter[] @params =
            {
                new SqlParameter("@ROLE_ID", user.ROLE_ID),
                new SqlParameter("@FIRST_NAME", user.FIRST_NAME),
                new SqlParameter("@LAST_NAME", user.LAST_NAME),
                new SqlParameter("@E_MAIL", user.E_MAIL),
                new SqlParameter("@PASSWORD", user.PASSWORD),
                new SqlParameter("@ACTIVATION_CODE", user.ACTIVATION_CODE),
                new SqlParameter("@CREATED_DATE", user.CREATED_DATE),
                new SqlParameter("@ACTIVE", user.ACTIVE),
                new SqlParameter("@DELETED", user.DELETED)
            };
                _dbContext.Database.ExecuteSqlCommand("SP_USER_ADD" +
                    " @ROLE_ID," +
                    " @FIRST_NAME," +
                    " @LAST_NAME," +
                    " @E_MAIL," +
                    " @PASSWORD," +
                    " @ACTIVATION_CODE," +
                    " @CREATED_DATE," +
                    " @ACTIVE," +
                    " @DELETED",
                    @params);
                USER_ACCOUNT rUser = GetByMail(user.E_MAIL);
                if (user != null)
                {
                    bool sit = SendMail(user.E_MAIL, 
                        "E-Posta Doğrulama", 
                        "Depremsiz Hayat'a hoşgeldiniz "+user.FIRST_NAME+" "+user.LAST_NAME+"! E-Postanızı doğrulamak için linke tıklayabilirsiniz: "+
                        "/Account/Activate?actCode="+user.ACTIVATION_CODE+"&mail="+Security.Encryptor.Encrypt(user.E_MAIL)+
                        " <br/>Doğrulama kodunuz: "+ Security.Decryptor.Decrypt(user.ACTIVATION_CODE));
                    if (sit==true)
                    {
                        return rUser;
                    }
                    else
                    {
                        _dbContext.USER.Remove(rUser);
                        return null;
                    }
                }
            }
            return null;
        }
        public USER_ACCOUNT GetByMail(string mail)
        {
            return _dbContext.USER.FirstOrDefault(p => p.E_MAIL == mail);
        }
        public bool Login(UserModel user)
        {
            SqlParameter[] @params =
            {

                new SqlParameter("@E_MAIL", user.E_MAIL),
                new SqlParameter("@PASSWORD", user.PASSWORD),
                new SqlParameter("@RETURN",SqlDbType.Int){ Direction=ParameterDirection.Output}
            };

            _dbContext.Database.ExecuteSqlCommand("SP_USER_LOGIN_CONTROL @E_MAIL,@PASSWORD, @RETURN out", @params);
            if ((int)@params[2].Value == 1)
            {
                return true;
            }
            return false;
        }
        public bool SendMail(string mail, string subject, string body)
        {
            try
            {
                var senderEmail = new MailAddress("yigit.gok@depremsizhayat.com", "Depremsiz Hayat");
                var receiverEmail = new MailAddress(mail, mail);
                var password = "37665445";
                var smtp = new SmtpClient
                {
                    Host = "mail.depremsizhayat.com",
                    Port = 587,
                    EnableSsl = false,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(senderEmail.Address, password)
                };
                using (var mess = new MailMessage(senderEmail, receiverEmail)
                {
                    Subject = subject,
                    Body = body
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
