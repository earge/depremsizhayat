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
using DepremsizHayat.DTO.User;
using DepremsizHayat.Security;
using System.Collections.Generic;

namespace DepremsizHayat.Business.ServiceRepository
{
    public class UserRepository : Repository<USER_ACCOUNT>, IUserRepository
    {
        private IMailRepository _mailRepository;
        private IUserAnalyseRequestRepository _userAnalyseRequestRepository;
        public UserRepository(IDbFactory dbFactory,
            IMailRepository mailRepository,
            IUserAnalyseRequestRepository userAnalyseRequestRepository) : base(dbFactory)
        {
            this._mailRepository = mailRepository;
            this._userAnalyseRequestRepository = userAnalyseRequestRepository;
        }
        public USER_ACCOUNT GetByResetAuth(string authCode)
        {
            return _dbContext.USER_ACCOUNT.FirstOrDefault(p => p.PASSWORD_RESET_HELPER == authCode);
        }
        public bool CreateForgottenPwdResetRequest(string mail)
        {
            SqlParameter[] @params =
            {

                new SqlParameter("@E_MAIL", mail),
                new SqlParameter("@RETURN",SqlDbType.Int){ Direction=ParameterDirection.Output}
            };
            _dbContext.Database.ExecuteSqlCommand("exec @RETURN=SP_FORGET_PASSWORD_RESET @E_MAIL", @params);
            if ((int)@params[1].Value == 1)
            {
                return true;
            }
            return false;
        }
        public USER_ACCOUNT CreateUser(UserModel user)
        {
            if (GetByMail(user.E_MAIL) == null)
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
                    var subject = "E-Posta Doğrulama";
                    var body = "Depremsiz Hayat'a hoşgeldiniz " + user.FIRST_NAME + " " + user.LAST_NAME + "! E-Postanızı doğrulamak için linke tıklayabilirsiniz: " +
                        "http://app.depremsizhayat.com/Account/Activate?actCode=" + user.ACTIVATION_CODE + "&mail=" + Security.Encryptor.Encrypt(user.E_MAIL) +
                        " <br/>Doğrulama kodunuz: " + Security.Decryptor.Decrypt(user.ACTIVATION_CODE);
                    if (_mailRepository.SendMail("app", user.E_MAIL, subject, body) == true)
                        return rUser;
                    else
                    {
                        Delete(rUser);
                        _dbContext.SaveChanges();
                        return null;
                    }
                }
            }
            return null;
        }
        public USER_ACCOUNT GetByMailForProcedure(string mail)
        {
            return _dbContext.USER_ACCOUNT.AsNoTracking().FirstOrDefault(p => p.E_MAIL == mail);
        }
        public USER_ACCOUNT GetByMail(string mail)
        {
            return _dbContext.USER_ACCOUNT.FirstOrDefault(p => p.E_MAIL == mail);
        }
        public bool Login(UserModel user)
        {
            SqlParameter[] @params =
            {

                new SqlParameter("@E_MAIL", user.E_MAIL),
                new SqlParameter("@PASSWORD", user.PASSWORD),
                new SqlParameter("@RETURN",SqlDbType.Int){ Direction=ParameterDirection.Output}
            };
            _dbContext.Database.ExecuteSqlCommand("exec @RETURN = SP_USER_LOGIN_CONTROL @E_MAIL,@PASSWORD", @params);
            if ((int)@params[2].Value == 1)
            {
                return true;
            }
            return false;
        }
        public bool ResetForgottenPassword(ResetPasswordRequest request)
        {
            SqlParameter[] @params =
           {
                new SqlParameter("@PASSWORD_RESET_HELPER", request.PASSWORD_RESET_HELPER),
                new SqlParameter("@NEW_PASSWORD", request.NewPassword),
                new SqlParameter("@E_MAIL", request.Mail),
                new SqlParameter("@RETURN",SqlDbType.Int){ Direction=ParameterDirection.Output}
            };
            _dbContext.Database.ExecuteSqlCommand("exec @RETURN=SP_AFTER_FORGET_PASSWORD_RESET @PASSWORD_RESET_HELPER,@NEW_PASSWORD,@E_MAIL", @params);
            if ((int)@params[3].Value == 1)
            {
                return true;
            }
            return false;
        }

        public USER_ACCOUNT GetRandomExpertForAnalyse(int? alreadyAssigned)
        {
            //Random random = new Random();
            //int id;
            int roleId = _dbContext.ROLE.FirstOrDefault(p => p.NAME == "Expert").ROLE_ID;
            List<USER_ACCOUNT> list = _dbContext.USER_ACCOUNT
                .Where(p => p.ROLE_ID == roleId)
                .OrderBy(p=>p.LAST_ANSWER_DATE)
                .ToList();
            foreach (USER_ACCOUNT user in list)
            {
                //id = random.Next(0, list.Max(p => p.USER_ACCOUNT_ID));
                var expert = GetById(user.USER_ACCOUNT_ID);
                if (expert != null)
                {
                    var maxAnswerCount = expert.MAX_COUNT_REQUEST;
                    int totalAssignmentsCount = _userAnalyseRequestRepository.GetExpertsActiveRequests(expert.USER_ACCOUNT_ID).Count();
                    if (totalAssignmentsCount<expert.MAX_COUNT_REQUEST)
                    {
                        if (alreadyAssigned!=null && alreadyAssigned==expert.USER_ACCOUNT_ID)
                        {
                            return expert;
                        }
                    }
                }
            }
            return null;
        }
        public bool ResetPassword(string mail,string pwd)
        {
            SqlParameter[] @params =
            {
                new SqlParameter("@E_MAIL", mail),
                new SqlParameter("@PASSWORD_NEW", pwd),
                new SqlParameter("@RETURN",SqlDbType.Int){ Direction=ParameterDirection.Output}
            };
            _dbContext.Database.ExecuteSqlCommand("exec @RETURN = SP_PASSWORD_RESET @E_MAIL, @PASSWORD_NEW", @params);
            if ((int)@params[2].Value == 1)
            {
                return true;
            }
            return false;
        }

        public List<USER_ACCOUNT> GetAdmins()
        {
            return _dbContext.USER_ACCOUNT.Where(p => p.ROLE.NAME == Resources.RoleCodes.Admin).ToList();
        }
    }
}
